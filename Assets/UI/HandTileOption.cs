using HexaLinks.Tile;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace HexaLinks.UI.PlayerHand
{
    public class HandTileOption
    {
        private readonly Button button;
        private readonly DeckContent.Deck.DrawableDeck deck;

        private TileResource tileResource = null;

        public bool DrawingPending { protected set; get; }

        public HandTileOption(Button tileButton, DeckContent.Deck.DrawableDeck drawableDeck)
        {
            button = tileButton;
            deck = drawableDeck;
        }

        public void Set(TileResource resource)
        {
            tileResource = resource;
            Reset();
        }

        public void Reset()
        {
            button.iconImage = Background.FromSprite(tileResource.Icon);
            DrawingPending = false;
        }

        public void ActivateSelection(Action<Tile.Tile> onTileSelection)
        {
            button.SetEnabled(true);
            button.clicked += () => SelectTile(onTileSelection);
        }

        protected virtual void SelectTile(Action<Tile.Tile> onTileSelection)
        {
            if (tileResource.Prefab != null)
            {
                button.iconImage = Background.FromSprite(Hand.EmptyTile.Icon);
                Tile.Tile tile = GameObject.Instantiate<Tile.Tile>(tileResource.Prefab);
                onTileSelection(tile);

                DrawingPending = true;
            }
        }

        public void DeactivateSelection()
        {
            button.SetEnabled(false);
            button.clickable = null;
        }

        public void Draw(TileResource fallback)
        {
            Set(deck.Draw(fallback));
            DrawingPending = false;
        }
    } 
}