using Greenyas.Input;
using Hexalinks.Tile;

public class TileSelector : Game.SubSystem
{
    private Tile currentSelectedTile = null;
    private InputManager input = null;

    protected override bool TryInitSystem()
    {
        input = Game.Instance.GetSystem<InputManager>();
        input.OnSelect.OnButtonPressed += PickUpTile;
        return true;
    }

    private void PickUpTile()
    {
        if (TileRaycast.CursorRaycastToTile(out currentSelectedTile))
        {
            input.OnSelect.OnButtonPressed -= PickUpTile;
            input.OnSelect.OnButtonPressed += ReleaseTile;

            currentSelectedTile.PickUp();
        }       
    }

    private void ReleaseTile()
    {
        if (!TileRaycast.CursorRaycastToTile())
        {
            input.OnSelect.OnButtonPressed -= ReleaseTile;
            input.OnSelect.OnButtonPressed += PickUpTile;

            currentSelectedTile.Release();
            currentSelectedTile = null;
        }
    }
}
