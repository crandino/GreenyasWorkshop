using Greenyas.Hexagon;
using UnityEngine;

[System.Serializable]
public class TileCoordinates 
{
    public CubeCoord Coord { private set; get; } = CubeCoord.Origin;

    public Vector3 Position
    {
        set
        {
            tileTransform.transform.position = value;
            Coord = CubeCoord.GetNearestCubeCoord(value);
        }
        get
        {
            return tileTransform.transform.position;
        }
    }

    public float RotationAngle
    {
        get
        {
            return tileTransform.localEulerAngles.y;
        }

        set
        {
            tileTransform.localEulerAngles = new Vector3(tileTransform.localEulerAngles.x, value, tileTransform.localEulerAngles.z);
        }
    }

    [SerializeField]
    private Transform tileTransform = null;

    public TileCoordinates(Transform tileTransform)
    {
        this.tileTransform = tileTransform;
        Position = tileTransform.position;
    }

    public TileCoordinates() { }

    public void HolaTile() => Debug.Log("Hola Tile");
}
