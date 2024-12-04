using HexaLinks.Path.Finder;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace HexaLinks.Tile
{
    public class TilePropagator : Tile
	{
		private const int MAX_PROPAGATOR_STRENGTH = 12;

		[SerializeField]
		private int maxPropagatorStrength;

        [HideInInspector]
		public int currentPropagatorStrength = 0;

        public Gate.ExposedGate[] StartingGates => GetComponentsInChildren<TerminalSegment>().
												   Where(s => s is TerminalSegment).
												   SelectMany(s => s.ExposedGates).ToArray();

        public override void Initialize()
        {
            base.Initialize();
            currentPropagatorStrength = maxPropagatorStrength;
        }

        public override bool TryRelease()
        {
            if (base.TryRelease())
            {
                PathFinder.Reset();
                PathFinder.Init(this);
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        private void Reset()
        {
			maxPropagatorStrength = MAX_PROPAGATOR_STRENGTH / GetComponentsInChildren<TraversalSegment>().Length; 
        }
#endif
    } 
}
