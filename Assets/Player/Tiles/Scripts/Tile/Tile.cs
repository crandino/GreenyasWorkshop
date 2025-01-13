using Greenyas.Hexagon;
using HexaLinks.Turn;
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
            ConnectionCandidate[] connectionCandidates = connectivity.GetNeighborCandidates(Coord).ToArray();

            HexMap hexMap = Game.Instance.GetSystem<HexMap>();

            if (hexMap.NumOfTiles == 0 || connectionCandidates.AreValid())
            {
                manipulator.Release();

                hexMap.AddTile(this);

#if UNITY_EDITOR && DEBUG
                Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new AddTileRecord(hexMap, this));
#endif
                Connect(connectionCandidates);
#if DEBUG
                this.AddMeaningfulName();
#endif
                return true;
            }

            return false;
        }

        public virtual void Connect(ConnectionCandidate[] connectionCandidates = null)
        {
            connectionCandidates ??= connectivity.GetNeighborCandidates(Coord).ToArray();
            connectionCandidates.Connect();
        }

        public virtual void Disconnect()
        {
            connectivity.Disconnect();
        }

        public uint HashFunction(CubeCoord c) => (uint)((c.Q * c.Q) * 13 + (c.Q * c.Q) * 3    +
                                                        (c.R * c.R)      + (c.R * c.R) * 17   +
                                                        (c.S * c.S) * 7  + (c.S * c.S) * 11);

        public uint Hash => HashFunction(Coord);

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
