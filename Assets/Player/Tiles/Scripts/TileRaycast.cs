using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public static class TileRaycast
{
    private static RaycastHit hit = new RaycastHit();

    private readonly static LayerMask tileMask = LayerMask.GetMask("Tile");
    private readonly static LayerMask boardMask = LayerMask.GetMask("Board");

    private static bool CursorRaycast(LayerMask mask, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        return Physics.Raycast(ray, out hit, Camera.main.farClipPlane, mask);
    }

    public static bool CursorRaycastToTile()
    {
        return CursorRaycast(tileMask, out hit);
    }


    public static bool CursorRaycastToTile<T>(out T component) where T : UnityEngine.Component
    {
        component = default;
        if (CursorRaycast(tileMask, out hit))
            component = hit.collider.GetComponent<T>();

        return component != null;
    }

    public static bool CursorRaycastToBoard(out Vector3 boardPos)
    {
        boardPos = Vector3.zero;

        if (CursorRaycast(boardMask, out hit))
            boardPos = hit.point;

        return boardPos != Vector3.zero;
    }    
}
