using UnityEngine;

public class BaseHealthManager : MonoBehaviour
{
	public int maxHealth = 100;
	private int currentHealth;

	private void Awake()
	{
		currentHealth = maxHealth;
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			Debug.Log("Eagle Base Destroyed!");
			GameManager.Instance.SetState(GameManager.GameState.GameOver);
		}
	}
}
