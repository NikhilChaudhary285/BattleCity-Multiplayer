public class JoinRoomPresenter : IPresenter
{
	private JoinRoomUI view;

	public JoinRoomPresenter(JoinRoomUI view)
	{
		this.view = view;
		Initialize();
		PhotonManager.Instance.OnJoinRoomFailedCallback += OnJoinRoomFailed;
	}

	public void Initialize()
	{
		view.SetStatus("Enter Room name to join the match");
	}

	public void Dispose() 
	{
		PhotonManager.Instance.OnJoinRoomFailedCallback -= OnJoinRoomFailed;
	}

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

	public void OnJoinRoomFailed(string errorMessage)
	{
		view.SetStatus($"Join failed: {errorMessage}");
	}

	public void GoBack()
	{
		UIManager.Instance.ShowMultiplayerPanel();
	}
}