using UnityEngine;

public class MultiplayerModePresenter : IPresenter
{
	private MultiplayerModeUI view;

	public MultiplayerModePresenter(MultiplayerModeUI view)
	{
		this.view = view;
		Initialize();
	}

	public void Initialize()
	{
		Debug.Log("[MultiplayerModePresenter] Initialized");
	}

	public void Dispose()
	{
		Debug.Log("[MultiplayerModePresenter] Initialized");
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

	public void GoToCreateRoom()
	{
		UIManager.Instance.ShowCreateRoomPanel();
	}

	public void GoToJoinRoom()
	{
		UIManager.Instance.ShowJoinRoomPanel();
	}

	public void GoBack()
	{
		UIManager.Instance.ShowGameModePanel();
	}
}