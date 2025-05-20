using UnityEngine;

[CreateAssetMenu(menuName = "Tank/PlayerTankData")]
public class PlayerTankModel : ScriptableObject
{
	public float moveSpeed = 5f;
	public float rotationSpeed = 180f;
	public float fireCooldown = 0.5f;

	[HideInInspector]
	public float lastFiredTime = -999f;

	public bool CanFire() => Time.time >= lastFiredTime + fireCooldown;
	public void RecordFireTime() => lastFiredTime = Time.time;
}
