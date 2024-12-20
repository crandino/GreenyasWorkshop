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
                hand.Initialize();
                score.Initialize();

                TileEvents.OnSegmentPropagated.RegisterCallback(UpdateScore);
            }

            private void UpdateScore(OnSegmentPropagatedArgs? args)
            {
                score.Value += args.Value.GetScoreIncrement(ownerShip);
            }
        }

        [SerializeField]
        private PlayerContext playerOneContext, playerTwoContext;
        private static PlayerContext Current { set; get; } = null;

        public static Owner CurrentPlayer { private set; get; } = Owner.None;

        private TurnSteps steps = null;

        public override void InitSystem()
        {
            playerOneContext.Init();
            playerTwoContext.Init();           
        }

        public void StartGame()
        {
            Current = playerOneContext;
            CurrentPlayer = Current.ownerShip;
            steps = new TurnSteps();
        }

        private void ChangePlayer()
        {
            Current = (Current == playerOneContext) ? playerTwoContext : playerOneContext;
            CurrentPlayer = Current.ownerShip;
        }

        public class TurnSteps
        {
            private readonly TurnStep[] steps = null;
            private int stepIndex = 0;

            private TurnStep Step => steps[stepIndex];

            public TurnSteps()
            {
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
                    Game.Instance.GetSystem<TurnManager>().ChangePlayer();
                    Initialize();
                }
            }
        }
    }

}