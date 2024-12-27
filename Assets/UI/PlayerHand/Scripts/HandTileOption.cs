using UnityEngine;
using UnityEngine.UIElements;

namespace HexaLinks.UI.PlayerHand
{
    public class HandTileOption
    {
        private readonly Button button;
        private readonly DeckContent.Deck.DrawableDeck deck;

        protected readonly TilePlacement tilePlacement = null;
        private TileResource tileResource = null;

        public bool DrawingPending { protected set; get; } = true;
        protected bool Active => button.enabledSelf;

        public HandTileOption(Button tileButton, DeckContent.Deck.DrawableDeck drawableDeck)
        {
            button = tileButton;
            button.clicked += LoadTile;
            deck = drawableDeck;

            tilePlacement = Game.Instance.GetSystem<TilePlacement>();
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

            UnregisterCallbacks();
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
            if(DrawingPending)
                Set(deck.Draw(fallback));
            DrawingPending = false;
        }

        private void LoadTile()
        {
            if (tileResource.Prefab != null)
            {
                PrepareTile(GameObject.Instantiate<Tile.Tile>(tileResource.Prefab));
                button.iconImage = Background.FromSprite(HandUI.EmptyTile.Icon);
                
                Deactivate();

                DrawingPending = true;
            }
        } 

        private void RegisterCallbacks()
        {
            TilePlacement.Events.OnSuccessPlacement.Register(OnTilePlaced);

            TilePlacement.Events.OnFailurePlacement.Register(Reset);
            TilePlacement.Events.OnFailurePlacement.Register(Activate);
        }

        private void UnregisterCallbacks()
        {
            TilePlacement.Events.OnFailurePlacement.Unregister(Reset);
            TilePlacement.Events.OnFailurePlacement.Unregister(Activate);

            TilePlacement.Events.OnSuccessPlacement.Unregister(OnTilePlaced);
        }
        
        protected virtual void PrepareTile(Tile.Tile tile)
        {
            RegisterCallbacks();
            tilePlacement.Start(tile);
        }

        protected virtual void OnTilePlaced()
        {
            UnregisterCallbacks();
        }
    } 
}