using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ShareRoomLobbyPresenter : IPresenter
{
    private ShareRoomLobbyUI view;
    private bool isReady = false; // local ready state

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

        // Ready button: On Visibility
        view.SetReadyButtonVisible(true);

        UpdatePlayerList();
    }

    public void Dispose()
    {
        PhotonManager.OnPlayerListUpdated -= UpdatePlayerList;
    }

    // ─────────────── LOBBY ACTIONS ───────────────

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("[LobbyPresenter] Only host can start the game.");
            return;
        }

        // Ensure all players are ready before starting
        if (!AreAllPlayersReady())
        {
            Debug.LogWarning("[LobbyPresenter] Cannot start game until all players are ready.");
            return;
        }

        PhotonManager.Instance.LoadGameplayForAll(); // load gameplay scene for all
    }

    public void LeaveRoom()
    {
        PhotonManager.Instance.LeaveRoom();
    }

    public void ToggleReady()
    {
        isReady = !isReady;
        PhotonManager.Instance.SetPlayerReady(isReady);
        Debug.Log($"[LobbyPresenter] Local Ready state set to {isReady}");
        view.SetReadyButtonVisible(!isReady); // Ready button: Off Visibility (disable it: if player ready to enter in game)
        view.SetStartButtonVisible(PhotonNetwork.IsMasterClient); // Start button: visible only for host (when all players are ready)
        UpdatePlayerList();
    }

    // ─────────────── UI UPDATES ───────────────

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

            // Check ready state from custom properties
            bool ready = false;
            if (player.CustomProperties.TryGetValue("isReady", out object readyObj))
            {
                ready = (bool)readyObj;
            }

            Sprite profileSprite = (view.UserProfileSprites != null && view.UserProfileSprites.Length > 0)
                ? view.UserProfileSprites[Random.Range(0, view.UserProfileSprites.Length)]
                : null;

            // Add ✅ / ❌ next to player name
            joinedRoomFriendDataList.Add(new JoinedRoomFriendData
            {
                userName = name + (ready ? " yes" : " no"),
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

        bool allReady = AreAllPlayersReady();

        string status;
        if (currentPlayers <= expectedPlayers)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                status = allReady ? "All Players Ready — Start!" : "Waiting for players to Ready...";
                view.SetStartButtonVisible(allReady); // Host sees Start only if all ready
            }
            else
            {
                status = isReady ? "You are Ready :)" : "Click Ready when ready!";
                view.SetStartButtonVisible(false); // Non-host never sees Start button
            }
        }
        else
        {
            status = "Waiting for Players...";
            if (PhotonNetwork.IsMasterClient)
                view.SetStartButtonVisible(false);
        }

        view.SetStatus(status);
    }

    private bool AreAllPlayersReady()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("isReady") || !(bool)player.CustomProperties["isReady"])
            {
                return false;
            }
        }
        return true;
    }

}
