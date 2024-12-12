using HexaLinks.Configuration;
using HexaLinks.Turn;
using UnityEngine;

public static class PropagatorPopUpHelper
{
    private static readonly TurnManager turnManager;
    private static readonly PropagatorPopUp propagatorPopUp;
    private static readonly Colors colors;

    public static Color CurrentLabelColor => colors[turnManager.CurrentPlayer].labelColor;

    static PropagatorPopUpHelper()
    {
        turnManager = Game.Instance.GetSystem<TurnManager>();
        propagatorPopUp = Game.Instance.GetSystem<PropagatorPopUp>();
        colors = Game.Instance.GetSystem<Configuration>().colors;
    }

    public static PropagatorPopUp.PropagatorLabel Show(int number, Transform transform)
    {
        return propagatorPopUp.Show(number, CurrentLabelColor, transform);
    }

    public static void Hide(PropagatorPopUp.PropagatorLabel label)
    {
        propagatorPopUp.Hide(label);
    }
}
