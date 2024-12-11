using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;

namespace HexaLinks.Tile
{
    public class TileConnectivity : MonoBehaviour
    {
        [SerializeField]
        private TileSegment[] segments = null;

        public List<ConnectionCandidate> GetNeighborCandidates(CubeCoord coord)
        {
            List<ConnectionCandidate> candidates = new List<ConnectionCandidate>();

            foreach (var s in segments)
                candidates.AddRange(s.GetCandidates(coord));

            return candidates;
        }  

        public SideGate[] GetAlignedGatesAgainst(SideGate gate)
        {
            List<SideGate> alignedGates = new();

            for (int i = 0; i < segments.Length; ++i)
                segments[i].GetAlignedGatesAgainst(gate, alignedGates);

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