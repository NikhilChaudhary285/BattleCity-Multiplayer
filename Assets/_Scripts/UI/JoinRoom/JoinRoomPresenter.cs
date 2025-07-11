public class JoinRoomPresenter : IPresenter
{
	private JoinRoomUI view;

	public JoinRoomPresenter(JoinRoomUI view)
	{
		this.view = view;
		Initialize();
	}

	public void Initialize()
	{
		view.SetStatus("Enter Room name to join the match");

	}

	public void Dispose() { }

	public void JoinRoom(string roomName)
	{
		if (string.IsNullOrWhiteSpace(roomName))
		{
			view.SetStatus("Room name cannot be empty.");
			return;
		}

		PhotonManager.Instance.JoinRoom(roomName);
		view.SetStatus($"Joining room: {roomName}...");
	}

	public void GoBack()
	{
		UIManager.Instance.ShowMultiplayerPanel();
	}
}