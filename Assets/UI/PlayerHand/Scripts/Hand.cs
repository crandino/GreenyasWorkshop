using HexaLinks.Ownership;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace HexaLinks.UI.PlayerHand
{
    public class Hand : MonoBehaviour
    {
        [SerializeField]
        private TileResource emptyTile;

        [SerializeField]
        private UIDocument playerHandUI;

        [SerializeField]
        private DeckContent deckContent;

        private HandTileOption[] playerOptions = null;
        private DeckContent.Deck deck;

        public static TileResource EmptyTile;

        public void Initialize(PlayerOwnership.Ownership playerOwner)
        {
            EmptyTile = emptyTile;
            deck = deckContent.CreateDeck();

            List<Button> buttons = playerHandUI.rootVisualElement.Query<Button>().ToList();
            playerOptions = new HandTileOption[buttons.Count];

            HandTileOption option;

            for (int i = 0; i < 3; ++i)
            {
                option = new(buttons[i], deck.TraversalDeck);
                option.Draw(emptyTile);
                playerOptions[i] = option;
            }

            option = new HandPropagatorOption(buttons[^1], playerHandUI.rootVisualElement.Query<Label>("TileCounter"), deck.PropagatorDeck, playerOwner);
            option.Set(emptyTile);
            playerOptions[^1] = option;

            Deactivate();
        }

        public void Activate()
        {
            foreach (HandTileOption option in playerOptions)
            {
                option.Reset();
                option.Activate();
            }
        }

        public void Deactivate()
        {
            foreach (HandTileOption option in playerOptions)
            {
                option.Deactivate();
            }
        }

        public void Draw()
        {
            var options = playerOptions.Where(o => o.DrawingPending);
            foreach(var o in options)
            {
                o.Draw(emptyTile);
            }
        }
    } 
}
