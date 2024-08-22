using Greenyas.Hexagon;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        private TileManipulator manipulator;
        [SerializeField]
        private TileConnectivity connectivity;

        public Node.Gate[] Gates { private set; get; }

        public CubeCoord Coord => manipulator.Coord;

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
            connectivity.Initialize();
        }

        public void PickUp()
        {
            manipulator.PickUp();
            connectivity.DisconnectTile();

            HexMap.Instance.RemoveTile(Coord);
        }

        public void Release()
        {
            manipulator.Release();
            connectivity.ConnectTile(this);

            HexMap.Instance.AddTile(manipulator.Coord, this);
            TileIterator.LookForClosedPaths();
        }
    }
}
