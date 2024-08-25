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
        }

        public void PickUp()
        {
            connectivity.Disconnection();
            manipulator.PickUp();

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
            List<TileConnectivity.TileQueryResult> candidates = connectivity.GetNeighborCandidates(Coord);

            for (int i = 0; i < candidates.Count; ++i)
            {
                candidates[i].tile.connectivity.TryConnection(candidates[i].gate);
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            manipulator.DrawDebugInfo();
            connectivity.DrawDebugInfo();
        }

#endif
    }
}
