using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
	public static PhotonManager Instance { get; private set; }

	[Header("Photon Config")]
	[SerializeField] private PhotonAppConfig config;

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

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogWarning($"❌ Disconnected from Photon: {cause}");
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("🎉 Joined Photon Lobby");
	}

	private void JoinLobby()
	{
		PhotonNetwork.JoinLobby();
	}
}