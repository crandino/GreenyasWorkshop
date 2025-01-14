using UnityEngine.UIElements;

namespace HexaLinks.UI.PlayerHand
{
    using Configuration;
    using Tile;
    using Turn;
    using Ownership;

    public class HandPropagatorOption : HandTileOption
    {
        private TileResource propagatorResource = null;

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
        }

        public override void Activate()
        {
            ConnectionCandidates.Events.OnSideConnected.Register(OnSegmentConnected);
            ConnectionCandidates.Events.OnSideBlocked.Register(OnSegmentBlocked);
        }

        public override void Deactivate()
        {
            ConnectionCandidates.Events.OnSideConnected.Unregister(OnSegmentConnected);
            ConnectionCandidates.Events.OnSideBlocked.Unregister(OnSegmentBlocked);
        }

        public override void Set(TileResource resource)
        {
            propagatorResource = resource;
            Set();        
        }

        private void Set()
        {
            base.Set(CountdownReached ? propagatorResource : HandUI.EmptyTile);
        }

        private void OnSegmentConnected()
        {
            --Counter;
#if RECORDING
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyPropagatorCounterRecord(this, -1));
#endif

            if (CountdownReached)
            {
                counterLabel.visible = false;
                Set();
            }
        }

        private void OnSegmentBlocked()
        {
            ++Counter;
#if RECORDING
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyPropagatorCounterRecord(this, 1));
#endif

            if (!CountdownReached)
            {
                counterLabel.visible = true;
                Set();
            }
        }

#if RECORDING
        public void ForceCountdown(int increment)
        {
            Counter += increment;
            counterLabel.visible = !CountdownReached;
            Set();
        }
#endif

        protected override void OnTilePlaced()
        {
            base.OnTilePlaced();
            InitializeCountdown();
#if RECORDING
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyPropagatorCounterRecord(this, connectionsToUnlock));
#endif
        }

        protected override void PrepareTile(Tile tile)
        {
            base.PrepareTile(tile);
            tile.GetComponentInChildren<PlayerOwnership>().InstantOwnerChange(TurnManager.CurrentPlayer);
        }
    }
}
   
