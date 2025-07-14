using Photon.Pun;
using UnityEngine;

public class NetworkGameManager : MonoBehaviourPunCallbacks
{
	[Header("Player Prefab (placed in Resources folder)")]
	[SerializeField] private GameObject playerTankPrefab;

	[Header("Spawn Points")]
	[SerializeField] private Transform[] spawnPoints;

	private void Start()
	{
		if (GameSettingsManager.Instance.Mode == GameMode.SinglePlayer)
		{
			SpawnLocalPlayerOffline();
		}
		else if (PhotonNetwork.InRoom)
		{
			SpawnNetworkPlayer();
		}
	}

	private void SpawnLocalPlayerOffline()
	{
		Instantiate(playerTankPrefab, spawnPoints[0].position, Quaternion.identity);
	}

	private void SpawnNetworkPlayer()
	{
		int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;

		if (index < 0 || index >= spawnPoints.Length)
		{
			index = 0;
		}

		PhotonNetwork.Instantiate(playerTankPrefab.name, spawnPoints[index].position, Quaternion.identity);
	}
}