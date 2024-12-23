using HexaLinks.Configuration;
using HexaLinks.Turn;
using System.Collections.Generic;
using UnityEngine;
using static Game;

public class StrengthIndicatorCanvas : GameSystemMonobehaviour
{
    [SerializeField]
    private Pool pool;

    private Colors colors;

    private readonly List<StrenghtIndicator> activeIndicators = new();

    public Color CurrentLabel => colors[TurnManager.CurrentPlayer].labelColor;

    public override void InitSystem()
    {
        pool.InitPool();
        colors = Game.Instance.GetSystem<Configuration>().colors;
    }

    public StrenghtIndicator Show(int number, Transform transformToFollow)
    {
        StrenghtIndicator ind = Get();
        ind.UpdateValues(number.ToString(), CurrentLabel, transformToFollow);

        return ind;
    }

    public void ShowWithCountdown(string number, Transform transformToFollow, float timeInSeconds)
    {
        StrenghtIndicator ind = Get();
        ind.UpdateValues(number, CurrentLabel, transformToFollow);
        ind.SetTimeToHide(timeInSeconds);
    }

    public void Hide(StrenghtIndicator indicator)
    {
        activeIndicators.Remove(indicator);
        pool.Release(indicator);
    }

    private void Update()
    {
        for(int i = activeIndicators.Count - 1; i >= 0; i--)
        {
            StrenghtIndicator indicator = activeIndicators[i];

            if (!indicator.Update())
            {
                Hide(indicator);
            }
        }
    }

    private StrenghtIndicator Get()
    {
        StrenghtIndicator ind = pool.Get();
        ind.transform.SetParent(transform);
        activeIndicators.Add(ind);
        return ind;
    }
}
