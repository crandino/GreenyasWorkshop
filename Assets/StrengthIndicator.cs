using TMPro;
using UnityEngine;

public class StrenghtIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indicator = null;
    [SerializeField] private Pool pool = null;

    private Transform transformToFollow = null;

    // TODO: 
    /* Añadir tiempos de vida de estos indicadores
     * Añadir animaciones para:
     *   - Cambio de texto
     *   - Resaltar la aparición
     */

    public Transform TransformToFollow
    {
        set
        {
            transformToFollow = value;
            enabled = transformToFollow != null;
        }
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transformToFollow.position);
        indicator.transform.position = screenPos;
    }

    //public StrenghtIndicator(Label label, ObjectPool<TextMeshProUGUI> pool)
    //{
    //    this.label = label;
    //    this.pool = pool;
    //}

    public void Initialize(string text, Color color, Transform transformRef)
    {
        SetText(text);
        SetColor(color);
        TransformToFollow = transformRef;
        //SetPosition();
    }

    //public void SetPosition()
    //{
    //    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
    //    SetPosition(new Vector2(screenPos.x, 1080 - screenPos.y));
    //}

    //public void SetPosition(Vector2 screenPos)
    //{
    //    label.transform.position = screenPos;
    //}

    public void SetColor(Color color) => indicator.color = color;
    public void SetText(string text) => indicator.text = text;
}
