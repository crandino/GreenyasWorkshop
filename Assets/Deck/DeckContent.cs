using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Hexagon/DeckContent")]
public class DeckContent : ScriptableObject
{
    [System.Serializable]
    public struct TileEntry
    {
        public TileResource tileResource;
        public int amount;

        public TileEntry(TileResource resource, int defaultAmount = 5)
        {
            tileResource = resource;
            amount = defaultAmount;
        }
    }

    [SerializeField]
    private TileEntry[] traversalContent;

    [SerializeField]
    private TileEntry[] propagatorContent;

    public Deck CreateDeck()
    {
        return new Deck(this);
    }

#if UNITY_EDITOR
    [ContextMenu("Create content structure")]
    private void Create()
    {
        // Traversal tiles
        traversalContent = GetTileResources("t:TileResource, _T_").ToArray();

        // Propagator tiles
        propagatorContent = GetTileResources("t:TileResource, _P_").ToArray();
    }

    private TileEntry[] GetTileResources(string searchFilter)
    {
        string[] tileResourcesIDs = AssetDatabase.FindAssets(searchFilter);
        List<TileResource> listOfResources = new();
        foreach (var t in tileResourcesIDs)
        {
            var resource = AssetDatabase.LoadAssetAtPath<TileResource>(AssetDatabase.GUIDToAssetPath(t));
            if (resource != null)
                listOfResources.Add(AssetDatabase.LoadAssetAtPath<TileResource>(AssetDatabase.GUIDToAssetPath(t)));
        }

        return listOfResources.Select(r => new TileEntry(r)).ToArray();
    }
#endif

    public class Deck
    {
        public class DrawableDeck
        {
            private readonly TileResource[] deck;
            private int discardIndex = 0;
            public bool EmptyDeck => discardIndex == deck.Length;

            public DrawableDeck(TileEntry[] content)
            {
                List<TileResource> newDeck = new();

                for (int i = 0; i < content.Length; i++)
                    newDeck.AddRange(Enumerable.Repeat(content[i].tileResource, content[i].amount));

                deck = newDeck.ToArray();
                Shuffle();
            }

            private void Shuffle()
            {
                discardIndex = 0;
                deck.Shuffle();
            }

            public TileResource Draw(TileResource fallback)
            {
                return !EmptyDeck ? deck[discardIndex++] : fallback;
            }     

            public void FakeDraw() => discardIndex++;
            public void FakeUndraw() => discardIndex--;
        }

        public DrawableDeck TraversalDeck { private set; get; }
        public DrawableDeck PropagatorDeck { private set; get; }

        public Deck(DeckContent creator)
        {
            TraversalDeck = new DrawableDeck(creator.traversalContent);
            PropagatorDeck = new DrawableDeck(creator.propagatorContent);
        }
    }
}
