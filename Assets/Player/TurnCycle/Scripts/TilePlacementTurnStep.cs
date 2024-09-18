public class TilePlacementTurnStep : ForkTurnStep
{
    private TilePlacement tilePlacement;

    protected override void Start()
    {
        tilePlacement = Game.Instance.GetSystem<TilePlacement>();
        tilePlacement.OnSuccessPlacement += Next;
        tilePlacement.OnFailurePlacement += Previous;
    }

    public override void Begin()
    { }   
}

