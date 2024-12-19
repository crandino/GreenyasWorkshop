using System.Collections.Generic;
using static HexaLinks.Path.Finder.PathFinder;

namespace HexaLinks.Propagation
{
    using HexaLinks.Tile;
    using Ownership;
    using System.Linq;
    using Tile.Events;
    using Turn;
    using UnityEngine;

    public class PropagationManager : Game.GameSystemMonobehaviour
    {
        private PathIterationStep iterationStep;
        private readonly List<GateSet> gateSetStep = new();
        private int stepIndex = 0;

        private class GateSet
        {
            public Gate.ReadOnlyGate[] gates;
            public NormalizedTimer timer;
            public bool Computes;

            public GateSet(Gate.ReadOnlyGate[] gates)
            {
                this.gates = gates;
                timer = new NormalizedTimer(1f);
                Computes = gates.Any(o => o.Ownership.ComputesInPropagation);
            }
        }

        public override void InitSystem()
        { }
        
        public void TriggerPropagation(PathIterationStep iterationStep)
        {
            this.iterationStep = iterationStep;

            enabled = true;
            stepIndex = -1;

            iterationStep.Combine();
            AddNewStep();
        }

        public void TerminatePropagation()
        {
            enabled = false;
            TileEvents.OnPropagationStep.UnregisterCallbacks(iterationStep.Precursor);
            TileEvents.OnPropagationStepEnded.Call(null);
        }

        private void Update()
        {
            for(int i = 0; i < gateSetStep.Count; i++)
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
            set.timer.AddEvent(0.8f, AddNewStep);
            set.timer.AddEvent(1.0f, RemoveLastStep);

            foreach (var gate in set.gates)
                gate.Ownership.PreparePropagation(TurnManager.CurrentPlayer, gate.ForwardTraversalDir);

            if (set.Computes)
                TileEvents.OnPropagationStep.Call(iterationStep.Precursor, null);

            gateSetStep.Add(set);
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
    }
}
