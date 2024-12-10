using Greenyas.Hexagon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static HexaLinks.Tile.TileConnectivity;

namespace HexaLinks.Tile
{
    public class TileSegment : MonoBehaviour, IHashable<TileSegment, uint>
    {
        [SerializeField]
        private Gate[] gates = null;

        protected SideGate[] SideGates => gates.Where(g => g is SideGate).Cast<SideGate>().ToArray();

        public uint Hash => HashFunction(this);
        public uint HashFunction(TileSegment s) => s.transform.GetTransformUpUntil<Tile>().GetComponent<Tile>().Hash + (uint)(s.transform.GetSiblingIndex() + 1);

        public List<TileQueryResult> GetCandidates(CubeCoord fromCoord)
        {
            List<TileQueryResult> candidates = new List<TileQueryResult>();

            for (int i = 0; i < SideGates.Length; ++i)
            {
                CubeCoord neighborHexCoord = fromCoord + CubeCoord.GetToNeighborCoord(SideGates[i].WorldSide);

                if (Game.Instance.GetSystem<HexMap>().TryGetTile(neighborHexCoord, out Tile neighborTileData))
                    candidates.Add(new()
                    {
                        toTile = neighborTileData,
                        fromGate = SideGates[i]
                    });
            }

            return candidates;
        }

        public void GetPossibleConnections(SideGate againstGate, SideGate.ConnectionCandidates candidatesResult)
        {
            foreach (var fromGate in SideGates)
                fromGate.GetPossibleConnections(againstGate, candidatesResult);
        }

        public void Disconnect()
        {
            //TODO: Completar la desconexión
            //foreach (var from in SideGates)
            //    from.Disconnect();
        }

        public Gate GoThrough(Gate enterGate)
        {
            return gates.Where(g => g != enterGate).First();
        }

        public bool IsTraversalForward(Gate enterGate) => gates[0] == enterGate;

#if UNITY_EDITOR

        private void Reset()
        {
            gates = new Gate[0];
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < gates.Length; i++)
            {
                if (gates[i] == null)
                    continue;

                Color tint = Color.grey;
                float tintMultiplier = (gates[0] == gates[i]) ? 1.5f : 0.5f;
                gates[i].DrawDebugInfo(new Color(tint.r * tintMultiplier,
                    tint.g * tintMultiplier,
                    tint.b * tintMultiplier,
                    1.0f));
            }
        }
#endif
    }
}