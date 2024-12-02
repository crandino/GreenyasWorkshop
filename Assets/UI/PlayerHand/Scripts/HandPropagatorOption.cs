using HexaLinks.Ownership;
using HexaLinks.Tile;
using System;
using UnityEngine.UIElements;

namespace HexaLinks.UI.PlayerHand
{
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

        private const int CONNECTIONS_TO_GET = 1;
        private readonly Label counterLabel;

        private bool CountdownReached => Counter <= 0;

        private PlayerOwnership.Ownership owner;

        public HandPropagatorOption(Button button, Label counter, DeckContent.Deck.DrawableDeck deck, PlayerOwnership.Ownership owner) : base(button, deck)
        {
            counterLabel = counter;
            this.owner = owner;
            InitializeCountdown();
        }

        private void InitializeCountdown()
        {
            Counter = CONNECTIONS_TO_GET;
            counterLabel.visible = true;
            DrawingPending = false;
            Set(Hand.EmptyTile);
        }

        private void FinalizeCountdown()
        {
            counterLabel.visible = false;
        }

        private void OnSegmentConnected()
        {
            --Counter;

            if (CountdownReached)
            {
                FinalizeCountdown();
                DrawingPending = true;
            }
        }

        public override void Activate()
        {
            base.Activate();
            SideGate.OnGateConnected += OnSegmentConnected;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            SideGate.OnGateConnected -= OnSegmentConnected;
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
   
