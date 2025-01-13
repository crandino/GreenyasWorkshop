using HexaLinks.UI.PlayerHand;

public class ModifyPropagatorCounterRecord : Record<HandPropagatorOption>
{
    private readonly int counterVariation;

    public ModifyPropagatorCounterRecord(HandPropagatorOption handOption, int variation) : base(handOption)
    {
        counterVariation = variation;
    }

    public override void Redo()
    {
        actor.ForceCountdown(+counterVariation);
    }

    public override void Undo()
    {
        actor.ForceCountdown(-counterVariation);
    } 
}


