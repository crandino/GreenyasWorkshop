using System.Linq;
using UnityEngine;
using Hexalinks.Tile;

public class TileManager : MonoBehaviour
{
    private Tile[] tiles;

    [SerializeField]
    private Tile[] excludeTiles;

    [SerializeField]
    private DeckContent deckCreator;

    private void Start()
    {
        HexMap.Instance.ConnectAll();
    }
}
