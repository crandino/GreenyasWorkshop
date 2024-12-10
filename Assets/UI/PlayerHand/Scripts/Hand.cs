using HexaLinks.Ownership;
using UnityEngine;

namespace HexaLinks.UI.PlayerHand
{
    public class Hand : MonoBehaviour
    {
        [SerializeField]
        private DeckContent deckContent;
        private DeckContent.Deck deck;

        [SerializeField]
        private HandUI ui;

        public void Initialize(Owner playerOwner)
        {
            deck = deckContent.CreateDeck();
            ui.Initialize(deck, playerOwner);
            Deactivate();
        }

        public void Activate() => ui.Activate();
        public void Deactivate() => ui.Deactivate();
        
        public void Draw()
        {
            ui.Draw();
        }
    } 
}
