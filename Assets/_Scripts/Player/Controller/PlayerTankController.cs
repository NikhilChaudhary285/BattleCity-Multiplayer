using UnityEngine;
using Photon.Pun;

public class PlayerTankController : MonoBehaviourPun
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

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        if (photonView != null && !photonView.IsMine) return; // Only enable input for local player

        input.Enable();

        input.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        input.Gameplay.Fire.performed += ctx => firePressed = true;
    }

    private void OnDisable()
    {
        if (photonView != null && !photonView.IsMine) return; // Prevent disabling input for remote players

        input.Disable();
    }

    void Start()
    {
        Init();
    }

    // Call this also on condition if wherever you spawn or initialize the player prefab
    public void Init()
    {
        if (view != null)
            view.SetController(this);

        if (health != null)
            health.Init(this, Model.maxHealth);

        if (bodyRenderer != null && Model.tankSprite != null)
        {
            bodyRenderer.sprite = Model.tankSprite;
            upperPartBodyRenderer.sprite = Model.upperTankSprite;
        }

        if (bodyRenderer != null && Model.tankSpriteColor != null)
        {
            bodyRenderer.color = Model.tankSpriteColor;
            upperPartBodyRenderer.color = Model.tankSpriteColor;
        }
    }

    void Update()
    {
        if (photonView != null && !photonView.IsMine) return; // Skip input & control for remote players

        float rotationInput = -moveInput.x;
        view.Move(GetCardinalDirection(moveInput));
        view.Rotate(rotationInput);

        if (firePressed)
        {
            view.Shoot();
            firePressed = false;
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
}
