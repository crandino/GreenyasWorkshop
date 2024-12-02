using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument canvas;


    [ContextMenu("Create")]
    private void Create()
    {
        Label number = new Label("Buenos días");

        canvas.rootVisualElement.Add(number);

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(Vector3.zero);
        number.transform.position = screenPoint;

    }
}
