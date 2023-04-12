using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Greenyas.Hexagon.HexSide;

[System.Serializable]
public class Node
{
#if UNITY_EDITOR
    [SerializeField]
    public Vector3 localDebugPosition;

    [SerializeField]
    private Transform tileTransform;

    public Vector3 WorldDebugPos => tileTransform.position + tileTransform.rotation * localDebugPosition;
#endif

    [SerializeField]
    private HexSide hexSide;

    public Side Side => hexSide.WorldSide;

    public List<LinkPoint> OutwardLinks { private set; get; } = new List<LinkPoint>();

    public struct LinkPoint
    {
        public TilePath path;
        public Node entryPoint;

        public LinkPoint(TilePath path, Node entry)
        {
            this.path = path;
            entryPoint = entry;
        }

        public void Connect(LinkPoint linkPoint)
        {
            entryPoint.Connect(linkPoint.path, linkPoint.entryPoint);
        }
    }

    public bool IsFacing(Node node) => hexSide.WorldSide == node.Side.GetOpposite();

    public void TryDisconnection()
    {
        LinkPoint[] disconnectedNodes = Disconnect();
        for (int i = 0; i < disconnectedNodes.Length; i++)
        {
            disconnectedNodes[i].entryPoint.Disconnect();
        }
    }

    private void Connect(TilePath path, Node entryNode)
    {
        OutwardLinks.Add(new LinkPoint(path, entryNode));
    }

    public LinkPoint[] Disconnect()
    {
        LinkPoint[] disconnectedNodes = new LinkPoint[OutwardLinks.Count];

        OutwardLinks.CopyTo(disconnectedNodes);
        OutwardLinks.Clear();

        return disconnectedNodes;
    }

    public void RotateClockwise() => hexSide.RotateClockwise();
    public void RotateCounterClockwise() => hexSide.RotateCounterClockwise();

#if UNITY_EDITOR
    public void ShowDebugInfo()
    {
        // Node position for debug purposes
        Gizmos.color = CustomColors.darkOrange;
        Gizmos.DrawSphere(WorldDebugPos, 0.05f);

        // Outward node conntections
        Handles.color = OutwardLinks.Count != 0 ? Color.green : Color.red;
        Vector2 vec = CubeCoord.GetVectorToNeighborHexOn(hexSide.WorldSide);
        Quaternion arrowOrientatinon = Quaternion.LookRotation(new Vector3(vec.x, 0f, vec.y));
        Handles.ArrowHandleCap(0, WorldDebugPos, arrowOrientatinon, 0.2f, EventType.Repaint);
    }
#endif
}
