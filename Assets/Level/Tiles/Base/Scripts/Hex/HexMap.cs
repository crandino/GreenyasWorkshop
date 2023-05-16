using UnityEngine;
using System.Collections.Generic;
using Greenyas.Hexagon;
using System.Linq;
#if UNITY_EDITOR
using UnityEngine.Assertions;
using UnityEditor;
#endif

public class HexMap : MonoBehaviour
{
    [SerializeField]
    private Color lineColor = Color.black;

    [SerializeField]
    private float size = 0.5f;

    /* 
     *  More info: https://www.redblobgames.com/grids/hexagons/
     *  Orientation flat
     */

    private static HexMap instance = null;

    public static HexMap Instance
    {
        private set
        {
            instance = value;
        }

        get
        {
            return instance;
        }
    }

    private readonly Dictionary<CubeCoord, Tile> mapStorage = new Dictionary<CubeCoord, Tile>(new CubeCoord.CoordinateComparer());

    private void Awake()
    {
        Instance = this;
    }

    public void AddTile(Tile tile)
    {
        mapStorage.Add(tile.HexCoord, tile);
    }

    public void RemoveTile(CubeCoord coord)
    {
        mapStorage.Remove(coord);
    }

    public bool IsTileOn(CubeCoord coord)
    {
        return mapStorage.ContainsKey(coord);
    }

    public bool TryGetTile(CubeCoord coord, out Tile tile)
    {
        return mapStorage.TryGetValue(coord, out tile);
    }

    public Tile[] GetAllStarterTiles()
    {
        return mapStorage.Select(t => t.Value).
                          Where(t => t as StarterTile).ToArray();
    }

    #region VISUAL_DEBUG
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        for (int r = -5; r < 5; r++)
        {
            for (int q = -5; q < 5; q++)
            {
                CubeCoord hexCoord = new CubeCoord(q, r);
                Vector3 hexCenterPos = HexTools.GetCartesianWorldPos(hexCoord);
                DrawHexagon(hexCenterPos);
                if (DebugOptions.ShowHexagonCoord)
                    DrawCubeCoordinates(hexCenterPos, hexCoord);
            }
        }
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

    private void DrawHexagon(Vector3 centerPosition)
    {
        const int MAX_NUM_CORNERS = 6;
        for (int cornerIndex = 0; cornerIndex <= MAX_NUM_CORNERS; cornerIndex++)
        {
            Vector3 vertexA = GetHexWorldCorner(centerPosition, size, cornerIndex);
            Vector3 vertexB = GetHexWorldCorner(centerPosition, size, (cornerIndex + 1) % MAX_NUM_CORNERS);

            Gizmos.DrawLine(vertexA, vertexB);
        }
    }

    private Vector3 GetHexWorldCorner(Vector3 hexCenter, float size, int cornerIndex)
    {
        Assert.IsTrue(cornerIndex >= 0 && cornerIndex <= 6, $"Invalid index {cornerIndex} for hexagon corner");
        float angle = Mathf.Deg2Rad * 60 * cornerIndex;
        return new Vector3(hexCenter.x + size * Mathf.Cos(angle), hexCenter.y, hexCenter.z + size * Mathf.Sin(angle));
    }

#endif 
    #endregion

}
