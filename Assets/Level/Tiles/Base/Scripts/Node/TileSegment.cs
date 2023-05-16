using Greenyas.Hexagon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public abstract partial class TileSegment : MonoBehaviour
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

    public void GetAllGates(List<Gate> gates)
    {
        for (int i = 0; i < nodes.Length; i++)
            gates.Add(Gate.Pool.Generate(this, nodes[i]));
    }

    public void GetAllNodes(List<Node> nodes)
    {
        nodes.AddRange(nodes);
    }

    public void SearchGatesAgainst(HexSide.Side side, List<Gate> gates)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (side.IsOpposite(nodes[i].Side))
                gates.Add(Gate.Pool.Generate(this, nodes[i]));
        }
    }

    public void ConnectSegment(CubeCoord tileCoord, bool bidirectional)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            CubeCoord toNeighborHexCoord = tileCoord + CubeCoord.GetToNeighborCoord(nodes[i].Side);

            if (HexMap.Instance.TryGetTile(toNeighborHexCoord, out Tile contactTile) &&
                contactTile.SearchGatesAgainst(nodes[i].Side, out List<Gate> gates))
            {
                nodes[i].Connections.AddRange(gates);

                if (bidirectional)
                {
                    for (int j = 0; j < gates.Count; j++)
                        gates[j].Node.Connections.Add(Gate.Pool.Generate(this, nodes[i]));
                }
            }
        }
    }

    public void DisconnectSegment()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int j = 0; j < nodes[i].Connections.Count; j++)
            {
                Gate.Pool.Release(nodes[i].Connections[j].Node.Connections);
                nodes[i].Connections[j].Node.Connections.Clear();
            }

            Gate.Pool.Release(nodes[i].Connections);
            nodes[i].Connections.Clear();
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
    }
#endif
}
