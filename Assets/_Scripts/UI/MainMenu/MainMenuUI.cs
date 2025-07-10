using UnityEngine;

public class MainMenuUI : MonoBehaviour, IMainMenuUI
{
	[Header("UI Panels")]
	[SerializeField] private GameModeUI gameModeUI;
	[SerializeField] private MultiplayerModeUI multiplayerModeUI;
	[SerializeField] private CreateRoomUI createRoomUI;
	[SerializeField] private JoinRoomUI joinRoomUI;
	[SerializeField] private LobbyUI lobbyUI;

	private void Awake()
	{
		UIManager.Instance?.RegisterMainMenuUI(this);
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

	public void ShowMultiplayerModeUI() // CREATE ROOM WITH PLAYER COUNT
	{
		HideAllPanels();
		DisposeAll();
		multiplayerModeUI.Initialize();
		multiplayerModeUI.Show();
	}

	public void ShowLobbyUI() // SHOW PLAYER LIST
	{
		HideAllPanels();
		DisposeAll();
		lobbyUI.Initialize();
		lobbyUI.Show();
	}

	public void HideAllPanels() // HIDE ALL PANELS 
	{
		gameModeUI?.Hide();
		multiplayerModeUI?.Hide();
		createRoomUI?.Hide();
		joinRoomUI?.Hide();
		lobbyUI?.Hide();
	}

	public void DisposeAll()
	{
		gameModeUI?.Dispose();
		multiplayerModeUI?.Dispose();
		createRoomUI?.Dispose();
		joinRoomUI?.Dispose();
		lobbyUI?.Dispose();
	}
}