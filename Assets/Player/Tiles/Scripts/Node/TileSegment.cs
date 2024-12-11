using Greenyas.Hexagon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexaLinks.Tile
{
    public class TileSegment : MonoBehaviour, IHashable<TileSegment, uint>
    {
        [SerializeField]
        private Gate[] gates = null;

        protected SideGate[] SideGates => gates.Where(g => g is SideGate).Cast<SideGate>().ToArray();

        public uint Hash => HashFunction(this);
        public uint HashFunction(TileSegment s) => s.transform.GetTransformUpUntil<Tile>().GetComponent<Tile>().Hash + (uint)(s.transform.GetSiblingIndex() + 1);

        public List<ConnectionCandidate> GetCandidates(CubeCoord fromCoord)
        {
            List<ConnectionCandidate> candidates = new List<ConnectionCandidate>();

            for (int i = 0; i < SideGates.Length; ++i)
            {
                CubeCoord neighborHexCoord = fromCoord + CubeCoord.GetToNeighborCoord(SideGates[i].WorldSide);

                if (Game.Instance.GetSystem<HexMap>().TryGetTile(neighborHexCoord, out Tile neighborTileData))
                    candidates.Add(new(neighborTileData, SideGates[i]));
            }

            return candidates;
        }

        public void GetAlignedGatesAgainst(SideGate gate, List<SideGate> alignedGates)
        {
            foreach (var fromGate in SideGates)
                fromGate.GetAlignedGatesAgainst(gate, alignedGates);
        }

        public void Disconnect()
        {
            foreach (var gate in SideGates)
                SideGate.Disconnect(gate);
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