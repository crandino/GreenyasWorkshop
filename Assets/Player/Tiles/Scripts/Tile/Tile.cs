using Greenyas.Hexagon;
using HexaLinks.Turn;
using System.Linq;
using UnityEngine;

namespace HexaLinks.Tile
{
    public class Tile : MonoBehaviour, IHashable<CubeCoord, uint>
    {
        [SerializeField]
        private TileManipulator manipulator;
        [SerializeField]
        protected TileCoordinates coordinates;
        [SerializeField]
        protected TileConnectivity connectivity;

        public CubeCoord Coord => coordinates.Coord;
        public TileConnectivity Connectivity => connectivity;

        public virtual void Initialize()
        {
            manipulator.Initialize(coordinates);
        }

        public virtual void Terminate()
        {
            manipulator.Terminate();
        }

        public void PickUp()
        {
            manipulator.PickUp();
        }

        public virtual bool TryRelease()
        {
            ConnectionCandidates candidates = connectivity.GetNeighborCandidates(Coord);

            HexMap hexMap = Game.Instance.GetSystem<HexMap>();

            if (hexMap.NumOfTiles == 0 || candidates.AtLeastOneConnection)
            {
                manipulator.Release();

                hexMap.AddTile(this);
#if RECORDING
                Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new AddTileRecord(hexMap, this));
#endif
                candidates.ConnectPairs(true);
#if DEBUG
                this.AddMeaningfulName();
#endif
                return true;
            }

            return false;
        }

        public virtual void Connect()
        {
            ConnectionCandidates candidates = connectivity.GetNeighborCandidates(Coord);
            candidates.ConnectPairs(false);
        }

        public virtual void Disconnect()
        {
            connectivity.Disconnect();
        }

        public uint HashFunction(CubeCoord c) => (uint)((c.Q * c.Q) * 13 + (c.Q * c.Q) * 3 +
                                                        (c.R * c.R) + (c.R * c.R) * 17 +
                                                        (c.S * c.S) * 7 + (c.S * c.S) * 11);

        public uint Hash => HashFunction(Coord);

        //private void SendEventsAccordingTo(ConnectionCandidate[] connectionCandidates)
        //{
        //    bool[] sideEvaluated = Enumerable.Repeat(false, HexSide.TOTAL_SIDES).ToArray();

        //    for (int i = 0; i < connectionCandidates.Length; i++)
        //    {
        //        int sideIndex = (int)connectionCandidates[i].From.WorldSide;

        //        if (sideEvaluated[sideIndex])
        //            continue;

        //        if (connectionCandidates[i].To.Length > 0)
        //        {
        //            Debug.Log("Calling face connected");
        //            SideGate.Events.OnSegmentConnected.Call();
        //        }
        //        else
        //        {
        //            Debug.Log("Calling face blocked");
        //            SideGate.Events.OnSegmentBlocked.Call();
        //        }

        //        sideEvaluated[sideIndex] = true;
        //    }
        //}

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
        }
#endif
    }
}
