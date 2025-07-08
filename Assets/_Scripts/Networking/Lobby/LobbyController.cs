using Photon.Pun;
using Photon.Realtime;
using System.Text;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks, IPresenter
{
	[SerializeField] private LobbyUI lobbyUI;

	public override void OnEnable()
	{
		base.OnEnable();
		Initialize();
	}

	public void Initialize()
	{
		if (lobbyUI == null) return;

		lobbyUI.Initialize();
		lobbyUI.OnJoinRoomPressed += HandleJoinRoom;
		lobbyUI.OnStartGamePressed += HandleStartGame;

		lobbyUI.SetStatus("Connecting to Photon...");
	}

	public void Dispose()
	{
		if (lobbyUI == null) return;

		lobbyUI.OnJoinRoomPressed -= HandleJoinRoom;
		lobbyUI.OnStartGamePressed -= HandleStartGame;
	}

	private void HandleJoinRoom(string roomName)
	{
		if (string.IsNullOrWhiteSpace(roomName))
			roomName = "Room_" + Random.Range(1000, 9999);

		int selectedCount = 4; // You can fetch this from a dropdown or toggle in UI later

		GameSettingsManager.Instance.SetMode(GameMode.Multiplayer4P, selectedCount);
		GameSettingsManager.Instance.settings.roomName = roomName;

		PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions
		{
			MaxPlayers = (byte)selectedCount
		}, TypedLobby.Default);

		lobbyUI.SetStatus("Joining Room: " + roomName);
	}

	private void HandleStartGame()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel(levelName: "Gameplay");
		}
	}

	public override void OnJoinedRoom()
	{
		lobbyUI.SetStatus("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
		UpdatePlayerList();
		lobbyUI.SetStartButtonVisible(PhotonNetwork.IsMasterClient);
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		UpdatePlayerList();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		UpdatePlayerList();
	}

	private void UpdatePlayerList()
	{
		StringBuilder sb = new StringBuilder();

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			sb.AppendLine(player.NickName);
		}

		lobbyUI.SetPlayerList(sb.ToString());
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		lobbyUI.SetStatus("Disconnected: " + cause);
	}
	public override void OnDisable()
	{
		base.OnDisable();
		Dispose();
	}
}