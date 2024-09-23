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

    public TilePosition(TileCoordinates coordinates, PositionMode positionMode = PositionMode.GRID) : base(coordinates)
    {
        SetPos(coordinates.Position);
    }

    public void AllowMovement()
    {
        Start();
        Mode = PositionMode.HOVER;
    }

    public void RestrictMovement()
    {
        AttachToGrid();
        Finish();
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
        SetPos(HexTools.GetGridCartesianWorldPos(Coordinates.Position));
    }

    public void SetPos(Vector3 position)
    {
        Vector3 finalPos = position + verticalOffset;
        Coordinates.Position = finalPos;
    }
}
