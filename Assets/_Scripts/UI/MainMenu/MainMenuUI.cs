using UnityEngine;

public class MainMenuUI : MonoBehaviour, IMainMenuUI
{
	[Header("UI Panels")]
	[SerializeField] private GameModeUI gameModeUI;
	[SerializeField] private MultiplayerModeUI multiplayerModeUI;
	[SerializeField] private CreateRoomUI createRoomUI;
	[SerializeField] private JoinRoomUI joinRoomUI;
	[SerializeField] private ShareRoomLobbyUI shareRoomLobbyUI;

	private void Awake()
	{
		UIManager.Instance?.RegisterMainMenuUI(this);
		UIManager.Instance.ShowGameModePanel();
	}

	public void ShowGameModeUI() // CHOOSE GAME MODE 
	{
		HideAllPanels();
		DisposeAll();
		gameModeUI.Initialize();
		gameModeUI.Show();
	}

	public void ShowCreateRoomUI() // CREATE ROOM 
	{
		HideAllPanels();
		DisposeAll();
		createRoomUI.Initialize();
		createRoomUI.Show();
	}

	public void ShowJoinRoomUI() // JOIN ROOM 
	{
		HideAllPanels();
		DisposeAll();
		joinRoomUI.Initialize();
		joinRoomUI.Show();
	}

	public void ShowMultiplayerModeUI() // CREATE AND JOIN ROOM
	{
		HideAllPanels();
		DisposeAll();
		multiplayerModeUI.Initialize();
		multiplayerModeUI.Show();
	}

	public void ShowShareRoomLobbyUI() // SHOW PLAYER LIST
	{
		HideAllPanels();
		DisposeAll();
		shareRoomLobbyUI.Initialize();
		shareRoomLobbyUI.Show();
	}

	public void HideAllPanels() // HIDE ALL PANELS 
	{
		gameModeUI?.Hide();
		multiplayerModeUI?.Hide();
		createRoomUI?.Hide();
		joinRoomUI?.Hide();
		shareRoomLobbyUI?.Hide();
	}

	public void DisposeAll()
	{
		gameModeUI?.Dispose();
		multiplayerModeUI?.Dispose();
		createRoomUI?.Dispose();
		joinRoomUI?.Dispose();
		shareRoomLobbyUI?.Dispose();
	}
}