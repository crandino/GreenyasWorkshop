using HexaLinks.Ownership;
using HexaLinks.UI.PlayerHand;
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

    public void Initialize(Deck deck, PlayerOwnership.Ownership ownership)
    {
        EmptyTile = emptyTile;

        List<Button> buttons = playerHandUI.rootVisualElement.Query<Button>().ToList();
        playerOptions = new HandTileOption[buttons.Count];

        HandTileOption option;

        for (int i = 0; i < 3; ++i)
        {
            option = new(buttons[i], deck.TraversalDeck);
            option.Draw(emptyTile);
            playerOptions[i] = option;
        }

        option = new HandPropagatorOption(buttons[^1], playerHandUI.rootVisualElement.Query<Label>("TileCounter"), deck.PropagatorDeck, ownership);
        option.Set(emptyTile);
        playerOptions[^1] = option;

        //score.Initialize();

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
        foreach (var o in options)
        {
            o.Draw(emptyTile);
        }
    }
}
