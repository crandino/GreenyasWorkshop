using HexaLinks.Tile;

public class ModifyStrengthCommand : CommandRecord<TilePropagator>
{
    private readonly int variation = 0;

    public ModifyStrengthCommand(TilePropagator propagator, int variation) : base(propagator)
    {
        this.variation = variation;
    }

    public override void Redo()
    {
        actor.CurrentStrength += variation;
    }

    public override void Undo()
    {
        actor.CurrentStrength -= variation;
    }
}

