using UnityEngine;

public class TileDestructible : MonoBehaviour
{
    public bool isDestructible;

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
