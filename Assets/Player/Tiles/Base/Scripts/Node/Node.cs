using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Greenyas.Hexagon.HexSide;

#if UNITY_EDITOR
using Hexagon.Tile.Debug;
#endif

namespace Hexalinks.Tile
{
    [System.Serializable]
    public class Node
    {
        [SerializeField]
        private HexSide hexSide;

        public Side Side => hexSide.WorldSide;

        public class Gate
        {
            public Node Node { private set; get; }
            public TileSegment Segment { private set; get; }

            public Gate(TileSegment seg, Node entryNode)
            {
                Node = entryNode;
                Segment = seg;
            }
        }       

        public List<Gate> Connections { private set; get; } = new List<Gate>();

#if UNITY_EDITOR
        [SerializeField]
        public Vector3 localDebugPosition;

        [SerializeField]
        private Transform tileTransform;

        public Vector3 WorldDebugPos => tileTransform.position + tileTransform.rotation * localDebugPosition;

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
                Vector2 vec = CubeCoord.GetVectorToNeighborHexOn(hexSide.WorldSide);
                Vector3 toNextTile = new Vector3(vec.x, 0f, vec.y);
                Quaternion arrowOrientatinon = Quaternion.LookRotation(toNextTile);

                GUIStyle textStyle = new GUIStyle();
                textStyle.fontSize = 18;
                Handles.color = textStyle.normal.textColor = Connections.Count != 0 ? Color.green : Color.red;
                Handles.Label(WorldDebugPos + toNextTile * 0.3f, $"{Connections.Count}", textStyle);

                Handles.ArrowHandleCap(0, WorldDebugPos, arrowOrientatinon, 0.2f, EventType.Repaint);
            }
        }
#endif
    }

}