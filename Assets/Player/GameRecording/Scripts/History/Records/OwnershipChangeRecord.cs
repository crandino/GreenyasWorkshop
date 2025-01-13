using HexaLinks.Ownership;

public class OwnershipChangeRecord : Record<PlayerOwnership>
{
    private readonly Owner previousOwner;
    private readonly Owner newOwner;

    public OwnershipChangeRecord(PlayerOwnership ownerhsip, Owner previousOwner, Owner newOwner) : base(ownerhsip)
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