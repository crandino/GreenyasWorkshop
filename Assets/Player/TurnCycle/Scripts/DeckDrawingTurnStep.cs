using HexaLinks.UI.PlayerHand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDrawingTurnStep : TurnStep
{
    [SerializeField]
    private Hand hand;

    public override void Begin()
    {
        hand.Draw();
        Next();
    }
}
