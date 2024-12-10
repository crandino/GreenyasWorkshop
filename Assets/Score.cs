using UnityEngine;
using UnityEngine.UIElements;

public class Score : MonoBehaviour
{
    [SerializeField]
    private Label scoreLabel;

    [SerializeField]
    private UIDocument playerHandUI;

    public int Value
    {
        get
        {
            return int.Parse(scoreLabel.text);
        }

        set
        {
            scoreLabel.text = value.ToString();
        }
    }

    public void Initialize()
    {
        scoreLabel = playerHandUI.rootVisualElement.Q<Label>("Score");
        Value = 0;
    }
}
