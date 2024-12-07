using Greenyas.Hexagon;
using Hexagon.Tile.Debug;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static HexaLinks.Tile.TileConnectivity;

namespace HexaLinks.Tile
{
    public class TileSegment : MonoBehaviour, IHashable<TileSegment, uint>
    {
        [SerializeField]
        private Gate[] gates = null;

        public virtual bool CanBeCrossed => gates.Length == 2;

        protected SideGate[] SideGates => gates.Where(g => g is SideGate).Cast<SideGate>().ToArray();

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

            //if (TileDebugOptions.Instance.showSegments && CanBeCrossed)
            //{
            //    Vector3 arrowDir = (gates[1].WorldDebugPos - gates[0].WorldDebugPos);
            //    Vector3 arrowDirNormalized = arrowDir.normalized;
            //    float magnitude = arrowDir.magnitude;

            //    Handles.color = CustomColors.darkGreen;
            //    Vector3 lateralOffset = Vector3.Cross(arrowDir.normalized, Vector3.up) * 0.1f;
            //    Handles.ArrowHandleCap(0, gates[0].WorldDebugPos + lateralOffset, Quaternion.LookRotation(arrowDirNormalized, Vector3.up), magnitude * 0.9f, EventType.Repaint);
            //    Handles.color = CustomColors.brown;
            //    Handles.ArrowHandleCap(0, gates[1].WorldDebugPos - lateralOffset, Quaternion.LookRotation(-arrowDirNormalized, Vector3.up), magnitude * 0.9f, EventType.Repaint);
            //}
        }
#endif
    }

}