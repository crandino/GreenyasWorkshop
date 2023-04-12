using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TilePath
{
    private const int NODES_PER_PATH = 2;

    [SerializeField]
    private Node[] nodes;

    [SerializeField]
    private bool isStarter = false;

    public TilePath()
    {
        nodes = new Node[NODES_PER_PATH];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Node();
        }
    }

    public bool IsConnected
    {
        get
        {
            bool connected = true;

            //for (int i = 0; i < nodes.Length; i++)
            //    connected &= nodes[i].Outward;

            return connected;
        }
    }

    public Node GoThrough(Node entrance)
    {
        return nodes[0] == entrance ? nodes[1] : nodes[0];
    }

    public void GetLinkPoint(Node entryPoint, List<Node.LinkPoint> candidates)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].IsFacing(entryPoint))
                candidates.Add(new Node.LinkPoint(this, nodes[i]));
        }
    }

    public void GetLinkPoints(CubeCoord tileCoord, List<Node.LinkPoint> candidates)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            CubeCoord toNeighborHexCoord = CubeCoord.GetNeighborCoord(tileCoord, nodes[i].Side);
            if (HexMap.Instance.IsTileOn(toNeighborHexCoord))
                candidates.Add(new Node.LinkPoint(this, nodes[i]));
        }
    }

    public void Disconnect()
    {
        for (int i = 0; i < nodes.Length; i++)
            nodes[i].TryDisconnection();
    }

    public void RotateClockwise()
    {
        for (int i = 0; i < nodes.Length; i++)
            nodes[i].RotateClockwise();
    }

    public void RotateCounterClockwise()
    {
        for (int i = 0; i < nodes.Length; i++)
            nodes[i].RotateCounterClockwise();
    }

#if UNITY_EDITOR
    public void ShowConnections()
    {
        Handles.color = CustomColors.darkOrange;
        Handles.DrawLine(nodes[0].WorldDebugPos, nodes[1].WorldDebugPos, 2f);

        for (int i = 0; i < nodes.Length; i++)
            nodes[i].ShowDebugInfo();
    }
#endif
}
