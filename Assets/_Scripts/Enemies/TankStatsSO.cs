using UnityEngine;

public enum EnemyTankType
{
    Basic,
    Fast,
    Power,
    Armored
}

[CreateAssetMenu(menuName = "Stats/TankStats")]
public class TankStatsSO : ScriptableObject
{
    public EnemyTankType tankType;

    public float moveSpeed;
    public int maxHealth; // For armored tanks, e.g. 4 hits
    public float fireRate;

    [Header("Visuals")]
    public Sprite tankSprite;
    public Sprite upperTankSprite;
    public Color tankSpriteColor;

    [Header("Visuals Effects")]
    public GameObject enemyExplosionPrefab;

    [Header("Special Abilities")]
    public bool isBurstFire;
    public int burstCount = 1; // e.g., 3 shots in burst
    public float burstDelay = 0.2f; // e.g., 0.2f seconds delay in burst shots 

    [TextArea] public string notes;
}
