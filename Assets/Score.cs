using HexaLinks.Configuration;
using UnityEngine;
using UnityEngine.UIElements;

public class Score : MonoBehaviour
{
    [SerializeField]
    private UIDocument playerHandUI;

    private Label scoreLabel;

    public bool IsMaxScoreReached => Value >= Game.Instance.GetSystem<Configuration>().parameters.MinScoreToWin;

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
