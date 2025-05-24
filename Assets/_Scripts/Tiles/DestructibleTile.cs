using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "DestructibleTile", menuName = "Tiles/Destructible Tile")]
public class DestructibleTile : Tile
{
    public bool isDestructible = true;

    // You can also override other methods here to customize visuals
}
