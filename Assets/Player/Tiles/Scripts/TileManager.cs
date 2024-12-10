using HexaLinks.Tile;
using static Game;

public class TileManager : GameSystemMonobehaviour
{
    public override void InitSystem()
    {
        Game.Instance.GetSystem<HexMap>().ConnectAll();
    }
}
