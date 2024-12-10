using HexaLinks.Configuration;
using UnityEngine;

public static class PropagatorPopUpHelper
{
    private static TurnManager turnManager;
    private static PropagatorPopUp propagatorPopUp;
    private static Colors colors;

    static PropagatorPopUpHelper()
    {
        turnManager = Game.Instance.GetSystem<TurnManager>();
        propagatorPopUp = Game.Instance.GetSystem<PropagatorPopUp>();
        colors = Game.Instance.GetSystem<Configuration>().colors;
    }

    public static PropagatorPopUp.PropagatorLabel Show(int number, Transform transform)
    {
        return propagatorPopUp.PopUpNumber(number, colors[turnManager.CurrentPlayer].labelColor, transform);
    }
}
