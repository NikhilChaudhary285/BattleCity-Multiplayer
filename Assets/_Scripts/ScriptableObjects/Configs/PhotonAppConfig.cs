using UnityEngine;

[CreateAssetMenu(menuName = "Config/Photon App Config")]
public class PhotonAppConfig : ScriptableObject
{
	public string gameVersion = "1.0.0";
	public string region = "auto"; // or "asia", "us", etc.

	[Tooltip("Optional - default room to auto-join")]
	public string defaultRoom = "BattleCityRoom";
	public byte maxPlayersPerRoom = 4;
}