using HexaLinks.Tile;

#if RECORDING
public class ModifyStrengthRecord : Record<TilePropagator>
{
    private readonly int variation = 0;

    public ModifyStrengthRecord(TilePropagator propagator, int variation) : base(propagator)
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
#endif

