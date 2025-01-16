using System.Collections.Generic;
using System.Linq;
using ReadOnlyGate = HexaLinks.Tile.Gate.ReadOnlyGate;

namespace HexaLinks.Path.Finder
{
    using Path.Finder.Tools;
    using static Path.Finder.PathFinder;
    using Propagation;
    using static Propagation.PropagationManager.Events;
    using Tile;
    using Tripolygon.UModelerX.Runtime.MessagePack.Formatters;

    public class PathIterator : Game.IGameSystem
    {
        public void InitSystem()
        {
            OnPropagationStepEnded.Register(TriggerSearch);
        }

        public void TerminateSystem()
        {
            OnPropagationStepEnded.Unregister(TriggerSearch);
            searches.Clear();
        }

        private readonly Searches searches = new Searches();        

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
        
        public void QueueSearch(TilePropagator tile)
        {
            searches.AddSearch(tile);
        }

        public void TriggerSearch()
        {
            if (!searches.ArePendingSearches)
            {
                StopSearch();
                return;
            }

            PathIterationStep step = searches.GetSearch();
            step.Precursor.ShowPropagationEvolution();

            TileStepTracker<ReadOnlyGate> gateTracker = new TileStepTracker<ReadOnlyGate>();

            UnityEngine.Debug.Log($"Searching paths for {step.Precursor}");

            ReadOnlyGate initialGate = step.Precursor.StartingGate;
            //int maxPropagationSteps = step.Precursor.CurrentStrength;
            
            // Needed propagator gate sequence to move to the next tile
            gateTracker.AddStep(initialGate);
            gateTracker.MoveNext();
            gateTracker.AddStep(initialGate.OutwardGates);

            // Rest of the gate sequences
            while (gateTracker.MoveNext())
            {
                ReadOnlyGate currentGate = gateTracker.GetCurrentStep();

                //int propagationStrengthUsed = gateTracker.NumAccumulatedSteps(o => o.Ownership.ComputesInPropagation);
                Path currentPath = new Path(gateTracker.GetEvaluatedSteps().ToArray());

                if (currentGate.GoThrough(out ReadOnlyGate[] nextGates) && currentPath.links.All(p => nextGates.All(n => n != p)))
                    gateTracker.AddStep(nextGates);
                else
                {
                    //Path newPath = new Path(gateTracker.GetEvaluatedSteps().ToArray());
                    if(currentPath.links.Length > 2)
                        step.AddPath(currentPath);

                    if (currentGate.IsEnd)
                        QueueSearch(currentGate.Propagator);
                }
            }

            searches.Archive(step);

            if (step.ExistsPropagation())
                Game.Instance.GetSystem<PropagationManager>().TriggerPropagation(step);
            else
                TriggerSearch();
        }

        private void StopSearch()
        {
            OnPropagationEnded.Call();
            searches.Clear();
        }       
    }
}
