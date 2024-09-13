using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private HandInitializationTurnStep handInitTurnStep;

    private void Start()
    {
        handInitTurnStep.Begin();        
    }
}

public class DeckDrawingTurnStep : TurnStep
{
    public override void Begin()
    {

    }
}
