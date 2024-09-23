using Greenyas.Hexagon;

public class EditorTileRotation : TileRotation
{
    public EditorTileRotation(TileCoordinates coordinates) : base(coordinates)
    { }

    public void RotateClockwise()
    {
        Coordinates.RotationAngle += HexTools.ROTATION_ANGLE;
    }

    public void RotateCounterClockwise()
    {
        Coordinates.RotationAngle -= HexTools.ROTATION_ANGLE;
    }

}
