using Hexalinks.Tile;
using UnityEngine;

public class TilePosition : TileModifier
{
    // Position
    private Vector3 verticalOffset = Vector3.up * 0.25f;

    public TilePosition(Tile tile) : base(tile)
    {
        Current.SetOnGrid();
    }

    public void AllowMovement()
    {
        Start();
    }

    public void RestrictMovement()
    {
        Finish();
        Current.SetOnGrid();
    }

    protected override void OnUpdate()
    {
        if (TileRaycast.CursorRaycastToBoard(out Vector3 boardCursorPos))
            Current.transform.position = boardCursorPos + verticalOffset;
    }
}
