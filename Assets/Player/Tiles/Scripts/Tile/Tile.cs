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

        [SerializeField]
        private TileCoordinates coordinates;

        public CubeCoord Coord => coordinates.Coord;       

        public enum Type
        {
            FlowStraight,
            FlowSimpleShortCurve,
            FlowSimpleLongCurve,
            FlowDoubleNearShortCurve,
            FlowDoubleFarShortCurve,
            FlowDoubleLongCurve,
            FlowTripleShortCurve,

            SplitLongCurve
        }        

        public void Initialize()
        {
            manipulator.Initialize(coordinates);
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

            HexMap.Instance.AddTile(this);
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
        public TileCoordinates Coordinates => coordinates;       

        private void OnValidate()
        {
            coordinates = new TileCoordinates(transform);
        }   

        private void OnDrawGizmos()
        {
            manipulator.DrawDebugInfo();
            connectivity.DrawDebugInfo();
        }

#endif
    }
}
