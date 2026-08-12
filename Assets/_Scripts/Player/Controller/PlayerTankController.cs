using UnityEngine;
using Photon.Pun;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerTankController : MonoBehaviourPun, IPunObservable
{
    [Tooltip("Main body SpriteRenderer used to display the player's tank base sprite")]
    [SerializeField] private SpriteRenderer bodyRenderer;

    [Tooltip("Upper part SpriteRenderer used to render the rotating tank cannon or turret")]
    [SerializeField] private SpriteRenderer upperPartBodyRenderer;

    [Tooltip("ScriptableObject that holds all the configuration data (speed, fire rate, explosion, etc.) for the player's tank")]
    public PlayerTankModel modelData;

    [Tooltip("The View component attached to the Player tank that handles movement, shooting, and physics")]
    public PlayerTankView view;

    [Tooltip("Player health handler for managing hit points and death logic")]
    public PlayerHealth health;

    private PlayerInputActions input;

    [Tooltip("Stores directional input from the player (WASD/arrow keys/joystick)")]
    private Vector2 moveInput;

    [Tooltip("True when fire input (spacebar, button) is pressed")]
    private bool firePressed;

    [Tooltip("Internal timer used to enforce firing cooldown")]
    private float lastFiredTime = -999f;

    [Tooltip("Read/write access to the ScriptableObject-based PlayerTankModel data")]
    public PlayerTankModel Model => modelData;

    [Tooltip("Checks if enough time has passed since the last shot to allow firing again")]
    public bool CanFire() => Time.time >= lastFiredTime + Model.fireCooldown;

    [Tooltip("Records the current time as the moment the player fired, used for enforcing cooldown")]
    public void RecordFireTime() => lastFiredTime = Time.time;

    // ---------- Networking / smoothing ----------
    private Rigidbody2D rb;

    // networked values (received)
    private Vector2 networkPosition;
    private Vector2 networkVelocity;
    private float networkRotationZ;

    // smoothing
    [Tooltip("Interpolation speed for remote players")]
    [SerializeField] private float lerpRate = 10f;

    // cached delegates so we can unsubscribe cleanly
    private Action<UnityEngine.InputSystem.InputAction.CallbackContext> onMovePerformed;
    private Action<UnityEngine.InputSystem.InputAction.CallbackContext> onMoveCanceled;
    private Action<UnityEngine.InputSystem.InputAction.CallbackContext> onFirePerformed;

    private void Awake()
    {
        input = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();

        // cache delegates to allow proper unsubscribe
        onMovePerformed = ctx => moveInput = ctx.ReadValue<Vector2>();
        onMoveCanceled = ctx => moveInput = Vector2.zero;
        onFirePerformed = ctx => firePressed = true;
    }

    private void OnEnable()
    {
        // Do not automatically enable input here because photonView ownership may not be ready
        // Input enabling is handled in Start() after ownership check
    }

    private void OnDisable()
    {
        // Ensure input is disabled and unsubscribed
        if (input != null)
        {
            try
            {
                input.Gameplay.Move.performed -= onMovePerformed;
                input.Gameplay.Move.canceled -= onMoveCanceled;
                input.Gameplay.Fire.performed -= onFirePerformed;
            }
            catch { /* ignore if already unsubscribed */ }

            input.Disable();
        }
    }

    void Start()
    {
        Debug.Log($"[PlayerTankController] Start() for {gameObject.name}");

        // Safe call to PhotonManager.LogPhotonInfo (avoid SendMessage and null refs)
        var pm = FindObjectOfType<PhotonManager>();
        if (pm != null)
        {
            pm.LogPhotonInfo();
        }
        else
        {
            Debug.LogWarning("[PlayerTankController] PhotonManager not found - skipping LogPhotonInfo.");
        }

        Init();

        // Ownership gating: enable input for local owner OR for offline/local instance
        // Treat "not connected" or "not in room" as local so offline single-player works.
        bool isLocalOwner = false;

        if (photonView != null)
        {
            isLocalOwner = photonView.IsMine;
        }

        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
        {
            // offline mode (or not yet in a room) -> treat as local owner
            isLocalOwner = true;
        }

        if (isLocalOwner)
        {
            EnableInput();
            // photonView.Owner might be null in offline/local cases — defend against that
            string ownerStr = (photonView != null && photonView.Owner != null) ? photonView.Owner.ActorNumber.ToString() : "local";
            Debug.Log($"[PlayerTankController] This is local player & Input enabled for local player (owner: {ownerStr}).");
        }
        else
        {
            // initialize network targets so remote instances don't snap
            networkPosition = rb.position;
            networkVelocity = rb.velocity;
            networkRotationZ = transform.rotation.eulerAngles.z;
            Debug.Log($"[PlayerTankController] This is a remote player & Remote instance. Owner actor: {photonView.Owner?.ActorNumber ?? -1}");
        }
    }

    private void EnableInput()
    {
        if (input == null) input = new PlayerInputActions();

        input.Enable();
        input.Gameplay.Move.performed += onMovePerformed;
        input.Gameplay.Move.canceled += onMoveCanceled;
        input.Gameplay.Fire.performed += onFirePerformed;

        Debug.Log("[PlayerTankController] EnableInput called - input enabled and callbacks bound.");
    }

    private void DisableInput()
    {
        if (input == null) return;

        try
        {
            input.Gameplay.Move.performed -= onMovePerformed;
            input.Gameplay.Move.canceled -= onMoveCanceled;
            input.Gameplay.Fire.performed -= onFirePerformed;
        }
        catch { }
        input.Disable();
    }

    // Call this also on condition if wherever you spawn or initialize the player prefab
    public void Init()
    {
        if (view != null)
            view.SetController(this);

        if (health != null)
            health.Init(this, Model.maxHealth);

        if (bodyRenderer != null && Model != null && Model.tankSprite != null)
        {
            bodyRenderer.sprite = Model.tankSprite;
            upperPartBodyRenderer.sprite = Model.upperTankSprite;
        }

        if (bodyRenderer != null && Model != null && Model.tankSpriteColor != null)
        {
            bodyRenderer.color = Model.tankSpriteColor;
            upperPartBodyRenderer.color = Model.tankSpriteColor;
        }
    }

    void Update()
    {
        // Determine whether this instance should accept local input:
        // - If Photon is connected and in a room, require photonView.IsMine
        // - If Photon is not connected or not in room (offline mode), treat as local
        bool isLocalControl = (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) || (photonView != null && photonView.IsMine);

        if (isLocalControl)
        {
            float rotationInput = -moveInput.x;
            view.Move(GetCardinalDirection(moveInput));
            view.Rotate(rotationInput);

            if (firePressed)
            {
                view.Shoot();
                firePressed = false;
            }
        }
        else
        {
            // Remote player: smooth physics-driven interpolation
            // Use Lerp for position & velocity, LerpAngle for rotation
            rb.position = Vector2.Lerp(rb.position, networkPosition, Time.deltaTime * lerpRate);
            rb.velocity = Vector2.Lerp(rb.velocity, networkVelocity, Time.deltaTime * lerpRate);

            float currentZ = transform.rotation.eulerAngles.z;
            float newZ = Mathf.LerpAngle(currentZ, networkRotationZ, Time.deltaTime * lerpRate);
            transform.rotation = Quaternion.Euler(0f, 0f, newZ);
        }
    }

    private Vector2 GetCardinalDirection(Vector2 input)
    {
        if (input == Vector2.zero) return Vector2.zero;

        // Prefer vertical over horizontal if both pressed
        if (Mathf.Abs(input.y) >= Mathf.Abs(input.x))
        {
            return new Vector2(0, Mathf.Sign(input.y));
        }
        else
        {
            return new Vector2(Mathf.Sign(input.x), 0);
        }
    }

    // ---------------- IPunObservable ----------------
    // Serialize Rigidbody2D position, velocity, rotation.z
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Owner: send state
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
            stream.SendNext(transform.rotation.eulerAngles.z);
        }
        else
        {
            // Remote: receive state
            networkPosition = (Vector2)stream.ReceiveNext();
            networkVelocity = (Vector2)stream.ReceiveNext();
            networkRotationZ = (float)stream.ReceiveNext();
        }
    }

}

