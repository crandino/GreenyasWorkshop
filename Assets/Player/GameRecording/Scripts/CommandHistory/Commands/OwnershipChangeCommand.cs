using HexaLinks.Ownership;

public class OwnershipChangeCommand : CommandRecord<PlayerOwnership>
{
    private readonly Owner previousOwner;
    private readonly Owner newOwner;

    public OwnershipChangeCommand(PlayerOwnership ownerhsip, Owner previousOwner, Owner newOwner) : base(ownerhsip)
    {
        this.previousOwner = previousOwner;
        this.newOwner = newOwner;
    }

    public override void Redo()
    {
        actor.InstantOwnerChange(newOwner);
    }

    public override void Undo()
    {
        actor.InstantOwnerChange(previousOwner);
    }
}