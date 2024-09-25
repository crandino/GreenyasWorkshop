using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDrawingTurnStep : TurnStep
{
    [SerializeField]
    private Hand hand;

    public override void Begin()
    {
        // TODO Drawing from empty deck doesn't crash

        hand.Draw();
        Next();
    }
}
