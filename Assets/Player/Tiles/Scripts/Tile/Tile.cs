using Greenyas.Hexagon;
using HexaLinks.PathFinder;
using UnityEngine;

namespace HexaLinks.Tile
{
    public class Tile : MonoBehaviour, IHashable<CubeCoord, uint>
    {
        [SerializeField]
        private TileManipulator manipulator;
        [SerializeField]
        protected TileConnectivity connectivity;

        [SerializeField]
        protected TileCoordinates coordinates;

        public CubeCoord Coord => coordinates.Coord;
        public Gate.ExposedGate[] StartingGates => connectivity.StartingGates;      

        public void Initialize()
        {
            manipulator.Initialize(coordinates);
        }

        public void Terminate()
        {
            manipulator.Terminate();
        }

        public void PickUp()
        {
            manipulator.PickUp();
        }

        public bool TryRelease()
        {
            SideGate.ConnectionCandidates connectionCandidates = GetPossibleConnections();
            if (!connectionCandidates.Check(SideGate.ConnectionCandidates.AtLeastOneConnection))
                return false;

            manipulator.Release();
            HexMap.Instance.AddTile(this);

            connectionCandidates.Connect();

            PathStorage.Reset();
            PathStorage.Init(this);

            return true;
        }

        public void Connect()
        {
            GetPossibleConnections().Connect();
        }

        private SideGate.ConnectionCandidates GetPossibleConnections()
        {
            TileConnectivity.TileQueryResult[] neighborTiles = GetNeighborCandidates();
            
            SideGate.ConnectionCandidates candidates = new();
            for (int i = 0; i < neighborTiles.Length; ++i)
                neighborTiles[i].toTile.connectivity.GetPossibleConnections(neighborTiles[i].fromGate, candidates);

            return candidates;
        }

        private TileConnectivity.TileQueryResult[] GetNeighborCandidates()
        {
            return connectivity.GetNeighborCandidates(Coord).ToArray();
        }

        public void Disconnect()
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
            //connectivity.DrawDebugInfo();
        }
#endif
    }
}
