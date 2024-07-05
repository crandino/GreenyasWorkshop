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

        public CubeCoord Coord => manipulator.Coord;
        public Node.Gate[] Gates { private set; get; }

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
            manipulator.Initialize();
            connectivity.Initialize();

            HexMap.Instance.AddTile(this);
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

            HexMap.Instance.AddTile(this);
            //TileIterator.LookForClosedPaths();
        }

        //public void SetOnGrid()
        //{
        //    Coord = manipulator.SetOnGrid();
        //}
    }
}
