using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static PropagatorPopUp;

public class Pool : MonoBehaviour
{
    [SerializeField]
    private UIDocument canvas;

    [SerializeField]
    private VisualTreeAsset numberTemplate;

    private ObjectPool<PropagatorLabel> pool = null;
    private readonly static Vector3 OUT_OF_CANVAS_POSITION = new Vector3(5000, 5000, 0);

    public void InitPool()
    {
        pool = new(OnCreate, null, OnRelease);
    }

    public PropagatorLabel Get()
    {
        return pool.Get();
    }

    private PropagatorLabel OnCreate()
    {
        var pooledObject = numberTemplate.Instantiate();
        Label l = pooledObject.Q<Label>();
        canvas.rootVisualElement.Add(l);

        PropagatorLabel item = new PropagatorLabel(l, pool);
        return item;
    }

    private void OnRelease(PropagatorLabel label)
    {
        label.SetPosition(OUT_OF_CANVAS_POSITION);
    }

    
}
