using UnityEngine;

[CreateAssetMenu(menuName = "Tank/PlayerTankData")]
public class PlayerTankModel : ScriptableObject
{
	public float moveSpeed = 5f;
	public float rotationSpeed = 180f;
	public float fireCooldown = 0.5f;
    public GameObject playerExplosionPrefab;

    [HideInInspector]
	public float lastFiredTime = -999f;

	#region ---- OF ----

	/*public bool CanFire() => Time.time >= lastFiredTime + fireCooldown;
	public void RecordFireTime() => lastFiredTime = Time.time;*/
	#endregion ---- OF ----
}
