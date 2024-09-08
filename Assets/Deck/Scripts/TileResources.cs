using Hexalinks.Tile;
using UnityEngine;

[CreateAssetMenu(fileName = "TileResources")]
public class TileResources : ScriptableObject
{
    [SerializeField]
    private Sprite emptyIcon;

    [SerializeField]
    private SerializableDictionary<Tile.Type, TileResourceLocator> tileTypeResources = new SerializableDictionary<Tile.Type, TileResourceLocator>();

    [System.Serializable]
    public struct TileResourceLocator
    {
        public Tile tile;
        public Sprite sprite;
    }

    public TileResourceLocator this[Tile.Type tileType]
    {
        get => tileTypeResources[tileType];
    }

    public Sprite EmptyIcon => emptyIcon;
}
