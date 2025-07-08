using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
	public GameMode selectedGameMode = GameMode.SinglePlayer;

	[Header("Photon")]
	public string roomName;
	public int expectedPlayerCount = 1;
}