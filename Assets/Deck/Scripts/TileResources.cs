using Hexalinks.Tile;
using UnityEngine;

[CreateAssetMenu(fileName = "TileResources")]
public class TileResources : ScriptableObject
{
    [SerializeField]
    private SerializableDictionary<Tile.Type, TileResourceLocator> resources = new SerializableDictionary<Tile.Type, TileResourceLocator>();

    [System.Serializable]
    public struct TileResourceLocator
    {
        public Tile tile;
        public Sprite sprite;
    }

    [ContextMenu("Do!")]
    private void FulfillResources()
    {
        resources.Clear();

        //resources.Add(Tile.Type.FlowStraight, new());
        //resources.Add(Tile.Type.SplitLongCurve, new());
    }
}
