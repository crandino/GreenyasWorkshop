using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;
using Hexalinks.PathFinder;

namespace Hexalinks.Tile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        private TileManipulator manipulator;
        [SerializeField]
        protected TileConnectivity connectivity;

        [SerializeField]
        protected TileCoordinates coordinates;

        public CubeCoord Coord => coordinates.Coord;
        public Gate.ExposedGate[] StartingGates => connectivity.StartingGates;

        public enum Type
        {
            FlowStraight,
            FlowSimpleShortCurve,
            FlowSimpleLongCurve,
            FlowDoubleNearShortCurve,
            FlowDoubleFarShortCurve,
            FlowDoubleLongCurve,
            FlowTripleShortCurve,

            SplitLongCurve,

            FillerSimple
        }        

        public void Initialize()
        {
            manipulator.Initialize(coordinates);
        }

        public void PickUp()
        {
            //connectivity.Disconnection();
            manipulator.PickUp();

            //HexMap.Instance.RemoveTile(Coord);
        }

        public void Release()
        {
            manipulator.Release();

            HexMap.Instance.AddTile(this);
            Connect();

            PathIterator.ExplorePathsFrom(this);
        }

        public void Connect()
        {
            List<TileConnectivity.TileQueryResult> candidates = connectivity.GetNeighborCandidates(Coord);

            for (int i = 0; i < candidates.Count; ++i)
                candidates[i].tile.connectivity.TryConnection(candidates[i].gate);
        }

#if UNITY_EDITOR
        public TileCoordinates Coordinates => coordinates;       

        private void OnValidate()
        {
            manipulator = GetComponent<TileManipulator>();
            connectivity = GetComponent<TileConnectivity>();

            coordinates = new TileCoordinates(transform);
        }

        private void OnDrawGizmos()
        {
            manipulator.DrawDebugInfo();
            //connectivity.DrawDebugInfo();
        }
#endif
    }
}
