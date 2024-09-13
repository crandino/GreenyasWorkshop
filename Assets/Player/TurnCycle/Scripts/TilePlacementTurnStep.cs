public class TilePlacementTurnStep : ForkTurnStep
{
    private TilePlacement tilePlacement;

    protected override void Awake()
    {
        tilePlacement = Game.Instance.GetSystem<TilePlacement>();
        tilePlacement.OnSuccessPlacement += OnTilePlacement;
        tilePlacement.OnFailurePlacement += OnCancelTilePlacement;
    }

    public override void Begin()
    {
        tilePlacement.Start(null);
    }

    private void OnTilePlacement()
    {
        Next();
        tilePlacement.Finish();
    }

    private void OnCancelTilePlacement()
    {
        Next();
        tilePlacement.Finish();
    }
}

