using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static Game;

public class PropagatorPopUp : GameSystemMonobehaviour
{
    [SerializeField]
    private Pool pool;

    public List<PropagatorLabel> labels = new List<PropagatorLabel>();

    public class PropagatorLabel
    {
        private Label label = null;
        private ObjectPool<PropagatorLabel> pool = null;
        private Transform transform = null;

        public PropagatorLabel(Label label,  ObjectPool<PropagatorLabel> pool)
        {
            this.label = label;
            this.pool = pool;
        }

        public void Initialize(string text, Color color, Transform transformRef)
        {
            SetText(text);
            SetColor(color);
            transform = transformRef;
            SetPosition();
        }

        public void SetPosition()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            SetPosition(new Vector2(screenPos.x, 1080 - screenPos.y));
        }

        public void SetPosition(Vector2 screenPos)
        {
            label.transform.position = screenPos;
        }
            
        public void SetColor(Color color) => label.style.color = color;
        public void SetText(string text) => label.text = text;
        public void Remove() => pool.Release(this);
    }

    public override void InitSystem()
    {
        pool.InitPool();
    }

    public PropagatorLabel Show(int number, Color color, Transform transform)
    {
        var label = pool.Get();
        label.Initialize(number.ToString(), color, transform);
        labels.Add(label);
        return label;
    }

    public void Hide(PropagatorLabel label)
    {
        labels.Remove(label);
        label.Remove();
    }

    private void Update()
    {
        foreach (var label in labels)
        {
            label.SetPosition();
        }
    }
}
