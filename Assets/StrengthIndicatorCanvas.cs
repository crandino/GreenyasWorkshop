using HexaLinks.Configuration;
using HexaLinks.Turn;
using UnityEngine;
using static Game;

public class StrengthIndicatorCanvas : GameSystemMonobehaviour
{
    [SerializeField]
    private Pool pool;

    private Colors colors;

    public Color CurrentLabel => colors[TurnManager.CurrentPlayer].labelColor;

    public override void InitSystem()
    {
        pool.InitPool();
        colors = Game.Instance.GetSystem<Configuration>().colors;
    }

    public StrenghtIndicator Get(int number, Transform transformToFollow)
    {
        StrenghtIndicator ind = pool.Get();

        ind.transform.SetParent(transform);
        ind.Update(number.ToString(), CurrentLabel, transformToFollow);

        return ind;
    }

    public void Hide(StrenghtIndicator label)
    {
        pool.Release(label);
    }  
}
