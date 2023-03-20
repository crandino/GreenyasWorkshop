using UnityEngine;

public static class TilePlaceHelper
{
    private const float DEFAULT_HEXAGON_LENGTH = 0.5f;

    public static float HexagonDiagonal { private set; get; }
    public static float HexagonCircumradius { private set; get; }
    public static float HexagonHeigth { private set; get; }
    public static float HexagonInradius { private set; get; }

    private static float HorizontalGap
    { set; get; }
    private static float GetVerticalGap(int horizontalIndex)
    {
        int multiplier = (horizontalIndex % 2 == 0) ? 2 : 1;
        return HexagonInradius * multiplier;
    }

    static TilePlaceHelper()
    {
        SetHexagonLength(DEFAULT_HEXAGON_LENGTH);
    }

    public static void SetHexagonLength(float length)
    {
        HexagonCircumradius = length;
        HexagonDiagonal = HexagonCircumradius * 2f;
        HexagonHeigth = Mathf.Sqrt(3) * 0.5f * HexagonDiagonal;
        HexagonInradius = HexagonHeigth * 0.5f;

        HorizontalGap = HexagonCircumradius * 1.5f;
    }   
    
    //public static Vector3 GetNearestPossiblePoint(Vector3 worldPos)
    //{
    //    int horizontalIndex = Mathf.RoundToInt(worldPos.x / (float)HorizontalGap);
    //    int verticalIndex = Mathf.RoundToInt(worldPos.z / (float)GetVerticalGap(horizontalIndex));

    //    return new Vector3()
    //    {
    //        x = horizontalIndex * HorizontalGap,
    //        y = 0f,
    //        z = verticalIndex * GetVerticalGap(horizontalIndex)
    //    };
    //}
}
