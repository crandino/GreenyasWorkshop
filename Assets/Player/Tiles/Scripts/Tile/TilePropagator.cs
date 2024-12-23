using HexaLinks.Path.Finder;
using UnityEngine;

namespace HexaLinks.Tile
{
    using static Propagation.PropagationManager.Events;

    public class TilePropagator : Tile
	{
		private const int MAX_PROPAGATOR_STRENGTH = 12;

		[SerializeField]
		private int maxPropagatorStrength;

		public int CurrentStrength { private set; get; }

        public Gate.ReadOnlyGate StartingGate => new Gate.ReadOnlyGate(GetComponentInChildren<Gate>());

        private StrengthIndicatorCanvas propagatorPopUp = null;
        private StrenghtIndicator strengthIndicator = null;

        public override void Initialize()
        {
            base.Initialize();
            CurrentStrength = maxPropagatorStrength;

            propagatorPopUp = Game.Instance.GetSystem<StrengthIndicatorCanvas>();
            strengthIndicator = propagatorPopUp.Show(CurrentStrength, transform);
            //OnTurnEnded.RegisterCallback(IncreaseStrength);
        }

        public override void Terminate()
        {
            base.Terminate();
            Game.Instance.GetSystem<StrengthIndicatorCanvas>().Hide(strengthIndicator);
            //OnTurnEnded.UnregisterCallback(IncreaseStrength);
        }

        public override bool TryRelease()
        {
            if (base.TryRelease())
            {
                PathIterator.QueueSearch(this);
                return true;
            }

            return false;
        }

        public void PreparePropagation()
        {
            strengthIndicator.UpdateValues(CurrentStrength.ToString(), propagatorPopUp.CurrentLabel, transform);
            OnPropagationStep.Register(DecreaseStrength);
        }

        private void IncreaseStrength()
        {
            CurrentStrength = Mathf.Clamp(CurrentStrength + 1, 0, maxPropagatorStrength);
            strengthIndicator.SetText(CurrentStrength.ToString());
        }

        private void DecreaseStrength()
        {
            CurrentStrength = Mathf.Clamp(CurrentStrength - 1, 0, maxPropagatorStrength);
            strengthIndicator.SetText(CurrentStrength.ToString());
        }

#if UNITY_EDITOR
        private void Reset()
        {
			maxPropagatorStrength = MAX_PROPAGATOR_STRENGTH / (GetComponentsInChildren<TileSegment>().Length - 1); 
        }
#endif
    } 
}
