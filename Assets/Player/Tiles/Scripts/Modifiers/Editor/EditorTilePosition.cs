using Greenyas.Hexagon;
using Hexalinks.Tile;

public class EditorTilePosition : TilePosition
{
    public EditorTilePosition(TileCoordinates coordinates, PositionMode positionMode = PositionMode.GRID) : base(coordinates, positionMode)
    { }

    public void MoveUp()
    {
        SetPos(HexTools.GetGridCartesianWorldPos(Coordinates.Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.North)));
    }

    public void MoveDown()
    {
        SetPos(HexTools.GetGridCartesianWorldPos(Coordinates.Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.South)));
    }

    public void MoveRight()
    {
        SetPos(HexTools.GetGridCartesianWorldPos(Coordinates.Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.NorthEast)));
    }

    public void MoveLeft()
    {
        SetPos(HexTools.GetGridCartesianWorldPos(Coordinates.Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.NorthWest)));
    }
}
