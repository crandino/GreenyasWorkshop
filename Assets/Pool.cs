using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Pool : MonoBehaviour
{
    [SerializeField]
    private UIDocument canvas;

    [SerializeField]
    private VisualTreeAsset numberTemplate;

    public Vector2 position;

    private ObjectPool<Label> pool = null;

    void Start()
    {
        pool = new(Create);
    }

    private Label Create()
    {
        var pooledObject = numberTemplate.Instantiate();
        Label l = pooledObject.Q<Label>();
        canvas.rootVisualElement.Add(l);
        return l;
    }

    public Label Get()
    {
        var obj = pool.Get();
        //Vector3 pos = Camera.main.WorldToScreenPoint(Vector3.zero);
        obj.transform.position = position;
        return obj;
    }

    private void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
            Get();
    }
}
