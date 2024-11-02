using Greenyas.Hexagon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexaLinks.Tile
{
    public class TileConnectivity : MonoBehaviour
    {
        [SerializeField]
        private TileSegment[] segments = null;

        public Gate.ExposedGate[] StartingGates => segments.Where(s => s is TerminalSegment).SelectMany(s => s.ExposedGates).ToArray();

        public struct TileQueryResult
        {
            public Tile toTile;
            public SideGate fromGate;
        }

        public List<TileQueryResult> GetNeighborCandidates(CubeCoord coord)
        {
            List<TileQueryResult> candidates = new List<TileQueryResult>();

            foreach (var s in segments)
                candidates.AddRange(s.GetCandidates(coord));

            return candidates;
        }     
        
        public void GetPossibleConnections(SideGate againstGate, SideGate.ConnectionCandidates candidatesResult)
        {
            for (int i = 0; i < segments.Length; ++i)
                segments[i].GetPossibleConnections(againstGate, candidatesResult);
        }

        public void Disconnect()
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

        //public void InitializeSegments(int numOfGates)
        //{
        //    GetSegments();
        //    foreach(TileSegment s in segments)
        //    {
        //        s.InitializeGates(numOfGates);
        //    }
        //}

        //public void DrawDebugInfo()
        //{
        //    foreach (TileSegment s in segments)
        //    {
        //        s.DrawDebugInfo();
        //    }
        //}
#endif
    }
}