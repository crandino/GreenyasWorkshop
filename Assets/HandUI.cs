using HexaLinks.Ownership;
using HexaLinks.UI.PlayerHand;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static DeckContent;

public class HandUI : MonoBehaviour
{
    [SerializeField]
    private TileResource emptyTile;

    [SerializeField]
    private UIDocument playerHandUI;

    [SerializeField]
    private Score score;

    private HandTileOption[] playerOptions = null;
    public static TileResource EmptyTile;

    public void Initialize(Deck deck)
    {
        EmptyTile = emptyTile;

        List<Button> buttons = playerHandUI.rootVisualElement.Query<Button>().ToList();
        playerOptions = new HandTileOption[buttons.Count];

        for (int i = 0; i < 3; ++i)
            playerOptions[i] = new(buttons[i], deck.TraversalDeck);

        playerOptions[^1] = new HandPropagatorOption(buttons[^1], playerHandUI.rootVisualElement.Query<Label>("TileCounter"), deck.PropagatorDeck);

        Draw();
        Deactivate();
    }

    public void Activate()
    {
        foreach (HandTileOption option in playerOptions)
        {
            option.Activate();
            option.Reset();
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
        foreach (HandTileOption option in playerOptions)
        {
            option.Draw(EmptyTile);
        }
    }
}
