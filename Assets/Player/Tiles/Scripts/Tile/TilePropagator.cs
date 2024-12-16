using HexaLinks.Path.Finder;
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

        public Gate.ReadOnlyGate StartingGate => new Gate.ReadOnlyGate(GetComponentInChildren<Gate>());												  

        public override void Initialize()
        {
            base.Initialize();
            CurrentStrength = maxPropagatorStrength;

            propagatorLabel = PropagatorPopUpHelper.Show(CurrentStrength, transform);
            //OnTurnEnded.RegisterCallback(IncreaseStrength);

        }

        public override void Terminate()
        {
            base.Terminate();
            PropagatorPopUpHelper.Hide(propagatorLabel);
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
            propagatorLabel.SetColor(PropagatorPopUpHelper.CurrentLabelColor);
            propagatorLabel.SetText(CurrentStrength.ToString());

            OnPropagationStep.RegisterCallback(this, DecreaseStrength);
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
