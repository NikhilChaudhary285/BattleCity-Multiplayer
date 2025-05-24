using UnityEngine;

public class TileDestructible : MonoBehaviour
{
    public bool isDestructible = true;

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
