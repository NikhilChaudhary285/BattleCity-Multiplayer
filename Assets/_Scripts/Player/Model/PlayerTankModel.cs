using UnityEngine;

[CreateAssetMenu(menuName = "Tank/PlayerTankData")]
public class PlayerTankModel : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    [Header("Health")]
    public int maxHealth = 1; // For players that are having armored tanks, e.g. 4 hits

    [Header("Firing")]
    public float fireCooldown = 0.5f;
    public int burstCount = 1; // optional: player burst fire support
    public bool isBurstFire = false;

    [HideInInspector]
    public float lastFiredTime = -999f;

    [Header("Visuals")]
    public Sprite tankSprite;
    public Sprite upperTankSprite;
    public Color tankSpriteColor = Color.white;

    [Header("Visual Effects")]
    public GameObject playerExplosionPrefab;

    [Header("Other Settings")]
    [TextArea]
    public string notes;
}
