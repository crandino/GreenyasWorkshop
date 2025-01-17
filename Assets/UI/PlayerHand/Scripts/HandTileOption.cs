using HexaLinks.Turn;
using UnityEngine;
using UnityEngine.UIElements;

namespace HexaLinks.UI.PlayerHand
{
    public class HandTileOption
    {
        private readonly Button button;
        private readonly DeckContent.Deck.DrawableDeck deck;

        protected readonly TilePlacement tilePlacement = null;
        protected TileResource tileResource = null;

        public bool DrawingPending { protected set; get; } = true;
        protected bool Active => button.enabledSelf;

        public HandTileOption(Button tileButton, DeckContent.Deck.DrawableDeck drawableDeck)
        {
            button = tileButton;
            button.clicked += LoadTile;
            deck = drawableDeck;

            tilePlacement = Game.Instance.GetSystem<TilePlacement>();
        }

        public virtual void Set(TileResource resource)
        {
            tileResource = resource;
            Reset();
        }

        public void Reset()
        {
            button.iconImage = Background.FromSprite(tileResource.Icon);
            DrawingPending = false;         
        }

        public virtual void Activate() { }

        public virtual void Deactivate() { }

        public virtual void Enable()
        {
            button.SetEnabled(true);
        }

        public void Disable()
        {
            button.SetEnabled(false);
        }

        public void Draw(TileResource fallback)
        {
            if(DrawingPending)
            {
                TileResource drawnResource = deck.Draw(fallback);
#if RECORDING
                if (tileResource != null)
                    Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new DrawDeckRecord(this, tileResource, drawnResource));
#endif
                Set(drawnResource);
            }
            DrawingPending = false;
        }    

        public void FakeDraw(TileResource resource)
        {
            deck.FakeDraw();
            Set(resource);
        }

        public void FakeUndraw(TileResource resource)
        {
            deck.FakeUndraw();
            Set(resource);
        }

        private void LoadTile()
        {
            if (tileResource.Prefab != null)
            {
                PrepareTile(GameObject.Instantiate<Tile.Tile>(tileResource.Prefab));
                button.iconImage = Background.FromSprite(HandUI.EmptyTile.Icon);

                DrawingPending = true;
            }
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

        private void RegisterCallbacks()
        {
            TilePlacement.Events.OnSuccessPlacement.Register(OnTilePlaced);

            TilePlacement.Events.OnFailurePlacement.Register(Reset);
            TilePlacement.Events.OnFailurePlacement.Register(UnregisterCallbacks);
        }

        private void UnregisterCallbacks()
        {
            TilePlacement.Events.OnSuccessPlacement.Unregister(OnTilePlaced);

            TilePlacement.Events.OnFailurePlacement.Unregister(Reset);
            TilePlacement.Events.OnFailurePlacement.Unregister(UnregisterCallbacks);

        }
    } 
}