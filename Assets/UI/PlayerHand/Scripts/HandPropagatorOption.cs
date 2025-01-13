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
#if UNITY_EDITOR && DEBUG
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyPropagatorCounterRecord(this, -1));
#endif

            if (CountdownReached)
            {
                counterLabel.visible = false;
                DrawingPending = true;
            }
        }

#if UNITY_EDITOR && DEBUG
        public void ForceCountdown(int increment)
        {
            Counter += increment;
            counterLabel.visible = !CountdownReached;
            DrawingPending = false;

            //if(!CountdownReached)
                Set(HandUI.EmptyTile);
        }
#endif

        protected override void OnTilePlaced()
        {
#if UNITY_EDITOR && DEBUG
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyPropagatorCounterRecord(this, connectionsToUnlock));
#endif
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
        }
    }
}
   
