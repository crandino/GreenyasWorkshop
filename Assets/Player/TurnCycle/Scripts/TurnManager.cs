using HexaLinks.Tile.Events;
using HexaLinks.UI.PlayerHand;
using UnityEngine;
using static Game;
using Owner = HexaLinks.Ownership.PlayerOwnership.Ownership;

public class TurnManager : GameSystemMonobehaviour
{
    [System.Serializable]
    public class PlayerContext
    {
        public Owner ownerShip;
        public Hand hand;

        [SerializeField]
        private Score score;

        public void Init()
        {
            hand.Initialize(ownerShip);
            score.Initialize();

            TileEvents.OnSegmentPropagated.Callbacks += UpdateScore;
        }

        private void UpdateScore(OnSegmentPropagatedArgs args)
        {
            score.Value += args.GetScoreIncrement(ownerShip);
        }
    }

    [SerializeField]
    private PlayerContext playerOneContext, playerTwoContext;
    private static PlayerContext Current { set; get; }

    public Owner CurrentPlayer => Current.ownerShip;

    private TurnSteps steps = null;

    public override void InitSystem()
    {
        steps = new TurnSteps(this);

        playerOneContext.Init();
        playerTwoContext.Init();

        Current = playerOneContext;
        steps.Initialize();
    }

    public void ChangePlayer()
    {
        Current = (Current == playerOneContext) ? playerTwoContext : playerOneContext;
        steps.Initialize();
    }

    public class TurnSteps
    {
        private readonly TurnStep[] steps;
        private int stepIndex = 0;

        public TurnSteps(TurnManager turnManager)
        {
            steps = new TurnStep[]
            {
                new TileSelectionTurnStep(NextStep),
                new DeckDrawingTurnStep(turnManager.ChangePlayer)
            };
        }

        public void Initialize()
        {
            stepIndex = 0;
            steps[stepIndex].Begin(Current);
        }

        private void NextStep()
        {
            if (stepIndex < steps.Length)
                steps[++stepIndex].Begin(Current);
        }
    }
}
