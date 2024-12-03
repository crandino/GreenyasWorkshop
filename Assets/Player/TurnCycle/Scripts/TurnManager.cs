using HexaLinks.UI.PlayerHand;
using UnityEngine;
using Owner = HexaLinks.Ownership.PlayerOwnership.Ownership;

public class TurnManager : MonoBehaviour
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

            TileEvents.OnSegmentPropagated.Register(UpdateScore);
        }

        private void UpdateScore(Owner propagationOwner)
        {
            score.ModifyScore(ownerShip, propagationOwner);
        }
    }

    [SerializeField]
    private PlayerContext playerOneContext, playerTwoContext;
    public PlayerContext CurrentContext { private set; get; }

    private TurnSteps steps;
   
    private void Start()
    {
        steps = new TurnSteps(this);

        playerOneContext.Init();
        playerTwoContext.Init();

        CurrentContext = playerOneContext;

        steps.Initialize(CurrentContext);
    }

    public void ChangePlayer()
    {
        CurrentContext = (CurrentContext == playerOneContext) ? playerTwoContext : playerOneContext;
        steps.Initialize(CurrentContext);
    }

    public class TurnSteps
    {
        private TurnStep[] steps;
        private PlayerContext context = null;

        private int stepIndex = 0;

        public TurnSteps(TurnManager turnManager)
        {
            steps = new TurnStep[]
            {
                new TileSelectionTurnStep(NextStep),
                new DeckDrawingTurnStep(turnManager.ChangePlayer)
            };
        }      

        public void Initialize(PlayerContext context)
        {
            this.context = context;

            TileEvents.Owner = context.ownerShip;

            stepIndex = 0;
            steps[stepIndex].Begin(context);
        }

        private void NextStep()
        {
            if(stepIndex < steps.Length) 
                steps[++stepIndex].Begin(context);
        }
    }
}
