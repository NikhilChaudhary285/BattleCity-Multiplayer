using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
	public static PhotonManager Instance { get; private set; }

	[Header("Photon Config")]
	[SerializeField] private PhotonAppConfig config;

	public static event Action OnPlayerListUpdated;

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
	}

	public void LoadGameplayForAll()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			SceneLoader.Instance.LoadGame(); // Gameplay scene
		}
	}

	// ─────────────── CALLBACKS ───────────────

	public override void OnJoinedRoom()
	{
		Debug.Log("✅ Joined Room: " + PhotonNetwork.CurrentRoom.Name);
		UIManager.Instance.ShowLobbyUI();
		OnPlayerListUpdated?.Invoke();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log("👤 Player Joined: " + newPlayer.NickName);
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
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.LogError("Join room failed: " + message);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogWarning($"❌ Disconnected from Photon: {cause}");
	}
}