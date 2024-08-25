using Greenyas.Hexagon;
using Hexalinks.Tile;
using UnityEngine;

public class TilePosition : TileModifier
{
    private Vector3 verticalOffset = Vector3.up * 0.25f;
    private readonly static Vector3 verticalGridOffset = Vector3.zero;
    private readonly static Vector3 verticalHoverGridOffset = Vector3.up * 0.25f;

    public CubeCoord Coord { get; private set; }

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

    public TilePosition(Tile tile, PositionMode positionMode = PositionMode.GRID) : base(tile)
    {
        Mode = positionMode;
        SetPos(tile.transform.position);
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
            SetPos(boardCursorPos);

        return true;
    }

    public void AttachToGrid()
    {
        Mode = PositionMode.GRID;
        SetPos(HexTools.GetGridCartesianWorldPos(Coord));
    }

    public void SetPos(Vector3 position)
    {
        Vector3 finalPos = position + verticalOffset;
        Current.transform.position = finalPos;
        Coord = CubeCoord.GetNearestCubeCoord(finalPos);
    }

#if UNITY_EDITOR

    public void MoveUp()
    {
        SetPos(HexTools.GetGridCartesianWorldPos(Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.North)));
    }

    public void MoveDown()
    {
        SetPos(HexTools.GetGridCartesianWorldPos(Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.South)));
    }

    public void MoveRight()
    {
        SetPos(HexTools.GetGridCartesianWorldPos(Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.NorthEast)));
    }

    public void MoveLeft()
    {
        SetPos(HexTools.GetGridCartesianWorldPos(Coord + CubeCoord.GetToNeighborCoord(HexSide.Side.NorthWest)));
    }

#endif
}
