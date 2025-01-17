using HexaLinks.Path.Finder;
using HexaLinks.Turn;
using UnityEngine;

namespace HexaLinks.Tile
{
    using static Propagation.PropagationManager;

    public class TilePropagator : Tile
    {
        private const int MAX_PROPAGATOR_STRENGTH = 12;

        [SerializeField]
        private int maxPropagatorStrength;

        private int currentStrength = 0;

        public int CurrentStrength
        {
            set
            {
                currentStrength = value;
                if(strengthIndicator != null)
                    strengthIndicator.UpdateValues(currentStrength.ToString(), strengthCanvas.CurrentLabel, transform);
            }

            get
            {
                return currentStrength;
            }
        }

        public Gate.ReadOnlyGate StartingGate => new Gate.ReadOnlyGate(GetComponentInChildren<Gate>());

        private StrengthIndicatorCanvas strengthCanvas = null;
        private StrenghtIndicator strengthIndicator = null;

        public override void Initialize()
        {
            base.Initialize();
            strengthCanvas = Game.Instance.GetSystem<StrengthIndicatorCanvas>();
            CurrentStrength = maxPropagatorStrength;
            strengthIndicator = strengthCanvas.Show(CurrentStrength, transform);
            TurnManager.Events.OnTurnEnded.Register(IncreaseStrength);
        }

        public override void Terminate()
        {
            base.Terminate();
            Game.Instance.GetSystem<StrengthIndicatorCanvas>().Hide(strengthIndicator);
            strengthIndicator = null;
            TurnManager.Events.OnTurnEnded.Unregister(IncreaseStrength);
        }

        public override bool TryRelease()
        {
            if (base.TryRelease())
            {
                Game.Instance.GetSystem<PathIterator>().QueueSearch(this);
                return true;
            }

            return false;
        }

        public override void Connect()
        {
            base.Connect();
            if(strengthIndicator == null)
                strengthIndicator = strengthCanvas.Show(CurrentStrength, transform);
            strengthIndicator.UpdateValues(currentStrength.ToString(), strengthCanvas.CurrentLabel, transform);
        }    

        public void ShowPropagationEvolution()
        {
            Events.OnPropagationStep.Register(DecreaseStrength);
        }

        private void IncreaseStrength()
        {
            ++currentStrength;/* = Mathf.Clamp(currentStrength + 1, 0, maxPropagatorStrength);*/
            strengthIndicator.SetText(currentStrength.ToString());

            StrengthIndicatorCanvas canvas = Game.Instance.GetSystem<StrengthIndicatorCanvas>();
            canvas.ShowWithCountdown($"+1", transform.position + Vector3.forward * 0.2f, 2f);
#if RECORDING
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyStrengthRecord(this, +1));
#endif
        }

        private void DecreaseStrength()
        {
            currentStrength = Mathf.Clamp(currentStrength - 1, 0, maxPropagatorStrength);
            strengthIndicator.SetText(currentStrength.ToString());

#if RECORDING
            Game.Instance.GetSystem<TurnManager>().History.RecordCommand(new ModifyStrengthRecord(this, -1));
#endif
        }

#if UNITY_EDITOR
        private void Reset()
        {
            maxPropagatorStrength = MAX_PROPAGATOR_STRENGTH / (GetComponentsInChildren<TileSegment>().Length - 1);
        }
#endif
    }
}
