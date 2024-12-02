using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private Label playerOneScore, playerTwoScore;

    [SerializeField]
    private UIDocument playerHandUI;

    [ContextMenu("Get")]
    private void Get()
    {
        //score = playerHandUI.rootVisualElement.Q<Label>("Score");
    }



}
