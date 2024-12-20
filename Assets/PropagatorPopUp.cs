using UnityEngine;
using static Game;

public class PropagatorPopUp : GameSystemMonobehaviour
{
    [SerializeField]
    private Pool pool;
    public override void InitSystem()
    {
        pool.InitPool();
    }

    public StrenghtIndicator Show(int number, Color color, Transform transformToFollow)
    {
        StrenghtIndicator ind = pool.Get();

        ind.transform.SetParent(transform);
        ind.Initialize(number.ToString(), color, transformToFollow);
        //label.Initialize(number.ToString(), color, transform);
        //labels.Add(label);
        //return label;


        return ind;
    }

    public void Hide(StrenghtIndicator label)
    {
        pool.Release(label);
        //label.tooltips();
    }

    //private void Update()
    //{
    //    foreach (var label in tooltips)
    //    {
    //        //label.SetPosition();
    //    }
    //}
}
