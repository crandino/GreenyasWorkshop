using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexaLinks.Propagation
{
    using Events;
    using Events.Arguments;
    using Tile;
    using Turn;
    using static Path.Finder.PathFinder;

    public class PropagationManager : Game.GameSystemMonobehaviour
    {
        private PathIterationStep iterationStep;
        private readonly List<GateSet> gateSetStep = new();
        private int stepIndex = 0;
        private int strength = 0;

        private class GateSet
        {
            public Gate.ReadOnlyGate[] gates;
            public NormalizedTimer timer;
            public bool Computes;
            public int Cost
            {
                get
                {
                    return gates.Count(g =>
                    {
                        return g.Ownership.ComputesInPropagation &&
                               !g.Ownership.IsSameOwner(TurnManager.CurrentPlayer);
                    });
                }
            }

            public GateSet(Gate.ReadOnlyGate[] gates)
            {
                this.gates = gates;
                timer = new NormalizedTimer(1f);
                Computes = gates.Any(o => o.Ownership.ComputesInPropagation);
            }
        }

        public override void TerminateSystem()
        {
            Events.Clear();
        }

        public void TriggerPropagation(PathIterationStep iterationStep)
        {
            this.iterationStep = iterationStep;

            enabled = true;
            stepIndex = 0;

            strength = 0;

            iterationStep.Combine();
            AddNewStep();
        }

        public void TerminatePropagation()
        {
            enabled = false;
            Events.OnPropagationStep.Unregister(iterationStep.InitialPropagator);

            Events.OnPropagationStepEnded.Call();
        }

        private void Update()
        {
            for (int i = 0; i < gateSetStep.Count; i++)
            {
                UpdateStep(gateSetStep[i]);
                gateSetStep[i].timer.Step(Time.deltaTime);
            }
        }

        private void AddNewStep()
        {
            if (++stepIndex >= iterationStep.MaxLengthPath)
                return;

            GateSet set = new(iterationStep.CombinedPaths[stepIndex]);

            if (set.Cost > iterationStep.InitialPropagator.CurrentStrength)
                return;

            set.timer.AddEvent(0.8f, AddNewStep);
            set.timer.AddEvent(1.0f, RemoveLastStep);

            gateSetStep.Add(set);

            foreach (var gate in set.gates)
            {
                gate.Ownership.PreparePropagation(TurnManager.CurrentPlayer, gate.ForwardTraversalDir);
                if (gate.Ownership.ComputesInPropagation && !gate.Ownership.IsSameOwner(TurnManager.CurrentPlayer))
                {
                    ShowStrength(gate);
                    Events.OnPropagationStep.Call();
                }
            }
        }

        private void RemoveLastStep()
        {
            foreach (var gate in gateSetStep[0].gates)
                gate.Ownership.FinalizePropagation();

            gateSetStep.RemoveAt(0);

            if (gateSetStep.Count == 0)
                TerminatePropagation();
        }

        private void UpdateStep(GateSet setStep)
        {
            for (int i = 0; i < setStep.gates.Length; i++)
                setStep.gates[i].Ownership.UpdatePropagation(setStep.timer.NormalizedTime);
        }

        private void ShowStrength(Gate.ReadOnlyGate gate)
        {
            strength++;
            StrengthIndicatorCanvas canvas = Game.Instance.GetSystem<StrengthIndicatorCanvas>();
            canvas.ShowWithCountdown($"+{strength}", gate.IndicatorPos, 3f);
            //var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //go.transform.localScale *= 0.1f;
            //go.transform.position = gate.IndicatorPos;
        }

        public static class Events
        {
            public readonly static EventType OnPropagationStep = new();         // Each propagation step (set of gates) completed. Reduce strength of TilePropagator
            public readonly static EventType OnPropagationStepEnded = new();    // When a complete propagation for a initial TilePropagator ends
            public readonly static EventType OnPropagationEnded = new();        // When there's no more pending propagations

            public readonly static EventTypeArg<OnSegmentPropagatedArgs> OnSegmentPropagated = new();

            public static void Clear()
            {
                OnPropagationStep.Clear();
                OnPropagationStepEnded.Clear();
                OnPropagationEnded.Clear();
                OnSegmentPropagated.Clear();
            }
        }
    }
}
