using Greenyas.Hexagon;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class TileSegment : MonoBehaviour
{
    [SerializeField]
    protected Node[] nodes;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private int emissionPathIndex;

    private static int pathEmissionID = Shader.PropertyToID("_EmissionPathSelector");

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

    public void Highlight()
    {
        Vector4 values = meshRenderer.material.GetVector(pathEmissionID);
        values[emissionPathIndex] = 1f;
        meshRenderer.material.SetVector(pathEmissionID, values);
    }

    public void Unhighlight()
    {
        Vector4 values = meshRenderer.material.GetVector(pathEmissionID);
        values[emissionPathIndex] = 0f;
        meshRenderer.material.SetVector(pathEmissionID, values);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (DebugOptions.ShowPaths && nodes.Length == 2)
        {
            Handles.color = CustomColors.darkOrange;
            Handles.DrawLine(nodes[0].WorldDebugPos, nodes[1].WorldDebugPos, 2f);
        }

        for (int i = 0; i < nodes.Length; i++)
            nodes[i].ShowDebugInfo();
    }
    protected abstract int NumberOfNodes { get; }

    private void Reset()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        InitializeNodes();
    }

    private void OnValidate()
    {
        if (nodes.Length != NumberOfNodes)
        {
            Debug.LogWarning("The number of nodes cannot be changed");
            InitializeNodes();
        }
    }

    private void InitializeNodes()
    {
        nodes = new Node[NumberOfNodes];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Node();
        }
    }
#endif
}
