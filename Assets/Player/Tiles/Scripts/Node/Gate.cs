using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using static Greenyas.Hexagon.HexSide;
using System.Linq;


#if UNITY_EDITOR
using Hexagon.Tile.Debug;
#endif

namespace Hexalinks.Tile
{
    [System.Serializable]
    public class Gate
    {
        [SerializeField]
        private HexSide hexSide = new();
        [SerializeField]
        private TileSegment segment;

        [SerializeField]
        private Gate inwardGate = null;

        //[SerializeReference]
        private readonly List<Gate> outwardGates = new List<Gate>();

        public Side WorldSide => hexSide.GetWorldSide(segment);

        public void TryConnect(Gate againstGate)
        {
            // There's any previous connection between those gates
            Assert.IsTrue(!outwardGates.Contains(againstGate) && againstGate.outwardGates.Where(g => g == againstGate).Count() == 0);

            if (IsFacingOtherGate(againstGate))
            {
                outwardGates.Add(againstGate);
                againstGate.outwardGates.Add(this);
            };
        }

        public void Disconnect()
        {
            foreach (var conn in outwardGates)
                conn.outwardGates.Clear();

            outwardGates.Clear();
        }

        private bool IsFacingOtherGate(Gate gateTo)
        {
            Assert.IsTrue(segment != gateTo.segment);
            return WorldSide.IsOpposite(gateTo.WorldSide);
        }

        public readonly struct ExposedGate
        {
            private readonly Gate gate;

            public ExposedGate[] OutwardGates => gate.outwardGates.Select(g => new ExposedGate(g)).ToArray();

            public ExposedGate(Gate gate)
            {
                this.gate = gate;
            }

            public ExposedGate[] GoThrough()
            {
                Assert.IsTrue(gate.inwardGate != null);
                return gate.inwardGate.outwardGates.Select(g => new ExposedGate(g)).ToArray();
            }

            public bool IsFiller => gate.inwardGate == null;
        }

#if UNITY_EDITOR

        public static Gate[] CreateUnlinkedGate(TileSegment segment)
        {
            return new Gate[]
            {
                new()
                {
                    segment = segment,
                    inwardGate = null
                }
            };
        }

        public static Gate[] CreateLinkedGates(TileSegment segment)
        {
            Gate[] gates = new Gate[2];
            gates[0] = new Gate() { segment = segment };
            gates[1] = new Gate() { segment = segment };
            gates[0].inwardGate = gates[1];
            gates[1].inwardGate = gates[0];
            return gates;
        }

        public Vector3 WorldDebugPos
        {
            get
            {
                Vector2 localDir = CubeCoord.GetVectorToNeighborHexOn(WorldSide);
                Vector3 localPosition = new Vector3(localDir.x, 0.05f, localDir.y);
                return segment.transform.position + localPosition * HexTools.hexagonSize;
            }
        }

        public void DrawDebugInfo()
        {
            // Safeguard for lazy initialization
            if (segment == null)
                return;

            // Node position for debug purposes
            if (TileDebugOptions.Instance.showNodes)
            {
                Gizmos.color = CustomColors.darkOrange;
                Gizmos.DrawSphere(WorldDebugPos, 0.05f);
            }

            // Outward node conntections
            if (TileDebugOptions.Instance.showConnections)
            {
                Vector2 vec = CubeCoord.GetVectorToNeighborHexOn(WorldSide);
                Vector3 toNextTile = new Vector3(vec.x, 0f, vec.y);
                Quaternion arrowOrientatinon = Quaternion.LookRotation(toNextTile);

                GUIStyle textStyle = new GUIStyle();
                textStyle.fontSize = 18;
                Handles.color = textStyle.normal.textColor = outwardGates.Count != 0 ? Color.green : Color.red;
                Handles.Label(WorldDebugPos + toNextTile * 0.3f, $"{outwardGates.Count}", textStyle);

                Handles.ArrowHandleCap(0, WorldDebugPos, arrowOrientatinon, 0.2f, EventType.Repaint);
            }
        }
#endif
    }

}