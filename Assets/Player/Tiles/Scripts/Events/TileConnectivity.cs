using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class TileConnectivity : MonoBehaviour
    {
        [SerializeField]
        private TileSegment[] segments = null;

        public struct TileQueryResult
        {
            public Tile tile;
            public Gate gate;
        }

        public List<TileQueryResult> GetNeighborCandidates(CubeCoord coord)
        {
            List<TileQueryResult> candidates = new List<TileQueryResult>();

            foreach (var s in segments)
                candidates.AddRange(s.GetCandidates(coord));

            return candidates;
        }

        public void TryConnection(Gate againstGate)
        {
            for (int i = 0; i < segments.Length; ++i)
            {
                segments[i].TryConnection(againstGate);
            }
        }

        public void Disconnection()
        {
            for (int i = 0; i < segments.Length; ++i)
            {
                segments[i].Disconnect();
            }
        }

#if UNITY_EDITOR

        [ContextMenu("Get Segments")]
        private void GetSegments()
        {
            segments = GetComponentsInChildren<TileSegment>();
        }

        public void DrawDebugInfo()
        {
            //TODO
        }

#endif


    }
}