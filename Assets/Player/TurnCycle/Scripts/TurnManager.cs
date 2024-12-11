using UnityEngine;
using static Game;

namespace HexaLinks.Turn
{
    using Tile.Events;
    using UI.PlayerHand;
    using Ownership;   

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

                TileEvents.OnSegmentPropagated.RegisterPermamentCallback(UpdateScore);
            }

            private void UpdateScore(OnSegmentPropagatedArgs? args)
            {
                score.Value += args.Value.GetScoreIncrement(ownerShip);
            }
        }

        [SerializeField]
        private PlayerContext playerOneContext, playerTwoContext;
        private static PlayerContext Current { set; get; }

        public Owner CurrentPlayer => Current.ownerShip;

        //private TurnSteps steps = null;

        public override void InitSystem()
        {
            playerOneContext.Init();
            playerTwoContext.Init();

            Current = playerOneContext;

            new TurnSteps(this);
        }

        private void ChangePlayer()
        {
            Current = (Current == playerOneContext) ? playerTwoContext : playerOneContext;
        }

        public class TurnSteps
        {
            private readonly TurnManager turnManager;
            private readonly TurnStep[] steps;
            private int stepIndex = 0;

            private TurnStep Step => steps[stepIndex];

            public TurnSteps(TurnManager turnManager)
            {
                this.turnManager = turnManager;

                steps = new TurnStep[]
                {
                    new TileSelectionTurnStep(NextStep),
                    new PropagationTurnStep(NextStep),
                    new DeckDrawingTurnStep(NextStep)
                };

                Initialize();
            }

            private void Initialize()
            {
                stepIndex = 0;
                Step.Begin(Current);
            }

            private void NextStep()
            {
                if (++stepIndex < steps.Length)
                    Step.Begin(Current);
                else
                {
                    TileEvents.OnTurnEnded.Call(null);
                    turnManager.ChangePlayer();
                    Initialize();
                }
            }
        }
    }

}