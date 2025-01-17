using UnityEngine;
using static Game;

namespace HexaLinks.Turn
{
    using Events;
    using Events.Arguments;
    using Ownership;
    using Propagation;
    using UI.PlayerHand;

    public partial class TurnManager : GameSystemMonobehaviour
    {
        [System.Serializable]
        public class PlayerContext
        {
            [SerializeField]
            private Owner ownerShip;
            public Owner Ownership => ownerShip;

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
                int scoreVariation = args.GetScoreVariation(Ownership);
                score.Value += scoreVariation;

#if RECORDING
                Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyScoreRecord(score, scoreVariation));
#endif
            }

            public bool IsMaxScoreReached => score.IsMaxScoreReached;
        }

        [SerializeField]
        private PlayerContext playerOneContext, playerTwoContext;
        private static PlayerContext Current { set; get; } = null;
        public static Owner CurrentPlayer => Current != null ? Current.Ownership : Owner.None;       

        private TurnSteps steps = null;

        public override void InitSystem()
        {
            playerOneContext.Init();
            playerTwoContext.Init();
        }

        public override void TerminateSystem()
        {
            playerOneContext.Terminate();
            playerTwoContext.Terminate();
            Events.OnTurnEnded.Clear();
        }            

        public void StartGame()
        {
            Current = playerOneContext;

            steps = new TurnSteps();
            steps.StartTurn(Current);
        }

        private void ChangePlayer()
        {
            Current = (Current == playerOneContext) ? playerTwoContext : playerOneContext;
            steps.StartTurn(Current);
        }

        private bool IsGameEnded
        {
            get
            {
                return playerOneContext.IsMaxScoreReached || playerTwoContext.IsMaxScoreReached;
            }
        }

        public partial class TurnSteps
        {
            private readonly TurnStep[] steps = null;
            private int stepIndex = 0;

            private TurnStep CurrentStep => steps[stepIndex];
            public PlayerContext CurrentContext { private set; get; }

            public TurnSteps()
            {
                steps = new TurnStep[]
                {
                    new TileSelectionTurnStep(this),
                    new PropagationTurnStep(this),
                    new DeckDrawingTurnStep(this)
                };
            }

            public void StartTurn(PlayerContext currentContext)
            {
                stepIndex = 0;
                CurrentContext = currentContext;
                CurrentContext.hand.Activate();
                CurrentStep.Begin();
            }

            public void FinalizeTurn(bool prematureExit = false)
            {
                if (prematureExit)
                    CurrentStep.SafeEnd();

                CurrentContext.hand.Deactivate();                
            }

            public void NextStep()
            {
                if (++stepIndex < steps.Length)
                    CurrentStep.Begin();
                else
                    PrepareNextPlayer();
            }

            private void PrepareNextPlayer()
            {
                FinalizeTurn();
                Events.OnTurnEnded.Call();

                TurnManager turnManager = Game.Instance.GetSystem<TurnManager>();
#if RECORDING
                turnManager.History.Save(); 
#endif

                if (turnManager.IsGameEnded)
                {
                    TurnManager.Current = null;
                }
                else
                {
                    turnManager.ChangePlayer();
                }
            }
        }

        public static class Events
        {
            public readonly static EventType OnTurnEnded = new();
        }
    }
}