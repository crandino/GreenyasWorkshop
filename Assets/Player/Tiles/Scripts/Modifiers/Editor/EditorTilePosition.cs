using Greenyas.Hexagon;

public class EditorTilePosition : TilePosition
{

    public bool Editable
    {
        set
        {
            if (value)
                Mode = PositionMode.HOVER;
            else
                Mode = PositionMode.GRID;
        }
        get
        {
            return Mode == PositionMode.HOVER;
        }
    }

    public EditorTilePosition(TileCoordinates coordinates, PositionMode positionMode = PositionMode.GRID) : base(coordinates, positionMode)
    {
    }

    public void MoveUp()
    {
        if(Editable)
            SetPos(HexTools.GetGridCartesianWorldPos(Coordinates.Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.North)));
    }

    public void MoveDown()
    {
        if (Editable)
            SetPos(HexTools.GetGridCartesianWorldPos(Coordinates.Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.South)));
    }

    public void MoveRight()
    {
        if (Editable)
            SetPos(HexTools.GetGridCartesianWorldPos(Coordinates.Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.NorthEast)));
    }

    public void MoveLeft()
    {
        if (Editable)
            SetPos(HexTools.GetGridCartesianWorldPos(Coordinates.Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.NorthWest)));
    }
}
