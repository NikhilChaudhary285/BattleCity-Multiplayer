public class GameSettingsManager : Singleton<GameSettingsManager>
{
	public GameSettings settings;

	override public void Awake()
	{
		base.Awake(); // Calling Base or Parent "Singleton class virtual Awake() Method"
	}

	public void SetMode(GameMode mode, int playerCount = 1)
	{
		settings.selectedGameMode = mode;
		settings.expectedPlayerCount = playerCount;
	}
}