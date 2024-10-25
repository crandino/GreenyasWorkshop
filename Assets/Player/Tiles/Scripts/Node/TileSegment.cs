using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;
using static HexaLinks.Tile.TileConnectivity;

namespace HexaLinks.Tile
{
    public abstract class TileSegment : MonoBehaviour, IHashable<TileSegment, uint>
    {
        public virtual bool CanBeCrossed => true;

        public abstract Gate[] AllGates { get; }

        public Gate.ExposedGate[] ExposedGates => AllGates.ToExposedGates();

        protected abstract SideGate[] SideGates { get; }

        public uint Hash => HashFunction(this);
        public uint HashFunction(TileSegment s) => s.transform.GetTransformUpUntil<Tile>().GetComponent<Tile>().Hash + (uint)(s.transform.GetSiblingIndex() + 1);

        public List<TileQueryResult> GetCandidates(CubeCoord fromCoord)
        {
            List<TileQueryResult> candidates = new List<TileQueryResult>();

            for (int i = 0; i < SideGates.Length; ++i)
            {
                CubeCoord neighborHexCoord = fromCoord + CubeCoord.GetToNeighborCoord(SideGates[i].WorldSide);

                if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile neighborTileData))
                    candidates.Add(new()
                    {
                        toTile = neighborTileData,
                        fromGate = SideGates[i]
                    });
            }

            return candidates;
        }

        //public void TryConnection(SideGate againstGate, List<SideGate.ConnectionCandidate> candidates)
        //{
        //    foreach (var fromGate in SideGates)
        //        fromGate.TryConnect(againstGate);
        //}

        public void GetPossibleConnections(SideGate againstGate, SideGate.ConnectionCandidates candidatesResult)
        {
            foreach (var fromGate in SideGates)
                fromGate.GetPossibleConnections(againstGate, candidatesResult);
        }

        public void Disconnect()
        {
            foreach (var from in AllGates)
                from.Disconnect();
        }

        public abstract Gate GoThrough(Gate enterGate);

        public bool IsTraversalForward(Gate enterGate) => AllGates[0] == enterGate;

#if UNITY_EDITOR

        protected virtual void OnDrawGizmos()
        {
            for (int i = 0; i < AllGates.Length; i++)
                AllGates[i].DrawDebugInfo();
        }

        protected Gate CreateGate()
        {
            return gameObject.AddComponent<Gate>();
        }

        protected SideGate CreateSideGate()
        {
            return gameObject.AddComponent<SideGate>();
        }
#endif
    }

}