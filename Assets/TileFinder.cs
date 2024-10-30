using HexaLinks.Tile;
using System.Linq;
using UnityEngine;

public static class TileFinder
{
    public struct GridLimits
    {
        public Transform north;
        public Transform south;
        public Transform west;
        public Transform east;
    }

    public static GridLimits GetLimits()
    {
        Tile[] tiles = GameObject.FindObjectsByType<Tile>(FindObjectsSortMode.None);
        return new()
        {
            north = tiles.OrderBy(t => t.Coord.R).ThenByDescending(c => c.Coord.S).First().transform,
            west = tiles.OrderBy(t => t.Coord.Q).First().transform,
            south = tiles.OrderByDescending(t => t.Coord.R).ThenBy(c => c.Coord.S).First().transform,
            east = tiles.OrderByDescending(t => t.Coord.Q).First().transform
        };
    }
}
