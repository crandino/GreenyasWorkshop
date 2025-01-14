using HexaLinks.Tile;
using UnityEngine;

#if RECORDING
public class AddTileRecord : Record<HexMap>
{
    private readonly Tile tile = null;

    public AddTileRecord(HexMap hexMap, Tile tileAdded) : base(hexMap)
    {
        tile = tileAdded;
    }

    public override void Redo()
    {
        tile.Initialize();
        tile.gameObject.SetActive(true);
        actor.AddTile(tile);
        tile.Connect();
    }

    public override void Undo()
    {
        tile.Terminate();
        tile.gameObject.SetActive(false);
        actor.RemoveTile(tile.Coord);
        tile.Disconnect();
    }

    public override void OnRemove()
    {
        if (tile != null)
            GameObject.Destroy(tile.gameObject);
    }
} 
#endif

