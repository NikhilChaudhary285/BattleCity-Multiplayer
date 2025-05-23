using UnityEngine;

[CreateAssetMenu(menuName = "Stats/TankStats")]
public class TankStatsSO : ScriptableObject
{
	public float moveSpeed;
	public int maxHealth;
	public float fireRate;
}
