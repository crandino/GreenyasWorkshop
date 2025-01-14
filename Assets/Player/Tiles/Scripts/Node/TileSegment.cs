using System.Linq;
using UnityEngine;
using static Greenyas.Hexagon.HexSide;

namespace HexaLinks.Tile
{
    public class TileSegment : MonoBehaviour, IHashable<TileSegment, uint>
    {
        [SerializeField]
        private Gate[] gates = null;

        protected SideGate[] SideGates => gates.Where(g => g is SideGate).Cast<SideGate>().ToArray();

        public uint Hash => HashFunction(this);
        public uint HashFunction(TileSegment s) => s.GetComponentInParent<Tile>().Hash + (uint)(s.transform.GetSiblingIndex() + 1);

        //public void FillCandidates(CubeCoord fromCoord, ConnectionCandidates candidates)
        //{
        //    for (Side s = Side.North; s <= Side.NorthWest; ++s)
        //    {
        //        CubeCoord neighborHexCoord = fromCoord + CubeCoord.GetToNeighborCoord(s);

        //        if (Game.Instance.GetSystem<HexMap>().TryGetTile(neighborHexCoord, out Tile neighborTileData))
        //        {
        //            candidates.AddFromGates(GetAlignedGatesOnSide(s), s);
        //            candidates.AddToGates(neighborTileData.Connectivity.GetAlignedGatesOnSide(s.GetOpposite()), s);
        //        }
        //    }
        //}

        public SideGate[] GetAlignedGatesOnSide(Side side)
        {
            return SideGates.Where(s => s.WorldSide == side).ToArray();
        }

        public void Disconnect()
        {
            foreach (var gate in SideGates)
                SideGate.Disconnect(gate);
        }

        public bool IsTraversalForward(Gate enterGate) => gates[0] == enterGate;

#if UNITY_EDITOR

        private void Reset()
        {
            gates = GetComponentsInChildren<Gate>();
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