public class ModifyScoreCommand : CommandRecord<Score>
{
    private readonly int scoreVariation = 0;

    public ModifyScoreCommand(Score score, int scoreVariation) : base(score)
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

