using Greenyas.Hexagon;
using HexaLinks.Tile.Extensions.Hexside;
using System.Collections.Generic;
using UnityEngine;
using static Greenyas.Hexagon.HexSide;

namespace HexaLinks.Tile
{
    public class TileConnectivity : MonoBehaviour
    {
        [SerializeField]
        private TileSegment[] segments = null;

        public ConnectionCandidates GetNeighborCandidates(CubeCoord coord)
        {
            ConnectionCandidates candidates = new ConnectionCandidates();

            for (Side s = Side.North; s <= Side.NorthWest; ++s)
            {
                CubeCoord neighborHexCoord = coord + CubeCoord.GetToNeighborCoord(s);

                if (Game.Instance.GetSystem<HexMap>().TryGetTile(neighborHexCoord, out Tile neighborTileData))
                {
                    candidates.AddFromGates(GetAlignedGatesOnSide(s), s);
                    candidates.AddToGates(neighborTileData.Connectivity.GetAlignedGatesOnSide(s.GetOpposite()), s);
                }
            }

            return candidates;
        }  

        public SideGate[] GetAlignedGatesOnSide(HexSide.Side side)
        {
            List<SideGate> alignedGates = new();

            for (int i = 0; i < segments.Length; ++i)
                alignedGates.AddRange(segments[i].GetAlignedGatesOnSide(side));

            return alignedGates.ToArray();
        }

        public void Disconnect()
        {
            for (int i = 0; i < segments.Length; ++i)
                segments[i].Disconnect();
        }

#if UNITY_EDITOR
        [ContextMenu("Get Segments")]
        private void GetSegments()
        {
            segments = GetComponentsInChildren<TileSegment>();
        }
#endif
    }
}