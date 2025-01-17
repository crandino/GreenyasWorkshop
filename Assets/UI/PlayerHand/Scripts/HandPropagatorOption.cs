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

        public override void Enable()
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (CountdownReached)
                base.Enable();
            else
                Disable();
        }

        private void OnSegmentConnected()
        {
            --Counter;
            UpdateStatus();
#if RECORDING
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyPropagatorCounterRecord(this, -1));
#endif
        }

        private void OnSegmentBlocked()
        {
            ++Counter;
            UpdateStatus();
#if RECORDING
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyPropagatorCounterRecord(this, +1));
#endif

        }

#if RECORDING
        public void ForceCountdown(int increment)
        {
            Counter += increment;
        }
#endif
        protected override void OnTilePlaced()
        {
#if RECORDING
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyPropagatorCounterRecord(this, connectionsToUnlock - Counter));
#endif
            base.OnTilePlaced();
            InitializeCountdown();
            UpdateStatus();
        }

        protected override void PrepareTile(Tile tile)
        {
            base.PrepareTile(tile);
            tile.GetComponentInChildren<PlayerOwnership>().InstantOwnerChange(TurnManager.CurrentPlayer);
        }
    }
}

