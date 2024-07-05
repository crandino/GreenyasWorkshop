using Greenyas.Hexagon;
using Hexalinks.Tile;
using UnityEngine;

public class TilePosition : TileModifier
{
    private Vector3 verticalOffset = Vector3.up * 0.25f;
    private readonly static Vector3 verticalGridOffset = Vector3.zero;
    private readonly static Vector3 verticalHoverGridOffset = Vector3.up * 0.25f;

    public enum PositionMode
    {
        GRID,
        HOVER
    }

    public PositionMode Mode
    {
        set
        {
            switch(value)
            {
                case(PositionMode.GRID):
                    verticalOffset = verticalGridOffset; break;
                case(PositionMode.HOVER):
                    verticalOffset = verticalHoverGridOffset; break;
            }
        }
    }

    private Vector3 TilePos
    {
        get
        {
            return HexTools.GetGridCartesianWorldPos(Coord) + verticalOffset;
        }
    }

    public CubeCoord Coord { private set; get; }

    public TilePosition(Tile tile, PositionMode initialMode = PositionMode.GRID) : base(tile)
    {
        Coord = HexTools.GetNearestCubeCoord(Current.transform.position);
        Mode = initialMode;
        Current.transform.position = TilePos;
    }

    public void AllowMovement()
    {
        Start();
        Mode = PositionMode.HOVER;
    }

    public void RestrictMovement()
    {
        Finish();
        AttachToGrid();
    }

    protected override bool OnUpdate()
    {
        if (TileRaycast.CursorRaycastToBoard(out Vector3 boardCursorPos))
            Current.transform.position = boardCursorPos + verticalOffset;

        return true;
    }

    public void AttachToGrid()
    {
        Mode = PositionMode.GRID;
        Coord = HexTools.GetNearestCubeCoord(Current.transform.position);
        Current.transform.position = TilePos;
    }

#if UNITY_EDITOR

    public void MoveUp()
    {
        Coord += CubeCoord.GetToNeighborCoord(HexSide.Side.North);
        Current.transform.position = TilePos;
    }

    public void MoveDown()
    {
        Coord += CubeCoord.GetToNeighborCoord(HexSide.Side.South);
        Current.transform.position = TilePos;
    }

    public void MoveRight()
    {
        Coord += CubeCoord.GetToNeighborCoord(HexSide.Side.NorthEast);
        Current.transform.position = TilePos;
    }

    public void MoveLeft()
    {
        Coord += CubeCoord.GetToNeighborCoord(HexSide.Side.NorthWest);
        Current.transform.position = TilePos;
    }

#endif
}
