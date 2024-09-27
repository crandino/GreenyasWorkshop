using Greenyas.Hexagon;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using static Greenyas.Hexagon.HexSide;


#if UNITY_EDITOR
using Hexagon.Tile.Debug;
#endif

namespace Hexalinks.Tile
{
    [System.Serializable]
    public class Gate
    {
        [SerializeField]
        private TileSegment segment;
        [SerializeField]
        private HexSide hexSide = new();

        [SerializeReference]
        private List<Gate> outwardGates = new List<Gate>();

        public Gate(TileSegment segment)
        {
            this.segment = segment;
        }

        public Side WorldSide => hexSide.GetWorldSide(segment);

        public void TryConnect(Gate againstGate)
        {
            // There's any previous connection between those gates
            if (outwardGates.Contains(againstGate) && againstGate.outwardGates.Where(g => g == againstGate).Count() == 0)
                return;

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

        private Gate GoThrough()
        {
            return segment.GoThrough(this);
        }

        public readonly struct ExposedGate
        {
            public readonly Gate gate;

            public bool IsOutterConnected => gate.outwardGates.Count > 0;
            public bool IsLooped => gate.segment.IsLooped;

            public ExposedGate[] OutwardGates => gate.outwardGates.Select(g => new ExposedGate(g)).ToArray();

            public ExposedGate(Gate gate)
            {
                this.gate = gate;
            }

            public ExposedGate GoThrough()
            {
                return new ExposedGate(gate.GoThrough());
            } 
            
            public void Log()
            {
                Debug.Log($"Segment on tile {gate.segment.transform.parent.gameObject.name}"); 
            }

            public TileSegment Segment => gate.segment;
        }

#if UNITY_EDITOR

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
            //// Safeguard for lazy initialization
            //if (segment == null)
            //    return;

            // Node position for debug purposes
            if (TileDebugOptions.Instance.showNodes)
            {
                Gizmos.color = CustomColors.darkOrange;
                Gizmos.DrawSphere(WorldDebugPos, 0.05f);
            }

            // Outward node conntections
            if (TileDebugOptions.Instance.showConnections && Application.isPlaying)
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