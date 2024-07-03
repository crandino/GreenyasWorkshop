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

        public CubeCoord Coord { private set; get; }
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

        public void SetOnGrid()
        {
            Coord = manipulator.SetOnGrid();
        }

#if UNITY_EDITOR

        public void MoveUp()
        {
            Coord = HexTools.GetNearestCubeCoord(transform.position);

            Coord += CubeCoord.GetToNeighborCoord(HexSide.Side.North);
            transform.position = HexTools.GetGridCartesianWorldPos(Coord);
        }


        public void MoveDown()
        {
            Coord = HexTools.GetNearestCubeCoord(transform.position);

            Coord += CubeCoord.GetToNeighborCoord(HexSide.Side.South);
            transform.position = HexTools.GetGridCartesianWorldPos(Coord);
        }

        public void MoveRight()
        {
            Coord = HexTools.GetNearestCubeCoord(transform.position);

            Coord += CubeCoord.GetToNeighborCoord(HexSide.Side.NorthEast);
            transform.position = HexTools.GetGridCartesianWorldPos(Coord);
        }

        public void MoveLeft()
        {
            Coord = HexTools.GetNearestCubeCoord(transform.position);

            Coord += CubeCoord.GetToNeighborCoord(HexSide.Side.NorthWest);
            transform.position = HexTools.GetGridCartesianWorldPos(Coord);
        }

#endif
    }
}
