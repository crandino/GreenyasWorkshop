using HexaLinks.Path.Finder;
using UnityEngine;

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
        }

        public override bool TryRelease()
        {
            if (base.TryRelease())
            {
                Ownership.PlayerOwnership.Ownership currentPlayer = Game.Instance.GetSystem<TurnManager>().CurrentPlayer;
                Game.Instance.GetSystem<PropagatorPopUp>().PopUpNumber(currentPropagatorStrength, Color.yellow, transform);

                PathFinder.Reset();
                PathFinder.Init(this);
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        private void Reset()
        {
			maxPropagatorStrength = MAX_PROPAGATOR_STRENGTH / (GetComponentsInChildren<TileSegment>().Length - 1); 
        }
#endif
    } 
}
