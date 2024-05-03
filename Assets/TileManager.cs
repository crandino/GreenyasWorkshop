using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Tile[] tiles;

    [SerializeField]
    private Tile[] excludeTiles;

    [SerializeField]
    private DeckCreator deckCreator;

    private void Start()
    {
        tiles = FindObjectsByType<Tile>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Except(excludeTiles).ToArray();

        for (int i = 0; i < tiles.Length; i++)
            tiles[i].Initialize();

        for (int i = 0; i < tiles.Length; i++)
            tiles[i].ConnectTile(false);
    }
}
