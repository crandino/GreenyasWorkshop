using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class TileConnectivity : MonoBehaviour
    {
        [SerializeField]
        private TileSegment[] segments = null;

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

#if UNITY_EDITOR

        [ContextMenu("Get Segments")]
        private void GetSegments()
        {
            segments = GetComponentsInChildren<TileSegment>();
        }

        public void DrawDebugInfo()
        {

        }

#endif


    }
}