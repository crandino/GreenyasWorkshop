using Newtonsoft.Json.Linq;
using NUnit.Framework.Internal.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private HandInitializationTurnStep handInitTurnStep;
    [SerializeField] private TileSelectionTurnStep tileSelectionTurnStep;
    [SerializeField] private TilePlacementTurnStep tilePlacementTurnStep;
    [SerializeField] private DeckDrawingTurnStep deckDrawingTurnStep;

    private void Start()
    {
        handInitTurnStep.SetNextStep(tileSelectionTurnStep);
        tileSelectionTurnStep.SetNextStep(tilePlacementTurnStep);
        tilePlacementTurnStep.SetNextStep(deckDrawingTurnStep, tileSelectionTurnStep);
        deckDrawingTurnStep.SetNextStep(tileSelectionTurnStep);
    }
}

public abstract class TurnStep : MonoBehaviour 
{
    private TurnStep successTurnStep, failureTurnStep;

    public void SetNextStep(TurnStep success, TurnStep failure)
    {
        successTurnStep = success;
        failureTurnStep = failure;
    }

    public void SetNextStep(TurnStep success)
    {
        successTurnStep = failureTurnStep = success;
    }

    public void Success()
    {
        successTurnStep.Init();
    }

    public void Failure()
    {
        failureTurnStep.Init();
    }

    public abstract void Init();
}


public class HandInitializationTurnStep : TurnStep
{
    [SerializeField]
    private 

        // TODO: Quizá, Deck enlaza con la Hand, en vez de al revés y podemos mover el mazo por todos los turnos

    public override void Init()
    {

    }

    // Player options
    // Recursos
    // Contenido de mazo
}

public class TileSelectionTurnStep : TurnStep
{
    public override void Init()
    {

    }

    // Arrancamos partida con mazo y selección de primeras 3 tiles
    // Selección de Tile del menú Hand
    // Jugador decide:
    //        * Coloca pieza --> escogemos nueva pieza
    //        * Cancelamos pieza --> volvemos a la selección
}

public class TilePlacementTurnStep : TurnStep
{
    public override void Init()
    {

    }
}

public class DeckDrawingTurnStep : TurnStep
{
    public override void Init()
    {

    }
}
