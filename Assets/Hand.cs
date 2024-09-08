using Hexalinks.Tile;
using UnityEngine;
using UnityEngine.UIElements;
using static TileResources;

public class Hand : MonoBehaviour
{
    private class HandOption
    {
        private Button button;
        private TileResourceLocator tileResourceLocator;

        public HandOption(Button button)
        {
            this.button = button;
        }

        public void AssignTile(TileResourceLocator locator)
        {
            tileResourceLocator = locator;

            button.iconImage = Background.FromSprite(locator.sprite);
            button.clicked += LoadTile;
        }

        private void LoadTile()
        {
            button.iconImage = Background.FromSprite(Hand.emptyIcon);

            Tile tile = GameObject.Instantiate<Tile>(tileResourceLocator.tile);
            tile.Initialize();
            tile.PickUp();

            button.clickable = null;
        }
    }

    private HandOption[] playerOptions = null;
    private DeckContent.Deck deck;

    [SerializeField]
    UIDocument playerHandUI;

    [SerializeField]
    private DeckContent deckContent;

    [SerializeField]
    private TileResources resources;

    private bool active = false;
    public static Sprite emptyIcon;

    private void Start()
    {
        Initialize(deckContent);
        emptyIcon = resources.EmptyIcon;
    }

    public void Initialize(DeckContent content)
    {
        deck = content.CreateDeck();
        playerOptions = playerHandUI.rootVisualElement.Query<Button>().ForEach(b =>
        {
            HandOption handOption = new HandOption(b);
            handOption.AssignTile(resources[deck.Draw()]);
            return handOption;
        }).ToArray();
    }
}
