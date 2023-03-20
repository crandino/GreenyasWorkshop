using Greenyass.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileSelector : MonoBehaviour
{
    [SerializeField]
    private LayerMask tileMask, boardMask;

    private Tile currentSelectedTile = null;
    private InputManager input = null;

    private Vector3 offset = Vector3.zero;

    private void Start()
    {
        input = Game.Instance.GetSystem<InputManager>();
        input.OnSelect.OnButtonPressed += PickUpTile;
    }

    private void PickUpTile()
    {
        if (CursorRaycastToTile(tileMask, out currentSelectedTile))
        {
            currentSelectedTile.PickUp();

            input.OnSelect.OnButtonPressed -= PickUpTile;
            input.OnSelect.OnButtonPressed += ReleaseTile;
        }       
    }

    private void ReleaseTile()
    {
        if (!CursorRaycastToTile(tileMask, out Tile _))
        {
            currentSelectedTile.Release();
            currentSelectedTile = null;

            input.OnSelect.OnButtonPressed -= ReleaseTile;
            input.OnSelect.OnButtonPressed += PickUpTile;
        }
    }

    private void Update()
    {
        if(currentSelectedTile)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit = new RaycastHit();
            if (CursorRaycast(boardMask, out hit))
            {
                currentSelectedTile.UpdatePosition(hit.point + offset);
            }
        }
    }

    private RaycastHit hit = new RaycastHit();

    private bool CursorRaycastToTile(LayerMask mask, out Tile component)
    {
        if (CursorRaycast(mask, out hit))
        {
            component = hit.collider.GetComponent<Tile>();
            offset = (component.transform.position - hit.point);
            offset.y = 0.25f;
            return true;
        }

        component = null;
        return false;
    }

    private bool CursorRaycast(LayerMask mask, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        return Physics.Raycast(ray, out hit, float.MaxValue, mask);
    }
}
