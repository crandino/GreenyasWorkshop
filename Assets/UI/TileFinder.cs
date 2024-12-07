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

    private static GridLimits Default = new()
    {
        limits = Enumerable.Repeat(HexTools.GetGridCartesianWorldPos(new CubeCoord(0, 0, 0)), 4).ToArray()
    };

    public static GridLimits GetLimits()
    {
        Tile[] tiles = GameObject.FindObjectsByType<Tile>(FindObjectsSortMode.None);

        if(tiles.Length == 0)
            return Default;
        else
        {
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
}
