using UnityEngine;

public class HandInitializationTurnStep : TurnStep
{
    [SerializeField]
    private Hand hand;

    public override void Begin()
    {
        hand.Initialize();
        Next();
    }
}
