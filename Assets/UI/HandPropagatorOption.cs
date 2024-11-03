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

        public HandPropagatorOption(Button button, Label counter, DeckContent.Deck.DrawableDeck deck) : base(button, deck)
        {
            counterLabel = counter;
            InitializeCountdown();
        }

        private void InitializeCountdown()
        {
            SideGate.OnGateConnected += OnSegmentConnected;
            Counter = CONNECTIONS_TO_GET;
            counterLabel.visible = true;
            DrawingPending = false;
            Set(Hand.EmptyTile);
        }

        private void FinalizeCountdown()
        {
            SideGate.OnGateConnected -= OnSegmentConnected;
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

        private void OnTilePlaced()
        {
            Game.Instance.GetSystem<TilePlacement>().OnSuccessPlacement -= OnTilePlaced;
            InitializeCountdown();
            UnregisterCallbacks();
        }

        protected override void SelectTile(Action<Tile.Tile> onTileSelection)
        {
            base.SelectTile(onTileSelection);
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
   
