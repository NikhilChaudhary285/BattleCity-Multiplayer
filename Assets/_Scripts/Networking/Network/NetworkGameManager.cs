using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkGameManager : MonoBehaviourPunCallbacks
{
    [Header("Player Prefab (placed in Resources folder)")]
    [SerializeField] private GameObject playerTankPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private bool hasSpawned = false;

    private void Start()
    {
        // If we are already in room and in gameplay scene, try to spawn
        TrySpawnIfReady();
    }

    public override void OnJoinedRoom()
    {
        // Called when this client joins a room
        TrySpawnIfReady();
    }

    public override void OnLeftRoom()
    {
        hasSpawned = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"[NetworkGameManager] Player entered: {newPlayer.NickName} (actor {newPlayer.ActorNumber})");
    }

    public override void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        // When scene changes, attempt spawn (useful when master triggers PhotonNetwork.LoadLevel)
        TrySpawnIfReady();
    }

    public override void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void TrySpawnIfReady()
    {
        Debug.Log("[TrySpawnIfReady] called. hasSpawned=" + hasSpawned);

        if (hasSpawned)
        {
            Debug.Log("[TrySpawnIfReady] skipped: hasSpawned == true");
            return;
        }

        // If offline single player mode — allow local spawn without Photon checks
        bool isOfflineSinglePlayer = (GameSettingsManager.Instance != null &&
                                      GameSettingsManager.Instance.Mode == GameMode.SinglePlayer);

        Debug.Log($"[TrySpawnIfReady] isOfflineSinglePlayer={isOfflineSinglePlayer}");

        if (!isOfflineSinglePlayer)
        {
            Debug.Log($"[TrySpawnIfReady] PhotonNetwork.InRoom={PhotonNetwork.InRoom}, IsConnectedAndReady={PhotonNetwork.IsConnectedAndReady}");

            if (!PhotonNetwork.InRoom || !PhotonNetwork.IsConnectedAndReady)
            {
                Debug.LogWarning("[TrySpawnIfReady] Not in room or not connected yet — spawn deferred.");
                return;
            }
        }

        var currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("[TrySpawnIfReady] CurrentScene=" + currentScene);

        if (currentScene != Scene.Gameplay.ToString())
        {
            Debug.LogWarning($"[TrySpawnIfReady] Current scene '{currentScene}' != expected '{Scene.Gameplay.ToString()}' — spawn deferred.");
            return;
        }

        if (playerTankPrefab == null)
        {
            Debug.LogError("[TrySpawnIfReady] playerTankPrefab is null. Ensure prefab exists in Resources and is assigned in inspector.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[TrySpawnIfReady] spawnPoints is empty or null. Assign spawn points in inspector.");
            return;
        }

        // Verify prefab exists in Resources by trying to load by name (helps detect naming/path issues)
        var resourceCheck = Resources.Load(playerTankPrefab.name);
        Debug.Log("[TrySpawnIfReady] Resources.Load('" + playerTankPrefab.name + "') returned: " + (resourceCheck ? resourceCheck.name : "null"));

        // Offline spawn path (local only)
        if (isOfflineSinglePlayer)
        {
            int index = 0; // single player always spawn at first spawnPoint
            Vector3 spawnPos = spawnPoints[Mathf.Clamp(index, 0, spawnPoints.Length - 1)].position;

            Debug.Log($"[TrySpawnIfReady] Spawning LOCAL OFFLINE player at spawn index {index} / pos {spawnPos}");
            SpawnLocalPlayerOffline(spawnPos);
            hasSpawned = true;
            return;
        }

        // Online spawn (Photon)
        int actorIndex = Mathf.Max(0, PhotonNetwork.LocalPlayer.ActorNumber - 1);
        int indexOnline = actorIndex % Mathf.Max(1, spawnPoints.Length);
        Vector3 spawnPosOnline = spawnPoints[indexOnline].position;

        Debug.Log($"[TrySpawnIfReady] Instantiating '{playerTankPrefab.name}' for actor {PhotonNetwork.LocalPlayer.ActorNumber} at spawn index {indexOnline} / pos {spawnPosOnline}");
        GameObject go = PhotonNetwork.Instantiate(playerTankPrefab.name, spawnPosOnline, Quaternion.identity);
        if (go == null)
        {
            Debug.LogError("[TrySpawnIfReady] PhotonNetwork.Instantiate returned null. Check prefab in Resources and Photon logs.");
            return;
        }

        hasSpawned = true;
        Debug.Log("[TrySpawnIfReady] Spawn successful. Spawned GO name: " + go.name);

        // optional: existing debug method on PhotonManager (if available)
        var pm = FindObjectOfType<PhotonManager>();
        if (pm != null) pm.LogPhotonInfo();
    }

    /// <summary>
    /// Spawns a non-networked local player (used for offline mode).
    /// This uses regular Instantiate (not Photon) so the object is local-only.
    /// If your Player prefab has networking components, make sure it will function in local mode as well.
    /// </summary>
    private void SpawnLocalPlayerOffline(Vector3 spawnPos)
    {
        GameObject localPlayerGo = Instantiate(playerTankPrefab, spawnPos, Quaternion.identity);
        if (localPlayerGo == null)
        {
            Debug.LogError("[SpawnLocalPlayerOffline] Failed to instantiate local player prefab.");
            return;
        }

        // If your PlayerTankController has Init() that must be called
        var controller = localPlayerGo.GetComponent<PlayerTankController>();
        if (controller != null) controller.Init();

        Debug.Log("[SpawnLocalPlayerOffline] Local player spawned at " + spawnPos);
    }
}
