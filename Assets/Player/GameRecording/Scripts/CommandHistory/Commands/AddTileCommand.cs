using HexaLinks.Tile;

public class AddTileCommand : CommandRecord<HexMap>
{
    private readonly Tile tile = null;

    public AddTileCommand(HexMap hexMap, Tile tileAdded) : base(hexMap)
    {
        tile = tileAdded;
    }

    public override void Redo()
    {
        tile.gameObject.SetActive(true);
        actor.AddTile(tile);
        tile.Connect();
    }

    public override void Undo()
    {
        tile.gameObject.SetActive(false);
        actor.RemoveTile(tile.Coordinates.Coord);
        tile.Disconnect();
    }
}

