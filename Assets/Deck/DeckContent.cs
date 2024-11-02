using HexaLinks.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Hexagon/DeckContent")]
public class DeckContent : ScriptableObject
{
    [System.Serializable]
    private struct TileEntry
    {
        public TileResource tileResource;
        public int amount;
    }

    [SerializeField]
    private TileEntry[] content;

    [SerializeField]
    private TileResource fallback;

    public Deck CreateDeck()
    {
        Deck newDeck = new Deck(this);
        newDeck.Shuffle();
        return newDeck;
    }

#if UNTIY_EDITOR
    [ContextMenu("Create content structure")]
    private void Create()
    {
        string[] tileResourcesIDs = AssetDatabase.FindAssets("t:TileResource");
        List<TileResource> listOfResources = new();
        foreach (var t in tileResourcesIDs)
        {
            var resource = AssetDatabase.LoadAssetAtPath<TileResource>(AssetDatabase.GUIDToAssetPath(t));
            if (resource != null)
                listOfResources.Add(AssetDatabase.LoadAssetAtPath<TileResource>(AssetDatabase.GUIDToAssetPath(t)));
        }

        content = listOfResources.Select(r => new TileEntry()
        {
            tileResource = r,
            amount = 0
        }).ToArray();
    } 
#endif

    public class Deck
    {
        private readonly TileResource[] deck;
        private int discardIndex = 0;
        public TileResource EmptyTile { private set; get; }

        public bool EmptyDeck => discardIndex >= deck.Length;

        public Deck(DeckContent creator)
        {
            EmptyTile = creator.fallback;

            List<TileResource> newDeck = new();

            for (int i = 0; i < creator.content.Length; i++)
                newDeck.AddRange(Enumerable.Repeat(creator.content[i].tileResource, creator.content[i].amount));

            deck = newDeck.ToArray();
        }

        public void Shuffle()
        {
            discardIndex = 0;
            deck.Shuffle();
        }

        public TileResource Draw()
        {
            return EmptyDeck ? EmptyTile : deck[discardIndex++];
        }
    }
}
