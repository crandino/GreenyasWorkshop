using HexaLinks.Tile;
using static Game;

public class TileManager : GameSystemMonobehaviour
{
    //private Tile[] tiles;

    //[SerializeField]
    //private Tile[] excludeTiles;

    //[SerializeField]
    //private DeckContent deckCreator;

    public override void InitSystem()
    {
        HexMap.Instance.ConnectAll();
    }
}
