using HexaLinks.UI.PlayerHand;

#if RECORDING
public class DrawDeckRecord : Record<HandTileOption>
{
    private readonly TileResource resourceUsed = null;
    private readonly TileResource resourceDrawn = null;

    public DrawDeckRecord(HandTileOption handOption, TileResource resourceUsed, TileResource resourceDrawn) : base(handOption)
    {
        this.resourceUsed = resourceUsed;
        this.resourceDrawn = resourceDrawn;
    }

    public override void Redo()
    {
        actor.FakeDraw(resourceDrawn);
    }

    public override void Undo()
    {
        actor.FakeUndraw(resourceUsed);
    }
} 
#endif


