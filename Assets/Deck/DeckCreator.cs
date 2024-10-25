using HexaLinks.Tile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Hexagon/DeckContent")]
public class DeckContent : ScriptableObject
{
    [System.Serializable]
    private struct TileEntry
    {
        public Tile.Type tileType;
        public int amount;
    }

    [SerializeField]
    private TileEntry[] content;

    public Deck CreateDeck()
    {
        Deck newDeck = new Deck(this);
        newDeck.Shuffle();
        return newDeck;
    }

    public class Deck
    {
        private readonly Tile.Type[] deck;
        private int discardIndex = 0;

        public bool Empty => discardIndex >= deck.Length;

        public Deck(DeckContent creator)
        {
            List<Tile.Type> newDeck = new List<Tile.Type>();

            for (int i = 0; i < creator.content.Length; i++)
                newDeck.AddRange(Enumerable.Repeat(creator.content[i].tileType, creator.content[i].amount));

            deck = newDeck.ToArray();
        }

        public void Shuffle()
        {
            discardIndex = 0;
            deck.Shuffle();
        }

        public Tile.Type Draw()
        {
            return deck[discardIndex++];
        }
    }
}
