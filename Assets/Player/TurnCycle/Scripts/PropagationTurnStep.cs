namespace HexaLinks.Turn
{
    using Path.Finder;
    using Propagation;

    public class PropagationTurnStep : TurnStep
    {
        public PropagationTurnStep(TurnManager.TurnSteps turnSteps) : base(turnSteps)
        { }  
        
        public override void Begin()
        {
            PropagationManager.Events.OnPropagationEnded.Register(OnPropagationEnded);
            Game.Instance.GetSystem<PathIterator>().TriggerSearch();
        }  

        private void OnPropagationEnded()
        {
            SafeEnd();
            turnSteps.NextStep();
        }
        
        public override void SafeEnd()
        {
            PropagationManager.Events.OnPropagationEnded.Unregister(OnPropagationEnded);
        }
    }
}