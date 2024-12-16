using HexaLinks.Path.Finder.Tools;
using HexaLinks.Propagation;
using HexaLinks.Tile;
using HexaLinks.Tile.Events;
using System.Collections.Generic;
using System.Linq;
using static HexaLinks.Path.Finder.PathFinder;
using ReadOnlyGate = HexaLinks.Tile.Gate.ReadOnlyGate;

namespace HexaLinks.Path.Finder
{
    public static class PathIterator
    {
        static PathIterator()
        {
            TileEvents.OnPropagationStepEnded.RegisterCallback(TriggerSearch);
        }

        private readonly static Queue<PathIterationStep> pendingSearches = new();

        public static bool ArePendingSearches => pendingSearches.Count > 0;

        public static void QueueSearch(TilePropagator precursor)
        {
            if (!IsAlreadyProcessed(precursor))
                pendingSearches.Enqueue(new PathIterationStep(precursor));
        }

        public static void TriggerSearch(TileEvents.EmptyArgs? args)
        {
            if (pendingSearches.Count == 0)
            {
                TileEvents.OnPropagationEnded.Call(null);
                return;
            }

            PathIterationStep step = pendingSearches.Dequeue();
            step.Precursor.PreparePropagation();

            //List<TilePropagator> nexts = new List<TilePropagator>();
            TileStepTracker<ReadOnlyGate> gateTracker = new TileStepTracker<ReadOnlyGate>();

            ReadOnlyGate initialGate = step.Precursor.StartingGate;
            int maxPropagationSteps = step.Precursor.CurrentStrength;

            // Tile propagator gate sequence
            gateTracker.AddStep(initialGate);
            gateTracker.MoveNext();
            gateTracker.AddStep(initialGate.OutwardGates);

            // Rest of the gate sequence
            while (gateTracker.MoveNext())
            {
                ReadOnlyGate currentGate = gateTracker.GetCurrentStep();

                int propagationStrengthUsed = gateTracker.NumAccumulatedSteps(o => o.Ownership.ComputesInPropagation);

                if (currentGate.GoThrough(out ReadOnlyGate[] nextGates) && propagationStrengthUsed < maxPropagationSteps)
                    gateTracker.AddStep(nextGates);
                else
                {
                    PathFinder.Path newPath = new PathFinder.Path(gateTracker.GetEvaluatedSteps().ToArray());
                    if(newPath.links.Length > 2)
                        step.AddPath(newPath);

                    if (currentGate.IsEnd)
                        QueueSearch(currentGate.Propagator);
                }
            }

            if (step.PathsFound)
                Game.Instance.GetSystem<PropagationManager>().TriggerPropagation(step);
            else
                TileEvents.OnPropagationEnded.Call(null);

        }

        private static bool IsAlreadyProcessed(TilePropagator tile)
        {
            return pendingSearches.Any(o => o.Precursor == tile);
        }
    }
}
