using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static Game;

public class PropagatorPopUp : GameSystemMonobehaviour
{
    [SerializeField]
    private Pool pool;

    //public List<TextMeshProUGUI> tooltips = new List<TextMeshProUGUI>();

    //public class PropagatorLabel
    //{
    //    private Label label = null;
    //    private ObjectPool<TextMeshProUGUI> pool = null;
    //    private Transform transform = null;

    //    public PropagatorLabel(Label label,  ObjectPool<TextMeshProUGUI> pool)
    //    {
    //        this.label = label;
    //        this.pool = pool;
    //    }

    //    public void Initialize(string text, Color color, Transform transformRef)
    //    {
    //        SetText(text);
    //        SetColor(color);
    //        transform = transformRef;
    //        SetPosition();
    //    }

    //    public void SetPosition()
    //    {
    //        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
    //        SetPosition(new Vector2(screenPos.x, 1080 - screenPos.y));
    //    }

    //    public void SetPosition(Vector2 screenPos)
    //    {
    //        label.transform.position = screenPos;
    //    }
            
        //public void SetColor(Color color) => label.style.color = color;
        //public void SetText(string text) => label.text = text;
        //public void Remove() {  } //pool.Release(this);
    //}

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
