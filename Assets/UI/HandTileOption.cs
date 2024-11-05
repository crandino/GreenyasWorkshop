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
            button.clicked += LoadTile;
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

        public virtual void Activate()
        {
            button.SetEnabled(true);
        }

        public virtual void Deactivate()
        {
            button.SetEnabled(false);
        }

        public void Draw(TileResource fallback)
        {
            Set(deck.Draw(fallback));
            DrawingPending = false;
        }

        private void LoadTile()
        {
            if (tileResource.Prefab != null)
            {
                PrepareTile(GameObject.Instantiate<Tile.Tile>(tileResource.Prefab));
                button.iconImage = Background.FromSprite(Hand.EmptyTile.Icon);
                Deactivate();

                DrawingPending = true;
            }
        } 
        
        protected virtual void PrepareTile(Tile.Tile tile)
        {
            Game.Instance.GetSystem<TilePlacement>().Start(tile);
        }
    } 
}