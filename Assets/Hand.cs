using HexaLinks.Tile;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class Hand : MonoBehaviour
{
    private class HandOption
    {
        private readonly Button button;

        private TileResource tileResource;
        private static TileResource emptyTileResource;

        public bool Selected { private set; get; }

        public HandOption(Button button, TileResource emptyTileResource)
        {
            this.button = button;
            HandOption.emptyTileResource = emptyTileResource;
            //Reset();
        }

        public void Set(TileResource resourceLocator)
        {
            tileResource = resourceLocator;
            Reset();
        }

        public void Reset()
        {
            button.iconImage = Background.FromSprite(tileResource.Icon);
            Selected = false;
        }

        //private Tile LoadTile()
        //{
        //    button.iconImage = Background.FromSprite(emptyIcon);
        //    return GameObject.Instantiate<Tile>(tileResourceLocator.Prefab);
        //}

        public void ActivateSelection(Action<Tile> onTileSelection)
        {
            button.clicked += () =>
            {
                if (tileResource.Prefab != null)
                {
                    button.iconImage = Background.FromSprite(emptyTileResource.Icon);
                    Tile tile = GameObject.Instantiate<Tile>(tileResource.Prefab);
                    onTileSelection(tile);

                    Selected = true;
                }
            };
        }

        public void DeactivateSelection()
        {
            button.clickable = null;
        }
    }

    private HandOption[] playerOptions = null;
    private DeckContent.Deck deck;

    [SerializeField]
    UIDocument playerHandUI;

    [SerializeField]
    private DeckContent deckContent;

    public void Initialize()
    {
        deck = deckContent.CreateDeck();

        playerOptions = playerHandUI.rootVisualElement.Query<Button>().ForEach(b =>
        {
            HandOption handOption = new(b, deck.EmptyTile);
            handOption.Set(deck.Draw());
            return handOption;
        }).ToArray();
    }

    public void ActivateSelection(Action<Tile> onTileSelection)
    {
        foreach (HandOption option in playerOptions)
        {
            option.Reset();
            option.ActivateSelection(onTileSelection);
        }
    }

    public void DeactivateSelection()
    {
        foreach (HandOption option in playerOptions)
        {
            option.DeactivateSelection();
        }
    }

    public void Draw()
    {
        HandOption handOption = playerOptions.First(o => o.Selected);
        handOption.Set(deck.Draw());
    }
}
