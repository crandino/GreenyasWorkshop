using Hexalinks.Tile;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static TileResources;

public class Hand : MonoBehaviour
{
    private class HandOption
    {
        private readonly Button button;
        private TileResourceLocator tileResourceLocator;

        public bool Empty => button.iconImage == Background.FromSprite(emptyIcon);

        public HandOption(Button button)
        {
            this.button = button;
        }

        public void Set(TileResourceLocator locator)
        {
            tileResourceLocator = locator;
            button.iconImage = Background.FromSprite(locator.sprite);
        }

        public void Reset()
        {
            if(Empty)
                button.iconImage = Background.FromSprite(tileResourceLocator.sprite);
        }

        private Tile LoadTile()
        {
            button.iconImage = Background.FromSprite(emptyIcon);
            return GameObject.Instantiate<Tile>(tileResourceLocator.tile);
        }

        public void ActivateSelection(Action<Tile> onTileSelection)
        {
            button.clicked += () =>
            {
                Tile tile = LoadTile();
                onTileSelection(tile);
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

    [SerializeField]
    private TileResources resources;

    public static Sprite emptyIcon;

    private void Start()
    {
        emptyIcon = resources.EmptyIcon;
    }

    public void Initialize()
    {
        deck = deckContent.CreateDeck();
        playerOptions = playerHandUI.rootVisualElement.Query<Button>().ForEach(b =>
        {
            HandOption handOption = new(b);
            handOption.Set(resources[deck.Draw()]);
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
        if (deck.Empty)
            return;
        
        HandOption handOption = playerOptions.First(o => o.Empty);
        handOption.Set(resources[deck.Draw()]);
    }
}
