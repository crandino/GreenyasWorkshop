using HexaLinks.Path.Finder;
using HexaLinks.Tile.Events;
using UnityEngine;
using static HexaLinks.Tile.Events.TileEvents;

namespace HexaLinks.Tile
{
    public class TilePropagator : Tile
	{
		private const int MAX_PROPAGATOR_STRENGTH = 12;

		[SerializeField]
		private int maxPropagatorStrength;

        [HideInInspector]
		public int currentPropagatorStrength = 0;

        public Gate.ExposedGate StartingGate => new Gate.ExposedGate(GetComponentInChildren<Gate>());												  

        public override void Initialize()
        {
            base.Initialize();
            currentPropagatorStrength = maxPropagatorStrength;
            OnTurnEnded.RegisterPermamentCallback(IncreaseStrength);
        }

        public override bool TryRelease()
        {
            if (base.TryRelease())
            {
                propagatorLabel = PropagatorPopUpHelper.Show(currentPropagatorStrength, transform);

                OnPropagationStep.RegisterVolatileCallback(DecreaseStrength);

                PathFinder.Reset();
                PathFinder.Init(this);
                return true;
            }

            return false;
        }

        private PropagatorPopUp.PropagatorLabel propagatorLabel;

        private void IncreaseStrength(EmptyArgs? noArgs)
        {
            currentPropagatorStrength = Mathf.Clamp(currentPropagatorStrength + 1, 0, maxPropagatorStrength);
            propagatorLabel.SetText(currentPropagatorStrength.ToString());
        }

        private void DecreaseStrength(EmptyArgs? args)
        {
            currentPropagatorStrength = Mathf.Clamp(currentPropagatorStrength - 1, 0, maxPropagatorStrength);
            propagatorLabel.SetText(currentPropagatorStrength.ToString());
        }

#if UNITY_EDITOR
        private void Reset()
        {
			maxPropagatorStrength = MAX_PROPAGATOR_STRENGTH / (GetComponentsInChildren<TileSegment>().Length - 1); 
        }
#endif
    } 
}
