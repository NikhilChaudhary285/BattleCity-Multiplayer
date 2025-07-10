public class GameSettingsManager : Singleton<GameSettingsManager>
{
	public GameSettings settings;

	public GameMode Mode => settings.selectedGameMode;
	public string RoomName => settings.roomName;
	public int PlayerCount => settings.expectedPlayerCount;

	override public void Awake()
	{
		base.Awake();
	}

	public void SetMode(GameMode mode, int playerCount = 1)
	{
		settings.selectedGameMode = mode;
		settings.expectedPlayerCount = playerCount;
	}

	public void SetRoomName(string name)
	{
		settings.roomName = name;
	}
}