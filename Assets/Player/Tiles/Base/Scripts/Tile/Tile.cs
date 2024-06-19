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

        private Data data;

        public class Data
        {
            public CubeCoord Coord { private set; get; }
            public Node.Gate[] Gates { private set; get; }

            private readonly Tile self;

            public Data(Tile tile)
            {
                self = tile;

                SetOnGrid();
                Gates = tile.connectivity.Gates;
            }

            public void SetOnGrid()
            {
                Coord = HexTools.GetNearestCubeCoord(self.transform.position);
                self.transform.position = HexTools.GetCartesianWorldPos(Coord);
            }
        }

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

            data = new Data(this);
            HexMap.Instance.AddTile(data);
        }

        public void PickUp()
        {
            manipulator.OnPickUp(data);
            connectivity.OnPickUp(data);

            HexMap.Instance.RemoveTile(data.Coord);
        }

        public void Release()
        {
            manipulator.OnRelease(data);
            connectivity.OnRelease(data);

            HexMap.Instance.AddTile(data);
            TileIterator.LookForClosedPaths();
        }
    }
}
