using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        private TileManipulator manipulator;
        [SerializeField]
        private TileConnectivity connectivity;

        public CubeCoord Coord => manipulator.Coord;
        //public Gate[] Gates => connectivity.Gates;

        public enum Type
        {
            StarterSimple,
            StarterDouble,
            StarterTriple,

            FlowStraight,
            FlowSimpleShortCurve,
            FlowSimpleLongCurve,
            FlowDoubleNearShortCurve,
            FlowDoubleFarShortCurve,
            FlowDoubleLongCurve,
            FlowTripleShortCurve,

            SplitLongCurve
        }

        private void Start()
        {
            manipulator.Initialize(this);
            //connectivity.Initialize();
        }

        public void PickUp()
        {
            manipulator.PickUp();
            connectivity.Disconnection();

            HexMap.Instance.RemoveTile(Coord);
        }

        public void Release()
        {
            manipulator.Release();
            Connect();

            HexMap.Instance.AddTile(Coord, this);
            TileIterator.LookForClosedPaths();
        }

        public void Connect()
        {

            // Uso TileConnectivity para buscar candidatos. Pasar el punto de partida CubeCoord de esta Tile.
            // Devuelve lista de candidatos
            List<Tile> candidates = connectivity.AreConnectedTiles(Coord);

            // Comprobamos que las conectividades son satisfactorias entre ambas Tiles
            // Enlazamos las Tiles

            for (int i = 0; i < candidates.Count; ++i)
            {
                connectivity.TryConnection(candidates[i].connectivity);


                //Gate gateFrom = Gates[i];
                //CubeCoord neighborHexCoord = tile.Coord + CubeCoord.GetToNeighborCoord(gateFrom.EntryNode.Side);

                //if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile neighborTileData))
                //    gateFrom.Connect(neighborTileData.Gates);
            }
        }
    }
}
