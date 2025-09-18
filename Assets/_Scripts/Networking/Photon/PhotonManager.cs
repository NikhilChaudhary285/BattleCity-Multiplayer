using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }

    public void SetPlayerReady(bool ready)
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { { "isReady", ready } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    [Header("Photon Config")]
    [SerializeField] private PhotonAppConfig config;

    public static event Action OnPlayerListUpdated;
    public Action<string> OnJoinRoomFailedCallback;
    public Action<string> OnCreateRoomFailedCallback;

    public bool IsConnectedToMaster => PhotonNetwork.IsConnectedAndReady;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        ConnectToPhotonServer();
    }

    public void ConnectToPhotonServer()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = config.gameVersion;

        PhotonNetwork.ConnectUsingSettings();

        Debug.Log($"Connecting to Photon... [v{config.gameVersion}]");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✅ Connected to Photon Master Server");
        if (string.IsNullOrWhiteSpace(PhotonNetwork.NickName))
        {
            PhotonNetwork.NickName = $"Tank_{UnityEngine.Random.Range(1, 999)}";
        }

        JoinLobby();
    }

    private void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom(string roomName, int maxPlayers)
    {
        RoomOptions options = new RoomOptions { MaxPlayers = (byte)maxPlayers };
        PhotonNetwork.CreateRoom(roomName, options);
        GameSettingsManager.Instance.SetRoomName(roomName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        GameSettingsManager.Instance.SetRoomName(roomName);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        UIManager.Instance.ShowMultiplayerPanel();
    }

    public IEnumerator ReloadSceneAfterDelay(float delay, Scene scene)
    {
        yield return new WaitForSecondsRealtime(delay); // Realtime ignores Time.timeScale = 0

        Time.timeScale = 1f; // Reset in case it was paused/frozen

        switch (scene)
        {
            case Scene.MainMenu:
                PhotonNetwork.LoadLevel(Scene.MainMenu.ToString());
                break;

            case Scene.Gameplay:
                PhotonNetwork.LoadLevel(Scene.Gameplay.ToString());
                break;

            default:
                Debug.LogWarning("Unhandled scene type: " + scene);
                break;
        }
    }

    public void LoadGameplayForAll()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(Scene.Gameplay.ToString()); // Gameplay scene
        }
    }

    public void LoadGameplay()
    {
        PhotonNetwork.LoadLevel(Scene.Gameplay.ToString()); // Gameplay scene
    }

    public void LoadMainMenu()
    {
        PhotonNetwork.LoadLevel(Scene.MainMenu.ToString()); // MainMenu scene
    }

    // ─────────────── CALLBACKS ───────────────

    public override void OnJoinedRoom()
    {
        Debug.Log("✅ Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        LogPhotonInfo();
        UIManager.Instance.ShowShareRoomPanel();
        OnPlayerListUpdated?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("👤 Player Joined: " + newPlayer.NickName);
        LogPhotonInfo();
        OnPlayerListUpdated?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("❌ Player Left: " + otherPlayer.NickName);
        OnPlayerListUpdated?.Invoke();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message);
        OnCreateRoomFailedCallback?.Invoke(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Join room failed: " + message);
        OnJoinRoomFailedCallback?.Invoke(message);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        OnPlayerListUpdated?.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"❌ Disconnected from Photon: {cause}");
    }

    public void LogPhotonInfo()
    {
        string roomName = PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.Name : "NoRoom";
        int playersCount = PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.PlayerCount : 0;
        Debug.Log($"[PhotonManager.LogPhotonInfo] Room={roomName}, LocalActor={(PhotonNetwork.LocalPlayer != null ? PhotonNetwork.LocalPlayer.ActorNumber.ToString() : "NoLocalPlayer")}, Players={playersCount}");

        var players = PhotonNetwork.PlayerList;
        if (players != null)
        {
            foreach (var p in players)
            {
                Debug.Log($"[PhotonManager] Player actor {p.ActorNumber}, name:{p.NickName}");
            }
        }

        var pviews = FindObjectsOfType<PhotonView>();
        foreach (var pv in pviews)
        {
            var owner = pv.Owner != null ? pv.Owner.ActorNumber.ToString() : "null";
            Debug.Log($"[PhotonView] id:{pv.ViewID} owner:{owner} isMine:{pv.IsMine} go:{pv.gameObject.name}");
        }
    }
}