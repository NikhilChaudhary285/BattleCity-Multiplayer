using UnityEngine;

public class CreateRoomPresenter : IPresenter
{
	private CreateRoomUI view;

	public CreateRoomPresenter(CreateRoomUI view)
	{
		this.view = view;
		Initialize();
		PhotonManager.Instance.OnCreateRoomFailedCallback += OnCreateRoomFailed;
	}

	public void Initialize()
	{
		view.SetStatus("Enter Room name to create the match");
	}

	public void Dispose()
	{
		PhotonManager.Instance.OnCreateRoomFailedCallback -= OnCreateRoomFailed;
	}

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

	#region ---- SelectPlayerCount Functionality ----

	public void SelectPlayerCount(int count)
	{
		Debug.Log($"[MultiplayerModePresenter] Selected {count}-Player Room");
		GameMode gameMode = GetGameModeForPlayerCount(count);
		GameSettingsManager.Instance.SetMode(gameMode, count);
		UIManager.Instance.ShowCreateRoomPanel(); // or join room logic
	}
	private GameMode GetGameModeForPlayerCount(int count)
	{
		GameMode mode = GameSettingsManager.Instance.settings.selectedGameMode;
		switch (count)
		{
			case 1:
				mode = GameMode.SinglePlayer;
				break;
			case 2:
				mode = GameMode.Multiplayer2P;
				break;
			case 3:
				mode = GameMode.Multiplayer3P;
				break;
			case 4:
				mode = GameMode.Multiplayer4P;
				break;
			default:
				mode = GameMode.SinglePlayer;
				break;
		}
		return mode;
	}
	#endregion ---- SelectPlayerCount Functionality ----

	public void OnCreateRoomFailed(string errorMessage)
	{
		view.SetStatus($"Room creation failed: {errorMessage}");
	}

	public void GoBack()
	{
		UIManager.Instance.ShowMultiplayerPanel();
	}
}