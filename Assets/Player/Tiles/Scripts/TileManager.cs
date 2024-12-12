using HexaLinks.Tile;
using static Game;

public class TileManager : GameSystemMonobehaviour
{
    public override void InitSystem()
    {
        foreach (var item in FindObjectsByType<Tile>(UnityEngine.FindObjectsSortMode.None))
        {
            item.Initialize();
        }

        Game.Instance.GetSystem<HexMap>().ConnectAll();
    }
}
