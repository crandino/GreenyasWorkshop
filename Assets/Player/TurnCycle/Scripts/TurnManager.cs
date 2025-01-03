using UnityEngine;
using static Game;

namespace HexaLinks.Turn
{
    using Events;
    using Events.Arguments;
    using Ownership;
    using Propagation;
    using UI.PlayerHand;
    using UnityEngine.InputSystem;

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

            public void Terminate()
            {
                PropagationManager.Events.OnSegmentPropagated.Unregister(UpdateScore);
            }

            private void UpdateScore(OnSegmentPropagatedArgs args)
            {
                CommandHistory.AddCommand(new ModifyScoreCommand(score, args.GetScoreVariation(ownerShip)));
                //score.Value += args.GetScoreVariation(ownerShip);
            }

            public bool IsMaxScoreReached => score.IsMaxScoreReached;
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

        public override void TerminateSystem()
        {
            playerOneContext.Terminate();
            playerTwoContext.Terminate();
            Events.OnTurnEnded.Clear();
        }

        private void Update()
        {
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                CommandHistory.Undo();

            }
            else if(Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                CommandHistory.Redo();

            }
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


        private bool IsGameEnded
        {
            get
            {
                return playerOneContext.IsMaxScoreReached || playerTwoContext.IsMaxScoreReached;
            }
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
                    PrepareNextPlayer();
            }

            private void PrepareNextPlayer()
            {
                Events.OnTurnEnded.Call();
                CommandHistory.Save();

                TurnManager turnManager = Game.Instance.GetSystem<TurnManager>();

                if (turnManager.IsGameEnded)
                {
                    Current = null;
                    CurrentPlayer = Owner.None;
                }
                else
                {
                    turnManager.ChangePlayer();
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