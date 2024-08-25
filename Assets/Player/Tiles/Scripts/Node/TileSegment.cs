#if UNITY_EDITOR
using Greenyas.Hexagon;
using Hexagon.Tile.Debug;
using System.Collections.Generic;
#endif

using UnityEditor;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class TileSegment : MonoBehaviour
    {
        [SerializeField]
        protected Gate[] gates = new Gate[2];

        [SerializeField]
        private MeshRenderer meshRenderer;

        public List<Tile> GetCandidates(CubeCoord fromCoord)
        {
            List<Tile> candidates = new List<Tile>();

            for (int i = 0; i < gates.Length; ++i)
            {
                Gate gateFrom = gates[i];
                HexSide.Side worldSide = gateFrom.WorldSide;
                CubeCoord neighborHexCoord = fromCoord + CubeCoord.GetToNeighborCoord(worldSide);

                if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile neighborTileData))
                    candidates.Add(neighborTileData);
            }

            return candidates;
        }      

        public void TryConnection(TileSegment[] toSegments)
        {
            foreach (var from in gates)
            {
                foreach (var segmentTo in toSegments)
                {
                    from.Connect(segmentTo.gates);
                }

            }
        }

        public void Disconnect()
        {
            foreach (var from in gates)
            {
                from.Disconnect();
            }
        }

        public void Highlight()
        {
            //Vector4 values = meshRenderer.material.GetVector(pathEmissionID);
            //values[emissionPathIndex] = 1f;
            //meshRenderer.material.SetVector(pathEmissionID, values);
        }

        public void Unhighlight()
        {
            //Vector4 values = meshRenderer.material.GetVector(pathEmissionID);
            //values[emissionPathIndex] = 0f;
            //meshRenderer.material.SetVector(pathEmissionID, values);
        }

       

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Safeguard to not render anything before initialization
            if (meshRenderer == null)
                return;

            if (TileDebugOptions.Instance.showSegments)
            {
                Handles.color = CustomColors.darkOrange;
                Handles.DrawLine(gates[0].WorldDebugPos, gates[1].WorldDebugPos, 2f);
            }

            for (int i = 0; i < gates.Length; i++)
                gates[i].OnDrawGizmos();
        }

        private void Reset()
        {
            meshRenderer = GetComponent<MeshRenderer>();

            for (int i = 0; i < gates.Length; ++i)
            {
                gates[i] = new Gate(this);
            }
        }
#endif
    }

}