#if RECORDING
public class ModifyScoreRecord : Record<Score>
{
    private readonly int scoreVariation = 0;

    public ModifyScoreRecord(Score score, int scoreVariation) : base(score)
    {
        this.scoreVariation = scoreVariation;
    }

    public override void Redo()
    {
        actor.Value += scoreVariation;
    }

    public override void Undo()
    {
        actor.Value -= scoreVariation;
    }
} 
#endif

