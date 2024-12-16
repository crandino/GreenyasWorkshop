using HexaLinks.Tile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if DEBUG
public static class TileEditorExtensions
{
    public static void AddMeaningfulName(this Tile tile)
    {
        tile.name = $"{tile.name.Remove(tile.name.IndexOf("(Clone)"))}_ID{tile.Hash}";
    }
} 
#endif
