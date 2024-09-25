#if UNITY_EDITOR
using Hexagon.Tile.Debug;
#endif

using System.Collections.Generic;
using Greenyas.Hexagon;
using UnityEditor;
using UnityEngine;
using static Hexalinks.Tile.TileConnectivity;
using UnityEngine.Assertions;
using System.Linq;

namespace Hexalinks.Tile
{
    public class TileSegment : MonoBehaviour
    {
        [SerializeField]
        private Gate[] gates;  
        
        public Gate.ExposedGate[] Gates => gates.Select(g => new Gate.ExposedGate(g)).ToArray();

        public List<TileQueryResult> GetCandidates(CubeCoord fromCoord)
        {
            List<TileQueryResult> candidates = new List<TileQueryResult>();

            for (int i = 0; i < gates.Length; ++i)
            {
                CubeCoord neighborHexCoord = fromCoord + CubeCoord.GetToNeighborCoord(gates[i].WorldSide);

                if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile neighborTileData))
                    candidates.Add(new()
                    {
                        tile = neighborTileData,
                        gate = gates[i]
                    });
            }

            return candidates;
        }

        public void TryConnection(Gate againstGate)
        {
            foreach (var fromGate in gates)
                fromGate.TryConnect(againstGate);
        }

        public void Disconnect()
        {
            foreach (var from in gates)
                from.Disconnect();
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
        public void DrawDebugInfo()
        {
            if (TileDebugOptions.Instance.showSegments && gates.Length == 2)
            {
                Handles.color = CustomColors.darkOrange;
                Handles.DrawLine(gates[0].WorldDebugPos, gates[1].WorldDebugPos, 2f);
            }

            for (int i = 0; i < gates.Length; i++)
                gates[i].DrawDebugInfo();
        }

        public void InitializeGates(int numOfGates)
        {
            Assert.IsTrue(numOfGates >= 1 && numOfGates <= 2);

            switch (numOfGates)
            {
                case (1): gates = Gate.CreateUnlinkedGate(this); break;
                case (2): gates = Gate.CreateLinkedGates(this); break;
                default:
                    break;
            }
        }
#endif
    }

}