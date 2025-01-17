using HexaLinks.Extensions.Vector;
using TMPro;
using UnityEngine;

public class StrenghtIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indicator = null;
    [SerializeField] new private Animation animation;

    private const float NO_TIME = -1;
    private float timeCountDown = NO_TIME;

    private Transform transformToFollow = null;
    public Transform TransformToFollow
    {
        set
        {
            transformToFollow = value;
            enabled = transformToFollow != null;
        }
    }

    private Vector2 screenPos = Vector2.zero;

    private Vector2 ScreenPos
    {
        set
        {
            screenPos = value;
            indicator.transform.position = screenPos;
        }

        get
        {
            return transformToFollow != null ? Camera.main.WorldToScreenPoint(transformToFollow.position).GetXY() : screenPos;
        }
    }

    public void SetTimeToHide(float timeInSeconds)
    {
        timeCountDown = timeInSeconds;
        animation.Play();
    }

    public bool Update()
    {
        SetPosition();

        if (timeCountDown != NO_TIME)
            timeCountDown -= Time.deltaTime;

        return timeCountDown == NO_TIME || timeCountDown > 0f;
    }

    public void UpdateValues(string text, Color color, Vector3 worldPos)
    {
        SetText(text);
        indicator.color = color;
        ScreenPos = Camera.main.WorldToScreenPoint(worldPos).GetXY();
        timeCountDown = NO_TIME;
    }

    public void UpdateValues(string text, Color color, Transform transformRef)
    {
        TransformToFollow = transformRef;
        UpdateValues(text, color, transformRef.position);
    }

    public void SetText(string text)
    {
        indicator.text = text;
    }

    public void SetPosition()
    {
        indicator.transform.position = ScreenPos;
    }
}
