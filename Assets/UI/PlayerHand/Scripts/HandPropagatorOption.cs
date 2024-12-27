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
            
            counterLabel.visible = true;
            DrawingPending = false;

            Set(HandUI.EmptyTile);
        }

        private void OnSegmentConnected()
        {
            if (!counterLabel.visible)
                return;

            --Counter;

            if (CountdownReached)
            {
                counterLabel.visible = false;
                DrawingPending = true;
            }
        }

        protected override void OnTilePlaced()
        {
            base.OnTilePlaced();
            InitializeCountdown();
        }

        public override void Activate()
        {
            base.Activate();
            SideGate.Events.OnSegmentConnected.Register(OnSegmentConnected);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            SideGate.Events.OnSegmentConnected.Unregister(OnSegmentConnected);
        }

        protected override void PrepareTile(Tile tile)
        {
            base.PrepareTile(tile);
            tile.GetComponentInChildren<PlayerOwnership>().InstantOwnerChange(TurnManager.CurrentPlayer);
            //RegisterCallbacks();
        }

        //private void RegisterCallbacks()
        //{
        //    TilePlacement.Events.OnFailurePlacement.Register(UnregisterCallbacks);
        //}

        //private void UnregisterCallbacks()
        //{
        //    TilePlacement.Events.OnFailurePlacement.Unregister(UnregisterCallbacks);
        //}
    }
}
   
