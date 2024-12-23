using UnityEngine;
using static Game;

namespace HexaLinks.Turn
{
    using Events;
    using Events.Arguments;
    using Ownership;
    using Propagation;
    using UI.PlayerHand;

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

                PropagationManager.Events.OnSegmentPropagated.Register(UpdateScore);
            }

            private void UpdateScore(OnSegmentPropagatedArgs args)
            {
                score.Value += args.GetScoreIncrement(ownerShip);
            }
        }

        [SerializeField]
        private PlayerContext playerOneContext, playerTwoContext;
        private static PlayerContext Current { set; get; } = null;

        public static Owner CurrentPlayer { private set; get; }

        private TurnSteps steps = null;

        public override void InitSystem()
        {
            playerOneContext.Init();
            playerTwoContext.Init();
            CurrentPlayer = Owner.None;
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
                    Events.OnTurnEnded.Call();
                    Game.Instance.GetSystem<TurnManager>().ChangePlayer();
                    Initialize();
                }
            }
        }

        public static class Events
        {
            public readonly static EventType OnTurnEnded = new();
        }
    }

}