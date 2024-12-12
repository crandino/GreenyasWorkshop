using UnityEngine.UIElements;

namespace HexaLinks.UI.PlayerHand
{
    using Configuration;
    using Cysharp.Threading.Tasks;
    using Ownership;
    using Tile.Events;

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

        private Owner owner;

        public HandPropagatorOption(Button button, Label counter, DeckContent.Deck.DrawableDeck deck, Owner owner) : base(button, deck)
        {
            counterLabel = counter;
            this.owner = owner;

            connectionsToUnlock = Game.Instance.GetSystem<Configuration>().parameters.NumOfConnectionsToUnlockPropagator;
            InitializeCountdown();
        }

        private void InitializeCountdown()
        {
#if DEBUG
            if (connectionsToUnlock == 0)
            {
                counterLabel.visible = false;
                return;
            }
#endif

            Counter = connectionsToUnlock;
            counterLabel.visible = true;
            DrawingPending = false;

            Set(HandUI.EmptyTile);
        }

        private void OnSegmentConnected(TileEvents.EmptyArgs? noArgs)
        {
#if DEBUG
            if (connectionsToUnlock == 0) return;
#endif
            --Counter;

            if (CountdownReached)
            {
                counterLabel.visible = false;
                DrawingPending = true;
                Counter = connectionsToUnlock;
            }
        }

        private void OnTilePlaced()
        {
            InitializeCountdown();
            UnregisterCallbacks();
        }

        protected override void PrepareTile(Tile.Tile tile)
        {
            base.PrepareTile(tile);
            tile.GetComponentInChildren<InitialPlayerOwnership>().InstantOwnerChange(owner);
            RegisterCallbacks();
        }

        public override void Activate()
        {
            base.Activate();
            if(!DrawingPending)
                TileEvents.OnSegmentConnected.RegisterCallback(OnSegmentConnected);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            TileEvents.OnSegmentConnected.UnregisterCallback(OnSegmentConnected);
        }

        private void RegisterCallbacks()
        {
            Game.Instance.GetSystem<TilePlacement>().OnSuccessPlacement += OnTilePlaced;
            Game.Instance.GetSystem<TilePlacement>().OnFailurePlacement += UnregisterCallbacks;
        }

        private void UnregisterCallbacks()
        {
            Game.Instance.GetSystem<TilePlacement>().OnSuccessPlacement -= OnTilePlaced;
            Game.Instance.GetSystem<TilePlacement>().OnFailurePlacement -= UnregisterCallbacks;
        }
    }
}
   
