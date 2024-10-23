using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;
using Hexalinks.PathFinder;
using static Hexalinks.Tile.Tile;
using System;

namespace Hexalinks.Tile
{
    public class Tile : MonoBehaviour, IHashable
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

            FillerSimple,
            FillerDouble,
            FillerTriple
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

        public bool TryRelease()
        {
            SideGate.ConnectionCandidates connectionCandidates = GetPossibleConnections();
            if (!connectionCandidates.Check(SideGate.ConnectionCandidates.AtLeastOneConnection))
                return false;

            manipulator.Release();
            HexMap.Instance.AddTile(this);

            connectionCandidates.Connect();

            PathStorage.InitNewPropagation(true);
            PathIterator.FindPathsFrom(this);

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

        public uint Hash => (uint)((Coord.Q * Coord.Q) + (Coord.Q * Coord.Q) * 3 +
                                   (Coord.R * Coord.R) * 13 + (Coord.R * Coord.R) * 4 +
                                   (Coord.S * Coord.S) * 7 + (Coord.S * Coord.S) * 2);

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

        public interface IHashable
        {
            public uint Hash { get; }
        }
    }
}
