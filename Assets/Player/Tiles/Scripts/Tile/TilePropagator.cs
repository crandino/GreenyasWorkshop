using HexaLinks.Path.Finder;
using HexaLinks.Tile.Events;
using HexaLinks.Turn;
using UnityEngine;
using static HexaLinks.Tile.Events.TileEvents;

namespace HexaLinks.Tile
{
    public class TilePropagator : Tile
	{
		private const int MAX_PROPAGATOR_STRENGTH = 12;

		[SerializeField]
		private int maxPropagatorStrength;

		public int CurrentStrength { private set; get; }

        public Gate.ExposedGate StartingGate => new Gate.ExposedGate(GetComponentInChildren<Gate>());												  

        public override void Initialize()
        {
            base.Initialize();
            CurrentStrength = maxPropagatorStrength;

            propagatorLabel = PropagatorPopUpHelper.Show(CurrentStrength, transform);
            OnTurnEnded.RegisterCallback(IncreaseStrength);

        }

        public override void Terminate()
        {
            base.Terminate();
            PropagatorPopUpHelper.Hide(propagatorLabel);
            OnTurnEnded.UnregisterCallback(IncreaseStrength);
        }

        public override bool TryRelease()
        {
            if (base.TryRelease())
            {
                Propagate();
                return true;
            }

            return false;
        }

        public void Propagate()
        {
            propagatorLabel.SetColor(PropagatorPopUpHelper.CurrentLabelColor);
            propagatorLabel.SetText(CurrentStrength.ToString());
            OnPropagationStep.RegisterCallback(this, DecreaseStrength);
            PathFinder.Init(this);
        }

        private PropagatorPopUp.PropagatorLabel propagatorLabel = null;

        private void IncreaseStrength(EmptyArgs? noArgs)
        {
            CurrentStrength = Mathf.Clamp(CurrentStrength + 1, 0, maxPropagatorStrength);
            propagatorLabel.SetText(CurrentStrength.ToString());
        }

        private void DecreaseStrength(EmptyArgs? args)
        {
            CurrentStrength = Mathf.Clamp(CurrentStrength - 1, 0, maxPropagatorStrength);
            propagatorLabel.SetText(CurrentStrength.ToString());
        }

#if UNITY_EDITOR
        private void Reset()
        {
			maxPropagatorStrength = MAX_PROPAGATOR_STRENGTH / (GetComponentsInChildren<TileSegment>().Length - 1); 
        }
#endif
    } 
}
