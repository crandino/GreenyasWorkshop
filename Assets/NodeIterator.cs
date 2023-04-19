using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public static class NodeIterator
{
    private class StepTracker<T>
    {
        private readonly Stack<IterationStep<T>> iteratedStack = new Stack<IterationStep<T>>();

        private class IterationStep<T> : IEnumerator
        {
            private T[] set;
            private int currentStep;

            public IterationStep(params T[] set)
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
        Tile[] starterTiles = HexMap.Instance.GetAllStarterTiles();
        for (int i = 0; i < starterTiles.Length; i++)
        {
            ExplorePathsFrom(starterTiles[i]);
        }       
    }

    private static void ExplorePathsFrom(Tile tile)
    {
        StepTracker<Connection> tracker = new StepTracker<Connection>();

        Connection[] links = tile.GetAllConnections();

        for (int i = 0; i < links.Length; i++)
        {
            tracker.AddStep(links[i]);

            while (tracker.MoveNext())
            {
                Connection currentLink = tracker.GetCurrentStep();
                if (currentLink.IsStarter)
                {
                    Debug.Log("One via is closed!");
                    Connection[] sequence = tracker.GetEvaluatedSteps();
                }
                else
                {
                    Connection[] nextConnections = currentLink.GoThrough();
                    tracker.AddStep(nextConnections);
                }
            }
        }
    }
}
