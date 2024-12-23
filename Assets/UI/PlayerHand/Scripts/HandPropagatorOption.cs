using UnityEngine.UIElements;

namespace HexaLinks.UI.PlayerHand
{
    using Configuration;
    using Tile;
    using Turn;
    using Ownership;

    public class HandPropagatorOption : HandTileOption
    {
        private int Counter
        {
            set
            {
                counterLabel.text = value.ToString();
            }

            get
            {
                return int.Parse(counterLabel.text);
            }
        }

        private readonly int connectionsToUnlock;
        private readonly Label counterLabel;

        private bool CountdownReached => Counter <= 0;

        public HandPropagatorOption(Button button, Label counter, DeckContent.Deck.DrawableDeck deck) : base(button, deck)
        {
            counterLabel = counter;
            connectionsToUnlock = Game.Instance.GetSystem<Configuration>().parameters.NumOfConnectionsToUnlockPropagator;
            InitializeCountdown();
        }

        private void InitializeCountdown()
        {
            Counter = connectionsToUnlock;

#if DEBUG
            if (connectionsToUnlock == 0)
            {
                counterLabel.visible = false;
                return;
            }
#endif
            
            counterLabel.visible = true;
            DrawingPending = false;

            Set(HandUI.EmptyTile);
        }

        private void OnSegmentConnected()
        {
#if DEBUG
            if (connectionsToUnlock == 0) return;
#endif
            --Counter;

            if (CountdownReached)
            {
                counterLabel.visible = false;
                DrawingPending = true;
            }
        }

        private void OnTilePlaced()
        {
            InitializeCountdown();
            UnregisterCallbacks();
        }

        protected override void PrepareTile(Tile tile)
        {
            base.PrepareTile(tile);
            tile.GetComponentInChildren<PlayerOwnership>().InstantOwnerChange(TurnManager.CurrentPlayer);
            RegisterCallbacks();
        }

        public override void Activate()
        {
            base.Activate();
            if(!CountdownReached)
                SideGate.Events.OnSegmentConnected.Register(OnSegmentConnected);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            SideGate.Events.OnSegmentConnected.Unregister(OnSegmentConnected);
        }

        private void RegisterCallbacks()
        {
            tilePlacement.AddEvents(OnTilePlaced, UnregisterCallbacks);
        }

        private void UnregisterCallbacks()
        {
            tilePlacement.RemoveEvents(OnTilePlaced, UnregisterCallbacks);
        }
    }
}
   
