#if UNITY_EDITOR
using Hexagon.Tile.Debug;
#endif

using System.Collections.Generic;
using Greenyas.Hexagon;
using UnityEditor;
using UnityEngine;
using static Hexalinks.Tile.TileConnectivity;
using System.Linq;

namespace Hexalinks.Tile
{
    public class TileSegment : MonoBehaviour
    {
        [SerializeField]
        private Gate[] gates;

        public bool IsLooped => gates[0].WorldSide == gates[1].WorldSide;
        
        public Gate.ExposedGate[] Gates => IsLooped ? new []{ new Gate.ExposedGate(gates[0]) } : gates.Select(g => new Gate.ExposedGate(g)).ToArray();

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

        public Gate GoThrough(Gate enterGate)
        {
            return gates.Where(g => g != enterGate).First();
        }

#if UNITY_EDITOR

        public bool AreGatesInitialized => gates != null && gates.Length != 0;

        public void DrawDebugInfo()
        {
            if (TileDebugOptions.Instance.showSegments && AreGatesInitialized)
            {
                Handles.color = CustomColors.darkOrange;
                Handles.DrawLine(gates[0].WorldDebugPos, gates[1].WorldDebugPos, 2f);
            }

            for (int i = 0; i < gates.Length; i++)
                gates[i].DrawDebugInfo();
        }

        private void OnValidate()
        {
            if (AreGatesInitialized)
                return;

            InitializeGates();
        }

        [ContextMenu("Initialize gates")]
        private void InitializeGates()
        {
            gates = new Gate[2];
            gates[0] = new Gate(this);
            gates[1] = new Gate(this);
        }
#endif
    }

}