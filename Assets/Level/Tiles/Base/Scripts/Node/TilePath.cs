using Greenyas.Hexagon;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class TilePath
{
    [SerializeField]
    protected Node[] nodes;

    public TilePath(int nodesPerPath)
    {
        nodes = new Node[nodesPerPath];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Node();
        }
    }

    public Node GoThrough(Node entry)
    {
        Assert.IsTrue(nodes.Contains(entry) && !IsStarter, $"Node {entry} doesn't belong to this path or is a Starter");
        return entry == nodes[0] ? nodes[1] : nodes[0];
    }

    public bool IsStarter => nodes.Length == 1;

    public void GetAllConnections(List<Connection> connections)
    {
        for (int i = 0; i < nodes.Length; i++)
            connections.AddRange(nodes[i].Links);
    }

    public void SearchCandidateAgainst(Connection candidate, List<Connection> candidates)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (candidate.IsFacing(nodes[i]))
                candidates.Add(new Connection(this, nodes[i]));
        }
    }

    public void SearchCandidates(CubeCoord tileCoord, List<Connection> candidates)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            CubeCoord toNeighborHexCoord = CubeCoord.GetNeighborCoord(tileCoord, nodes[i].Side);
            if (HexMap.Instance.IsTileOn(toNeighborHexCoord))
                candidates.Add(new Connection(this, nodes[i]));
        }
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
    public void ShowPath()
    {
        if (DebugOptions.showTilePaths && nodes.Length == 2)
        {
            Handles.color = CustomColors.darkOrange;
            Handles.DrawLine(nodes[0].WorldDebugPos, nodes[1].WorldDebugPos, 2f);
        }

        for (int i = 0; i < nodes.Length; i++)
            nodes[i].ShowDebugInfo();
    }
#endif
}
