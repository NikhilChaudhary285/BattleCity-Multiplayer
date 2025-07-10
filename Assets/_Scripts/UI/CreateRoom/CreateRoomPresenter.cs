public class CreateRoomPresenter : IPresenter
{
	private CreateRoomUI view;

	public CreateRoomPresenter(CreateRoomUI view)
	{
		this.view = view;
		Initialize();
	}

	public void Initialize()
	{
		view.SetStatus("Enter Room name to create the match");
	}

	public void Dispose() { }

	public void CreateRoom(string roomName)
	{
		if (string.IsNullOrWhiteSpace(roomName))
		{
			view.SetStatus("Room name cannot be empty.");
			return;
		}

		int playerCount = GameSettingsManager.Instance.PlayerCount;

		PhotonManager.Instance.CreateRoom(roomName, playerCount);
		view.SetStatus($"Creating room: {roomName} for {playerCount} players...");
	}

	public void GoBack()
	{
		UIManager.Instance.ShowMultiplayerModeUI();
	}
}