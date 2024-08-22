using Hexalinks.Tile;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHand : MonoBehaviour
{
    [SerializeField]
    private UIDocument playerZoneUI;

    //[SerializeField]
    //private SerializedDictionary<Tile.Type, Sprite> tileIcons;

    [SerializeField]
    private DeckCreator playerDeck;

    private PlayerOption[] playerOptions = null;
    private DeckCreator.Deck deck;

    private struct PlayerOption
    {
        private Button button;
        public bool TileAssigned => button.resolvedStyle.backgroundImage.sprite != null;

        public PlayerOption(Button button)
        {
            this.button = button;
        }

        public void AssignImage(Sprite tileIcon)
        {
            button.style.backgroundImage = Background.FromSprite(tileIcon);
        }
    }

    private void Start()
    {
        deck = playerDeck.Create();
        playerOptions = playerZoneUI.rootVisualElement.Query<Button>().ForEach(b => new PlayerOption(b)).ToArray();
    }

    //[ContextMenu("Get new")]
    //public void GetNewTile()
    //{
    //    Sprite icon = tileIcons[deck.Draw()];
    //    for (int i = 0; i < playerOptions.Length; i++)
    //    {
    //        if (!playerOptions[i].TileAssigned)
    //        {
    //            playerOptions[i].AssignImage(icon);
    //            break;
    //        }
    //    }
    //}
}
