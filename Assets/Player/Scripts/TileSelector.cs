using Greenyass.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileSelector : MonoBehaviour
{
    [SerializeField]
    private LayerMask tileMask, boardMask;

    private Tile currentSelectedTile = null;

    private InputManager input;

    private void Start()
    {
        input = Game.Instance.GetSystem<InputManager>();
        input.OnSelect.OnButtonPressed += GetTile;
    }

    public void GetTile()
    {
        if (Tile.IsTileSelected)
        {
            currentSelectedTile.Release();
            currentSelectedTile = null;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                currentSelectedTile = hit.collider.GetComponent<Tile>();
                currentSelectedTile.PickUp();
            }            
        }
    }

    private void Update()
    {
        if(Tile.IsTileSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, float.MaxValue, boardMask))
            {
                currentSelectedTile.UpdatePosition(hit.point, 0.25f);
            }
        }
    }
}
