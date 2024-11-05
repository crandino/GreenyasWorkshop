using HexaLinks.UI.PlayerHand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDrawingTurnStep : TurnStep
{
    public override void Begin(TurnManager.PlayerContext context)
    {
        context.hand.Draw();
        context.turnManager.ChangePlayer(context);
    }
}
