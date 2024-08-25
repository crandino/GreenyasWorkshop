using Greenyas.Hexagon;
using System.Collections.Generic;
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
        private HexSide hexSide;
        [SerializeField]
        private TileSegment segment;

        public Side WorldSide => hexSide.GetWorldSide(segment);

        public Gate(TileSegment seg)
        {
            segment = seg;
            connections = new List<Gate>();
        }

        [SerializeReference]
        private List<Gate> connections;

        private bool IsFacingOtherGate(Gate gateTo)
        {
            Assert.IsTrue(segment != gateTo.segment);
            return WorldSide.IsOpposite(gateTo.WorldSide);
        }

        public void TryConnect(Gate againstGate)
        {
            if (IsFacingOtherGate(againstGate))
            {
                connections.Add(againstGate);
                againstGate.connections.Add(this);
            };
        }

        public void Disconnect()
        {
            foreach(var conn in connections)
                conn.connections.Clear();

            connections.Clear();
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

        public void OnDrawGizmos()
        {
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
                Handles.color = textStyle.normal.textColor = connections.Count != 0 ? Color.green : Color.red;
                Handles.Label(WorldDebugPos + toNextTile * 0.3f, $"{connections.Count}", textStyle);

                Handles.ArrowHandleCap(0, WorldDebugPos, arrowOrientatinon, 0.2f, EventType.Repaint);
            }
        }
#endif
    }

}