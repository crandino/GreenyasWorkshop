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

        private readonly static Searches searches = new Searches();        

        private class Searches
        {
            private readonly Queue<PathIterationStep> pending = new();
            private readonly List<PathIterationStep> archive = new();

            public bool ArePendingSearches => pending.Count > 0;

            public PathIterationStep GetSearch() => pending.Dequeue();
                
            public void AddSearch(TilePropagator precursor)
            {
                if (!IsAlreadyProcessed(precursor))
                    pending.Enqueue(new PathIterationStep(precursor));
            }

            public void Archive(PathIterationStep step)
            {
                archive.Add(step);
            }

            public void Clear()
            {
                pending.Clear();
                archive.Clear();
            }

            private bool IsAlreadyProcessed(TilePropagator tile)
            {
                return pending.Any(o => o.Precursor == tile) || archive.Any(o => o.Precursor == tile);
            }
        }    
        
        public static void QueueSearch(TilePropagator tile)
        {
            searches.AddSearch(tile);
        }

        public static void TriggerSearch(TileEvents.EmptyArgs? args)
        {
            if (!searches.ArePendingSearches)
            {
                StopSearch();
                return;
            }

            PathIterationStep step = searches.GetSearch();
            step.Precursor.PreparePropagation();

            TileStepTracker<ReadOnlyGate> gateTracker = new TileStepTracker<ReadOnlyGate>();

            UnityEngine.Debug.Log($"Searching paths for {step.Precursor}");

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

            searches.Archive(step);

            if (step.ExistsPropagation())
                Game.Instance.GetSystem<PropagationManager>().TriggerPropagation(step);
            else
                TriggerSearch(null);
        }

        private static void StopSearch()
        {
            TileEvents.OnPropagationEnded.Call(null);
            searches.Clear();
        }       
    }
}
