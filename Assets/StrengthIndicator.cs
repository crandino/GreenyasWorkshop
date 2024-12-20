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

    public void Update(string text, Color color, Transform transformRef)
    {
        SetText(text);
        indicator.color = color;
        TransformToFollow = transformRef;
    }

    public void SetText(string text) => indicator.text = text;
}
