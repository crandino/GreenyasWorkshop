using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;
using static Hexalinks.Tile.TileConnectivity;

namespace Hexalinks.Tile
{
    public abstract class TileSegment : MonoBehaviour, Tile.IHashable
    {
        public virtual bool CanBeCrossed => true;

        public abstract Gate[] AllGates { get; }

        public Gate.ExposedGate[] ExposedGates => AllGates.ToExposedGates();

        protected abstract SideGate[] SideGates { get; }

        public uint Hash => transform.GetTransformUpUntil<Tile>().GetComponent<Tile>().Hash + ((uint)transform.GetSiblingIndex() + 1);

        public List<TileQueryResult> GetCandidates(CubeCoord fromCoord)
        {
            List<TileQueryResult> candidates = new List<TileQueryResult>();

            for (int i = 0; i < SideGates.Length; ++i)
            {
                CubeCoord neighborHexCoord = fromCoord + CubeCoord.GetToNeighborCoord(SideGates[i].WorldSide);

                if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile neighborTileData))
                    candidates.Add(new()
                    {
                        tile = neighborTileData,
                        gate = SideGates[i]
                    });
            }

            return candidates;
        }

        public void TryConnection(SideGate againstGate)
        {
            foreach (var fromGate in SideGates)
                fromGate.TryConnect(againstGate);
        }

        public void Disconnect()
        {
            foreach (var from in AllGates)
                from.Disconnect();
        }

        public abstract Gate GoThrough(Gate enterGate);

        public bool IsOriginalDirection(Gate enterGate) => AllGates[0] == enterGate;

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


        //private void Reset()
        //{
        //    if(AllGates.Length == 0 || AllGates.Any(g => g == null))
        //        InitializeGates();
        //}

        //protected abstract void InitializeGates();
#endif
    }

}