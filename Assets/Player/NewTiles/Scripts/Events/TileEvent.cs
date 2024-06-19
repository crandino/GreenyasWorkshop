using Greenyas.Hexagon;
using UnityEngine;

namespace Hexalinks.Tile
{
    public abstract class TileEvent : MonoBehaviour
    {
        public virtual void Initialize() { }

        public abstract void OnPickUp(Tile.Data tileData);

        public abstract void OnRelease(Tile.Data tileData);

        //public struct Data
        //{
        //    public CubeCoord Coord;

        //    public Data(Vector3 tilePosition) : this(HexTools.GetNearestCubeCoord(tilePosition))
        //    { }

        //    public Data(CubeCoord tileCubeCoord)
        //    {
        //        Coord = tileCubeCoord;
        //    }
        //}

    } 
}
