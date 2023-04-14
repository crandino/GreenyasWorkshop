using Greenyas.Hexagon;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

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
        Assert.IsTrue(nodes.Contains(entry) && IsStarter, $"Node {entry} doesn't belong to this path or is a Starter");
        return entry == nodes[0] ? nodes[1] : nodes[0];
    }

    public bool IsStarter => nodes.Length == 1;

    public Link[] GetAllLinks()
    {
        List<Link> links = new List<Link>();

        for (int i = 0; i < nodes.Length; i++)
            links.AddRange(nodes[i].Links);

        return links.ToArray();
    }

    public void SearchCandidateAgainst(Candidate candidate, List<Candidate> candidates)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (candidate.IsFacing(nodes[i]))
                candidates.Add(new Candidate(this, nodes[i]));
        }
    }

    public void SearchCandidates(CubeCoord tileCoord, List<Candidate> candidates)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            CubeCoord toNeighborHexCoord = CubeCoord.GetNeighborCoord(tileCoord, nodes[i].Side);
            if (HexMap.Instance.IsTileOn(toNeighborHexCoord))
                candidates.Add(new Candidate(this, nodes[i]));
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

    public class Candidate
    {
        private TilePath path;
        private Node node;

        public HexSide.Side Side => node.Side;

        public Candidate(TilePath path, Node nodeToExplore)
        {
            this.path = path;
            node = nodeToExplore;
        }

        public bool IsFacing(Node againstNode)
        {
            return node.IsFacing(againstNode);
        }

        public void Connect(Candidate[] candidates, bool bidirectional = true)
        {
            for (int i = 0; i < candidates.Length; i++)
            {
                node.Links.Add(candidates[i]);
                if (bidirectional)
                    candidates[i].node.Links.Add(this);
            }
        }       

        public static implicit operator Link(Candidate candidate)
        {
            return new Link(candidate.path, candidate.node);
        }

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
