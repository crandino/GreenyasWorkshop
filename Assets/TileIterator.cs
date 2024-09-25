using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine;
using Hexalinks.Tile;

using Gate = Hexalinks.Tile.Gate.ExposedGate;

public static class TileIterator
{
    private class StepTracker<T>
    {
        private readonly Stack<IterationStep<T>> iteratedStack = new Stack<IterationStep<T>>();

        private class IterationStep<S> : IEnumerator
        {
            private S[] set;
            private int currentStep;

            public IterationStep(params S[] set)
            {
                this.set = set;
                currentStep = -1;
            }

            public object Current
            {
                get
                {
                    Assert.IsTrue(currentStep >= 0, "Use MoveNext() first before accessing the current Link");
                    return set[currentStep];
                }
            }

            public bool MoveNext()
            {
                ++currentStep;
                return currentStep < set.Length;
            }

            public void Reset()
            {
                currentStep = -1;
            }
        }

        public void AddStep(params T[] stepSet)
        {
            if (stepSet.Length == 0)
                return;

            iteratedStack.Push(new IterationStep<T>(stepSet));
        }

        public T GetCurrentStep()
        {
            return (T)iteratedStack.Peek().Current;
        }

        public bool Empty => iteratedStack.Count == 0;

        public bool MoveNext()
        {
            while (!Empty)
            {
                if (iteratedStack.Peek().MoveNext())
                    return true;

                iteratedStack.Pop();
            }

            return false;
        }

        public T[] GetEvaluatedSteps()
        {
            IterationStep<T>[] steps = new IterationStep<T>[iteratedStack.Count];
            iteratedStack.CopyTo(steps, 0);
            return steps.Select(s => (T)s.Current).ToArray();
        }
    }

    public static void LookForClosedPaths()
    {
        //Tile[] starterTiles = HexMap.Instance.GetAllStarterTiles();

        //for (int i = 0; i < starterTiles.Length; i++)
        //    ExplorePathsFrom(starterTiles[i]);
        
        //PathStorage.ShowCompletedPaths();
    }

    public static void ExplorePathsFrom(TileFiller filler)
    {
        StepTracker<Gate> tracker = new StepTracker<Gate>();
        HashSet<Gate> visited = new HashSet<Gate>();

        Gate[] gates = filler.Gates;

        for (int i = 0; i < gates.Length; i++)
        {
            tracker.AddStep(gates[i].OutwardGates);

            while (tracker.MoveNext())
            {
                Gate currentGate = tracker.GetCurrentStep();
                if (visited.Contains(currentGate))
                {
                    Debug.Log($"Function exits with {visited.Count} tiles visited");
                    break;
                }

                visited.Add(currentGate);
                if (currentGate.IsFiller)
                {

                    Debug.Log($"Function exits with {visited.Count} tiles visited");

                    //TileSegment[] path = tracker.GetEvaluatedSteps().
                    //                     Select(g => g.Segment).
                    //                     Append(gates[i].Segment).ToArray();

                    //PathStorage.AddPath(path);
                }
                else
                {
                    Gate[] nextGates = currentGate.GoThrough();
                    tracker.AddStep(nextGates);
                }
            }
        }

        // Gate.Pool.Release(gates);
    }
}
