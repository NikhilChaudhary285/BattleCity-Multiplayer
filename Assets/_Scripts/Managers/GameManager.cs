using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
	public enum GameState { MainMenu, Playing, Paused, GameOver }

	public GameState CurrentStateType { get; private set; }
	private IGameState currentState;

	[Header("References")]
	[Tooltip("Wave Spawner")][SerializeField] private WaveSpawner waveSpawner;
	[Tooltip("Eagle Base Health Manager")] public BaseHealthManager EagleBase;

	private Dictionary<GameState, IGameState> stateMap;

	override public void Awake()
	{
		base.Awake(); // Calling Base or Parent "Singleton class virtual Awake() Method"
		InitStates();
	}

	private void Start()
	{
		SetState(GameState.MainMenu); // Transition to MainMenu state: To Start menu logic or preload (if needed) 
	}

	private void InitStates()
	{
		stateMap = new Dictionary<GameState, IGameState>
		{
			{ GameState.MainMenu, new MainMenuState() },
			{ GameState.Playing, new PlayingState() },
			{ GameState.GameOver, new GameOverState() },
            { GameState.Paused, new PausedState() } 
        };
	}

	public void SetState(GameState newState)
	{
		if (CurrentStateType == newState) return;

		currentState?.ExitState(this); // Call exit on previous state

		CurrentStateType = newState;

		if (stateMap.TryGetValue(newState, out var newStateHandler))
		{
			currentState = newStateHandler;
			currentState.EnterState(this);
		}
		else
		{
			Debug.LogWarning($"No handler for state: {newState}");
		}
	}

	public WaveSpawner WaveSpawner => waveSpawner;

	public void RegisterWaveSpawner(WaveSpawner _waveSpawner)
	{
		waveSpawner = _waveSpawner;
	}

	public void RegisterEagleBase(BaseHealthManager _eagleBase)
	{
		EagleBase = _eagleBase;
	}
}
