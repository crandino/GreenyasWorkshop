using UnityEngine;
using System.Collections.Generic;
using Greenyas.Hexagon;
using System.Linq;
using System;
using static Game;

#if UNITY_EDITOR
using UnityEngine.Assertions;
using HexaLinks.Extensions.Vector;
using UnityEditor;
#endif

namespace HexaLinks.Tile
{
    public class HexMap : GameSystemMonobehaviour
    {
        /* 
         *  More info: https://www.redblobgames.com/grids/hexagons/
         *  Orientation flat
         */

        [SerializeField]
        private SerializableDictionary<CubeCoord, Tile> gridData = new(new CubeCoord.CoordinateComparer());

        public event Action OnGridChanged; // TODO: Use TileEvents

        public int NumOfTiles => gridData.Count;

        public override void InitSystem()
        {
            // Nothing!
        }

        public bool AddTile(Tile tile)
        {
            bool successfulAdded = gridData.TryAdd(tile.Coord, tile);

            if (successfulAdded
#if UNITY_EDITOR
                 && Application.isPlaying
#endif
               )
               OnGridChanged(); 
            return successfulAdded;
        }

        public void RemoveTile(CubeCoord coord)
        {
            gridData.Remove(coord);
        }

        public bool TryGetTile(CubeCoord coord, out Tile tileData)
        {
            return gridData.TryGetValue(coord, out tileData);
        }

        public void ConnectAll()
        {
            foreach(var tile in gridData.Values)
                tile.Connect();
        }

        [ContextMenu("Clear Grid")]
        public void ClearGrid()
        {
            gridData.Clear();
        }

        [ContextMenu("Add all tiles")]
        public void AddAllToGrid()
        {
            ClearGrid();
            Tile[] tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
            foreach (var tile in tiles)
            {
                AddTile(tile);
            }
        }

#if UNITY_EDITOR
        #region VISUAL_DEBUG

        [SerializeField]
        private int mapSize = 3;

        [SerializeField]
        private float lineWidth = 0.05f;

        [SerializeField]
        private MeshFilter meshFilter;

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        [ContextMenu("Get Hexagon Line Vertices")]
        private void GetLineVertices()
        {
            vertices.Clear();
            indices.Clear();

            for (int q = -mapSize; q <= mapSize; ++q)
            {
                for (int r = Mathf.Max(-mapSize, -q - mapSize); r <= Mathf.Min(mapSize, -q + mapSize); ++r)
                {
                    Vector3 hexCenter = HexTools.GetGridCartesianWorldPos(new CubeCoord(q, r));
                    FillHexagonMeshData(hexCenter);

                    //DrawHexagon(hexCenterPos);

                    //if (DebugOptions.ShowHexagonCoord)
                    //    DrawCubeCoordinates(hexCenterPos, hexCoord);
                }
            }

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetNormals(Enumerable.Repeat(Vector3.up, vertices.Count).ToArray());
            mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
            meshFilter.sharedMesh = mesh;
        }

        private void FillHexagonMeshData(Vector3 hexCenter)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3 a = GetHexWorldCorner(hexCenter, i);
                Vector3 b = GetHexWorldCorner(hexCenter, i + 1);

                Vector3 centerToA = (a - hexCenter).normalized;
                Vector3 centerToB = (b - hexCenter).normalized;

                Vector3 upA = a + 0.5f * lineWidth * centerToA;
                Vector3 downA = a - 0.5f * lineWidth * centerToA;
                Vector3 upB = b + 0.5f * lineWidth * centerToB;
                Vector3 downB = b - 0.5f * lineWidth * centerToB;

                TryAddVertex(upA);
                TryAddVertex(downA);
                TryAddVertex(downB);
                TryAddVertex(upB);

                AddTriangle(upA, downB, upB);
                AddTriangle(upA, downA, downB);
            }
        }

        private void TryAddVertex(Vector3 vertex)
        {
            if (!FindVector(vertex, out _))
                vertices.Add(vertex);
        }

        private void AddTriangle(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC)
        {
            FindVector(vertexA, out int indexA);
            FindVector(vertexB, out int indexB);
            FindVector(vertexC, out int indexC);

            indices.AddRange(new int[] { indexA, indexB, indexC });
        }

        private bool FindVector(Vector3 match, out int index)
        {
            index = vertices.FindIndex(v => v.Approximately(match));
            return index != -1;
        }

        private void DrawCubeCoordinates(Vector3 centerPosition, CubeCoord hexCoord)
        {
            const float offset = 0.25f;
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontSize = 18;

            // Q
            textStyle.normal.textColor = Color.green;
            Handles.Label(centerPosition + (Vector3.forward + Vector3.left).normalized * offset, $"{hexCoord.Q}", textStyle);

            // R
            textStyle.normal.textColor = Color.blue;
            Handles.Label(centerPosition + (Vector3.right).normalized * offset, $"{hexCoord.R}", textStyle);

            // S
            textStyle.normal.textColor = Color.red;
            Handles.Label(centerPosition + (Vector3.back + Vector3.left).normalized * offset, $"{hexCoord.S}", textStyle);
        }

        private Vector3 GetHexWorldCorner(Vector3 hexCenter, int cornerIndex)
        {
            Assert.IsTrue(cornerIndex >= 0 && cornerIndex <= 6, $"Invalid index {cornerIndex} for hexagon corner");
            float angle = Mathf.Deg2Rad * 60 * cornerIndex;
            return new Vector3(hexCenter.x + HexTools.hexagonSize * Mathf.Cos(angle), hexCenter.y, hexCenter.z + HexTools.hexagonSize * Mathf.Sin(angle));
        }

        #endregion
#endif
    }
}