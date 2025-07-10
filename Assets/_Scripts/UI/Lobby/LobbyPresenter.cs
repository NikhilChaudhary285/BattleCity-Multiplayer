using Photon.Pun;
using System.Text;
using UnityEngine;

public class LobbyPresenter : IPresenter
{
	private LobbyUI view;

	public LobbyPresenter(LobbyUI view)
	{
		this.view = view;
		Initialize();

		PhotonManager.OnPlayerListUpdated += UpdatePlayerList;
	}

	public void Initialize()
	{
		string roomName = PhotonNetwork.CurrentRoom?.Name ?? "Unknown";
		view.SetRoomName(roomName);
		view.SetStartButtonVisible(PhotonNetwork.IsMasterClient);
		UpdatePlayerList();
	}

	public void Dispose()
	{
		PhotonManager.OnPlayerListUpdated -= UpdatePlayerList;
	}

	public void StartGame()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogWarning("[LobbyPresenter] Only host can start the game.");
			return;
		}

		PhotonManager.Instance.LoadGameplayForAll();
	}

	public void LeaveRoom()
	{
		PhotonManager.Instance.LeaveRoom();
	}

	private void UpdatePlayerList()
	{
		StringBuilder sb = new StringBuilder();
		foreach (var player in PhotonNetwork.PlayerList)
		{
			sb.AppendLine(player.NickName);
		}

		view.SetPlayerList(sb.ToString());

		string status = PhotonNetwork.IsMasterClient
			? "Start when ready"
			: "Waiting for Players...";
		view.SetStatus(status);
	}
}