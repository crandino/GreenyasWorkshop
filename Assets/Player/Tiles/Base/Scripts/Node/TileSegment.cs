#if UNITY_EDITOR
using Hexagon.Tile.Debug; 
#endif
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using static Hexalinks.Tile.Node;

namespace Hexalinks.Tile
{
    public class TileSegment : MonoBehaviour
    {
        [SerializeField]
        protected Node[] nodes = new Node[2];

        [SerializeField]
        private MeshRenderer meshRenderer;

        //[SerializeField]
        //private int emissionPathIndex;

        //private static int pathEmissionID = Shader.PropertyToID("_EmissionPathSelector");

        public Gate[] Gates { private set; get; }

        public Node GoThrough(Node entry)
        {
            Assert.IsTrue(nodes.Contains(entry) && !IsStarter, $"Node {entry} doesn't belong to this path or is a Starter");
            return entry == nodes[0] ? nodes[1] : nodes[0];
        }

        public void Initialize()
        {
            Gates = new Gate[nodes.Length];

            for (int i = 0; i < nodes.Length; i++)
                Gates[i] = new Gate(this, nodes[i]);
        }

        public bool IsStarter => nodes.Length == 1;

        //public List<Gate> Gates
        //{
        //    get
        //    {
        //        List<Gate> gates = new List<Gate>();

        //        for (int i = 0; i < nodes.Length; i++)
        //            gates.Add(new Gate(this, nodes[i]));

        //        return gates;
        //    }
        //}

        //public void ConnectSegment(Tile.Data tileData)
        //{
        //    for (int i = 0; i < nodes.Length; i++)
        //    {
        //        CubeCoord neighborHexCoord = tileData.Coord + CubeCoord.GetToNeighborCoord(nodes[i].Side);

        //        if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile.Data neighborTileData))
        //        {
        //            Gate[] neighborGates = neighborTileData.Gates;

        //            for (int j = 0; j < neighborGates.Length; ++j)
        //            {
        //                if (nodes[i].Side.IsOpposite(neighborGates[j].Node.Side))
        //                {
        //                    nodes[i].Connections.Add(neighborGates[j]);
        //                    neighborGates[j].Node.Connections.Add(new Gate(this, nodes[i]));
        //                }
        //            }
        //        }
        //    }
        //}

        //public void DisconnectSegment()
        //{
        //    for (int i = 0; i < nodes.Length; i++)
        //    {
        //        for (int j = 0; j < nodes[i].Connections.Count; j++)
        //        {
        //            Gate.Pool.Release(nodes[i].Connections[j].Node.Connections);
        //            nodes[i].Connections[j].Node.Connections.Clear();
        //        }

        //        Gate.Pool.Release(nodes[i].Connections);
        //        nodes[i].Connections.Clear();
        //    }
        //}

        public void Highlight()
        {
            //Vector4 values = meshRenderer.material.GetVector(pathEmissionID);
            //values[emissionPathIndex] = 1f;
            //meshRenderer.material.SetVector(pathEmissionID, values);
        }

        public void Unhighlight()
        {
            //Vector4 values = meshRenderer.material.GetVector(pathEmissionID);
            //values[emissionPathIndex] = 0f;
            //meshRenderer.material.SetVector(pathEmissionID, values);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (TileDebugOptions.Instance.showSegments /*&& nodes.Length == 2*/)
            {
                Handles.color = CustomColors.darkOrange;
                Handles.DrawLine(nodes[0].WorldDebugPos, nodes[1].WorldDebugPos, 2f);
            }

            for (int i = 0; i < nodes.Length; i++)
                nodes[i].OnDrawGizmos();
        }

        //protected abstract int NumberOfNodes { get; }

        private void Reset()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            InitializeNodes();
        }

        //private void OnValidate()
        //{
        //    if (nodes.Length != NumberOfNodes)
        //    {
        //        Debug.LogWarning("The number of nodes cannot be changed");
        //        InitializeNodes();
        //    }
        //}

        private void InitializeNodes()
        {
            nodes = new Node[2];
        }
#endif
    }

}