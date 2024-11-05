using HexaLinks.Ownership;
using HexaLinks.UI.PlayerHand;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private PlayerContext playerOneContext, playerTwoContext;
    public static TurnSteps Steps { private set; get; }

    static TurnManager()
    {
        Steps = new TurnSteps();
    }
   
    private void Start()
    {
        playerOneContext.Init();
        playerTwoContext.Init();

        TileEvents.DisableCallbacks(playerTwoContext.ownerShip);
        TileEvents.EnableCallbacks(playerOneContext.ownerShip);

        Steps.Initialize(playerOneContext);
    }

    public void ChangePlayer(PlayerContext lastPlayer)
    {
        PlayerContext newPlayer = lastPlayer == playerOneContext ? playerTwoContext : playerOneContext;
        
        TileEvents.DisableCallbacks(lastPlayer.ownerShip);
        TileEvents.EnableCallbacks(newPlayer.ownerShip);

        Steps.Initialize(newPlayer);
    }

    [System.Serializable]
    public class PlayerContext
    {
        public PlayerOwnership.Ownership ownerShip;
        public Hand hand;
        public TurnManager turnManager;

        // UI -> Score 
        // UI -> PlayerHand

        public void Init()
        {
            hand.Initialize(ownerShip);
        }
    }

    public class TurnSteps
    {
        public TurnStep[] steps;
        private PlayerContext context = null;

        private int stepIndex = 0;

        public TurnSteps()
        {
            steps = new TurnStep[]
            {
                new TileSelectionTurnStep(),
                new DeckDrawingTurnStep()
            };
        }      

        public void Initialize(PlayerContext context)
        {
            this.context = context;
            
            stepIndex = 0;
            steps[stepIndex].Begin(context);
        }

        public void NextStep()
        {
            steps[stepIndex].End();
            //yield return new WaitForEndOfFrame();
            steps[++stepIndex].Begin(context);
        }
    }
}
