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

        public void Initialize()
        {
            EmptyTile = emptyTile;
            deck = deckContent.CreateDeck();

            List<Button> buttons = playerHandUI.rootVisualElement.Query<Button>().ToList();
            playerOptions = new HandTileOption[buttons.Count];

            for (int i = 0; i < 3; ++i)
            {
                playerOptions[i] = new(buttons[i], deck.TraversalDeck);
                playerOptions[i].Draw(emptyTile);
            }

            playerOptions[^1] = new HandPropagatorOption(buttons[^1], playerHandUI.rootVisualElement.Query<Label>(), deck.PropagatorDeck);
            playerOptions[^1].Set(emptyTile);
        }

        public void ActivateSelection(Action<Tile.Tile> onTileSelection)
        {
            foreach (HandTileOption option in playerOptions)
            {
                option.Reset();
                option.ActivateSelection(onTileSelection);
            }
        }

        public void DeactivateSelection()
        {
            foreach (HandTileOption option in playerOptions)
            {
                option.DeactivateSelection();
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
