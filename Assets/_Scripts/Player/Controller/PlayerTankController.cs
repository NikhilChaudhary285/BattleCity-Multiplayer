using UnityEngine;

public class PlayerTankController : MonoBehaviour
{
	public PlayerTankModel modelData;
	public PlayerTankView view;

	private PlayerInputActions input;
	private Vector2 moveInput;
	private bool firePressed;

	private float lastFiredTime = -999f;

	public PlayerTankModel Model => modelData;

	public bool CanFire() => Time.time >= lastFiredTime + modelData.fireCooldown;
	public void RecordFireTime() => lastFiredTime = Time.time;

	private void Awake()
	{
		input = new PlayerInputActions();
	}

	private void OnEnable()
	{
		input.Enable();

		input.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		input.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

		input.Gameplay.Fire.performed += ctx => firePressed = true;
	}

	private void OnDisable()
	{
		input.Disable();
	}

	void Start()
	{
		view.SetController(this);
	}

	void Update()
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
