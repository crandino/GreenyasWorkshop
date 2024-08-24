using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class TileConnectivity : MonoBehaviour
    {
        [SerializeField]
        private TileSegment[] segments = null;

        //public Gate[] Gates
        //{
        //    get
        //    {
        //        List<Node.Gate> gates = new List<Gate>();

        //        for (int i = 0; i < segments.Length; i++)
        //            gates.AddRange(segments[i].Gates);

        //        return gates.ToArray();
        //    }
        //}

        //public void Initialize()
        //{
        //    for (int i = 0; i < segments.Length; ++i)
        //        segments[i].Initialize();
        //}

        public List<Tile> AreConnectedTiles(CubeCoord coord)
        {
            List<Tile> candidates = new List<Tile>();

            foreach (var s in segments)
                candidates.AddRange(s.GetCandidates(coord));

            return candidates;
        }

        public void TryConnection(TileConnectivity toConnectivity)
        {
            for (int i = 0; i < segments.Length; ++i)
            {
                segments[i].TryConnection(toConnectivity.segments);
            }
        }

        public void Disconnection()
        {
            for (int i = 0; i < segments.Length; ++i)
            {
                segments[i].Disconnect();
            }
        }

        //public bool IsTileAvailableForConnection(CubeCoord coordOrigin, out List<Tile> tileCandidates)
        //{
        //    foreach( var segment in segments)
        //    {
        //        CubeCoord neighborHexCoord = coordOrigin + CubeCoord.GetToNeighborCoord(segment..Side);
        //    }


        //    //CubeCoord neighborHexCoord = against.Coord + CubeCoord.GetToNeighborCoord(gateFrom.EntryNode.Side);
        //}

        //public static void ConnectTile(TileConnectivity fromTile, Tile toTile)
        //{
        //    for (int i = 0; i < fromTile.segments.Length; ++i)
        //    {
        //        Gate gateFrom = Gates[i];
        //        CubeCoord neighborHexCoord = tile.Coord + CubeCoord.GetToNeighborCoord(gateFrom.EntryNode.Side);

        //        if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile neighborTileData))
        //            gateFrom.Connect(neighborTileData.Gates);
        //    }
        //}

        //public void ConnectTile(Tile tile)
        //{
        //    for (int i = 0; i < segments.Length; ++i)
        //    {


        //        Gate gateFrom = Gates[i];
        //        CubeCoord neighborHexCoord = tile.Coord + CubeCoord.GetToNeighborCoord(gateFrom.EntryNode.Side);

        //        if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile neighborTileData))
        //            gateFrom.Connect(neighborTileData.Gates);
        //    }
        //}

        //public void DisconnectTile()
        //{
        //    for (int i = 0; i < Gates.Length; ++i)
        //        Gates[i].Disconnect();
        //}

        [ContextMenu("Get Segments")]
        private void GetSegments()
        {
            segments = GetComponentsInChildren<TileSegment>();
        }
    }
}