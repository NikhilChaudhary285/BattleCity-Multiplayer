using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ShareRoomLobbyPresenter : IPresenter
{
	private ShareRoomLobbyUI view;

	public ShareRoomLobbyPresenter(ShareRoomLobbyUI view)
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

		PhotonManager.Instance.LoadGameplayForAll(); // load gameplay scene for all
	}

	public void LeaveRoom()
	{
		PhotonManager.Instance.LeaveRoom();
	}

	private void UpdatePlayerList()
	{
		UpdatePlayerListData();
		UpdatePlayerListStatus();
	}

	private void UpdatePlayerListData()
	{
		view.ToggleJoinedComradesContainerByPlayerCount(PhotonNetwork.PlayerList.Length);

		List<JoinedRoomFriendData> joinedRoomFriendDataList = new();

		foreach (var player in PhotonNetwork.PlayerList)
		{
			string name = string.IsNullOrWhiteSpace(player.NickName)
				? $"User_{Random.Range(1000, 9999)}"
				: player.NickName;

			Sprite profileSprite = (view.UserProfileSprites != null && view.UserProfileSprites.Length > 0)
				? view.UserProfileSprites[Random.Range(0, view.UserProfileSprites.Length)]
				: null;

			joinedRoomFriendDataList.Add(new JoinedRoomFriendData
			{
				userName = name,
				wins = 0,
				userProfile = profileSprite
			});
		}

		view.SetPlayerList(joinedRoomFriendDataList);

	}

	private void UpdatePlayerListStatus()
	{
		int currentPlayers = PhotonNetwork.PlayerList.Length;
		int expectedPlayers = GameSettingsManager.Instance.PlayerCount;

		string status;
		if (currentPlayers >= expectedPlayers)
		{
			status = "All Comrades Joined";
		}
		else if (PhotonNetwork.IsMasterClient)
		{
			status = "Start when ready";
		}
		else
		{
			status = "Waiting for Players...";
		}

		view.SetStatus(status);
	}

}
