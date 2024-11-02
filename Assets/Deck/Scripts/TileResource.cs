using HexaLinks.Tile;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TileResource"), Serializable]
public class TileResource : ScriptableObject
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private Tile tilePrefab;

    public Sprite Icon => icon;
    public Tile Prefab => tilePrefab;

    public static TileResource Create(Sprite icon, Tile prefab)
    {
        TileResource sourceInstance = TileResource.CreateInstance<TileResource>();
        sourceInstance.icon = icon;
        sourceInstance.tilePrefab = prefab;
        return sourceInstance;
    }
}



