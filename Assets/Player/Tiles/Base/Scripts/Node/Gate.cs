using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Greenyas.Hexagon.HexSide;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using Hexagon.Tile.Debug;
#endif

namespace Hexalinks.Tile
{
    [System.Serializable]
    public class Gate
    {
        public Node node;
        public TileSegment segment;

        public Side GetWorldSide(TileSegment segment) => node.GetWorldSide(segment.transform.GetTransformUpUntil<Tile>());

        public Gate(TileSegment seg)
        {
            segment = seg;
            node = new Node();

            connections = new List<Gate>();
        }

        [System.Serializable]
        public class Node
        {
            [SerializeField]
            private HexSide hexSide;

            public Side GetWorldSide(Transform t) => hexSide.GetWorldSide(t);
        }

        [SerializeReference]
        private List<Gate> connections;

        public bool IsFacingOtherGate(Gate gateTo)
        {
            Assert.IsTrue(segment != gateTo.segment);
            return GetWorldSide(segment).IsOpposite(gateTo.GetWorldSide(gateTo.segment));
        }

        public void Connect(Gate gate)
        {
            connections.Add(gate);
            gate.Connect(this);
        }

        public void Disconnect()
        {
            foreach(var connection in connections)
                connection.connections.Clear();

            connections.Clear();
        }

#if UNITY_EDITOR
        public Vector3 WorldDebugPos
        {
            get
            {
                Vector2 localDir = CubeCoord.GetVectorToNeighborHexOn(GetWorldSide(segment));
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
                Vector2 vec = CubeCoord.GetVectorToNeighborHexOn(GetWorldSide(segment));
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