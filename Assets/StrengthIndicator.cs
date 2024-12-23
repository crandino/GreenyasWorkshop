using HexaLinks.Extensions.Vector;
using TMPro;
using UnityEngine;

public class StrenghtIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indicator = null;
    [SerializeField] new private Animation animation;

    private Transform transformToFollow = null;

    private const float NO_TIME = -1;
    private float timeCountDown = NO_TIME;

    public Transform TransformToFollow
    {
        set
        {
            transformToFollow = value;
            enabled = transformToFollow != null;
        }
    }

    public void SetTimeToHide(float timeInSeconds)
    {
        timeCountDown = timeInSeconds;
        animation.Play();
    }

    public bool Update()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transformToFollow.position).GetXY();
        indicator.transform.position = screenPos;

        if(timeCountDown != NO_TIME)
            timeCountDown -= Time.deltaTime;

        return timeCountDown == NO_TIME || timeCountDown > 0f;
    }

    public void UpdateValues(string text, Color color, Transform transformRef)
    {
        SetText(text);
        indicator.color = color;
        TransformToFollow = transformRef;
        timeCountDown = NO_TIME;
    }

    public void SetText(string text) => indicator.text = text;
}
