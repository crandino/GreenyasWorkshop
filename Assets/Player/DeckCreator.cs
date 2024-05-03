using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Hexagon/Deck")]
public class DeckCreator : ScriptableObject
{
    [System.Serializable]
    private struct TileEntry
    {
        public Tile.Type tileType;
        public int amount;
    }

    [SerializeField]
    private TileEntry[] deckContent;

    public Deck Create()
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

        public Deck(DeckCreator creator)
        {
            List<Tile.Type> newDeck = new List<Tile.Type>();

            for (int i = 0; i < creator.deckContent.Length; i++)
                newDeck.AddRange(Enumerable.Repeat(creator.deckContent[i].tileType, creator.deckContent[i].amount));

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
