using UnityEditor;

[CustomEditor(typeof(StarterTile))]
public class StarterTileEditor : TileEditor
{
    protected override int NodesPerPath => 1;
}
