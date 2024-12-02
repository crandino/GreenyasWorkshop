using Greenyas.Hexagon;
using HexaLinks.Tile;
using System.Linq;
using UnityEngine;

public static class TileFinder
{
    public struct GridLimits
    {
        public Vector3[] limits; // Left, right, bottom, up
    }

    public static GridLimits GetLimits()
    {
        Tile[] tiles = GameObject.FindObjectsByType<Tile>(FindObjectsSortMode.None);
        IOrderedEnumerable<Tile> orderedTilesByDistanceToOrigin = tiles.OrderByDescending(c => HexTools.GetGridCartesianWorldPos(c.Coord).magnitude);
        
        return new()
        {
            limits = new[]
            {
                tiles.OrderBy(c => HexTools.GetGridCartesianWorldPos(c.Coord).x).First().transform.position,
                tiles.OrderByDescending(c => HexTools.GetGridCartesianWorldPos(c.Coord).x).First().transform.position,
                orderedTilesByDistanceToOrigin.OrderBy(c => HexTools.GetGridCartesianWorldPos(c.Coord).z).First().transform.position,
                orderedTilesByDistanceToOrigin.OrderByDescending(c => HexTools.GetGridCartesianWorldPos(c.Coord).z).First().transform.position
            }
        };
    }
}
