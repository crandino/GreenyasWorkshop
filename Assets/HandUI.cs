using HexaLinks.UI.PlayerHand;
using System.Collections.Generic;
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
        Disable();
    }

    public void Activate()
    {
        TilePlacement.Events.OnStartPlacement.Register(Disable);
        TilePlacement.Events.OnFailurePlacement.Register(Enable);

        foreach (HandTileOption option in playerOptions)
        {
            option.Activate();
            option.Enable();
        }
    }

    private void Enable()
    {
        foreach (HandTileOption option in playerOptions)
        {
            option.Enable();
        }
    }

    public void Deactivate()
    {
        TilePlacement.Events.OnStartPlacement.Unregister(Disable);
        TilePlacement.Events.OnFailurePlacement.Unregister(Enable);

        foreach (HandTileOption option in playerOptions)
        {
            option.Deactivate();
            option.Disable();
        }
    }

    private void Disable()
    {
        foreach (HandTileOption option in playerOptions)
        {
            option.Disable();
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
