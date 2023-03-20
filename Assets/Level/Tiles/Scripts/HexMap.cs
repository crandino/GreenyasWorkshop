using UnityEngine;
using Greenyas.VectorExtensions.Swizzle;
#if UNITY_EDITOR
using UnityEngine.Assertions;
using UnityEditor;
#endif

public class HexMap : MonoBehaviour
{
    [SerializeField]
    private float planeRestrictionHeight = 0.1f;

    [SerializeField]
    private Color lineColor = Color.black;

    [SerializeField]
    private int boardWitdh = 5;
    [SerializeField]
    private int boardHeight = 5;

    [SerializeField]
    private float size = 0.5f;

    /* 
     *  More info: https://www.redblobgames.com/grids/hexagons/
     *  Orientation flat
     */ 

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        for (int r = 0; r < boardHeight; r++)
        {
            for (int q = 0; q < boardWitdh; q++)
            {
                Hex hex = new Hex(q, r);
                Vector2 hexCenterPos = hex.Get2DCartesianWorldPos();
                DrawHexagon(hexCenterPos);
            }
        }        
    } 

    private void DrawHexagon(Vector2 centerPosition)
    {
        const int MAX_NUM_CORNERS = 6;
        for (int cornerIndex = 0; cornerIndex <= MAX_NUM_CORNERS; cornerIndex++)
        {
            Vector2 vertexA = GetHexWorldCorner(centerPosition, size, cornerIndex);
            Vector2 vertexB = GetHexWorldCorner(centerPosition, size, (cornerIndex + 1) % MAX_NUM_CORNERS);

            Vector3 worldVertexA = vertexA.SwizzleX_Y(planeRestrictionHeight);
            Vector3 worldVertexB = vertexB.SwizzleX_Y(planeRestrictionHeight);

            Gizmos.DrawLine(worldVertexA, worldVertexB);
        }
    }

    private Vector3 GetHexWorldCorner(Vector2 center, float size, int cornerIndex)
    {
        Assert.IsTrue(cornerIndex >= 0 && cornerIndex <= 6, $"Invalid index {cornerIndex} for hexagon corner");
        float angle = Mathf.Deg2Rad * 60 * cornerIndex;
        return new Vector3(center.x + size * Mathf.Cos(angle), center.y + size * Mathf.Sin(angle));
    }

#endif

}
