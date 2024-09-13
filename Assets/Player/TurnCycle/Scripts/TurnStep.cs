using UnityEngine;

public abstract class TurnStep : MonoBehaviour
{
    [SerializeField]
    private TurnStep nextStep;  

    public void Next()
    {
        End();
        nextStep.Begin();
    }

    protected virtual void Awake() { }
    protected virtual void End() { }

    public abstract void Begin();

}

public abstract class ForkTurnStep : TurnStep
{
    [SerializeField]
    private TurnStep previousStep;

    public void Previous()
    {
        End();
        previousStep.Begin();
    }
}
