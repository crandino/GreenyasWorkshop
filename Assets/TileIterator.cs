using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine;
using Hexalinks.Tile;

using Gate = Hexalinks.Tile.Gate.ExposedGate;
using static Hexalinks.Tile.TileSegment;

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
            return steps.Select(s => (T)s.Current).Reverse().ToArray();
        }
    }   

    public static void ExplorePathsFrom(Tile startingTile)
    {
        StepTracker<Gate> gateTracker = new StepTracker<Gate>();
        PathStorage.Clear();
        //HashSet<Gate> visited = new HashSet<Gate>();

        Gate[] initialGates = startingTile.Gates;

        for (int i = 0; i < initialGates.Length; i++)
        {
            //Gate initialGate = initialGates[i];
            gateTracker.AddStep(initialGates);

            while (gateTracker.MoveNext())
            {
                Gate currentGate = gateTracker.GetCurrentStep();
                gateTracker.AddStep(currentGate.GoThrough().OutwardGates);

                if(!(currentGate.GoThrough().IsOutterConnected || currentGate.IsLooped))
                {
                    PathStorage.Path path = new(gateTracker.GetEvaluatedSteps().
                                         Select(g => g.Segment)
                                        .ToArray());

                    //PathStorage.Add(path);

                    path.Log();
                    path.TriggerContamination();
                }

                //if (visited.Contains(currentGate))
                //{
                //    Debug.Log($"Function exits with {visited.Count} tiles visited");
                //    break;
                //}

                //visited.Add(currentGate);

                //Gate oppositeGate = currentGate.GoThrough();

                //if (currentGate.GoThrough().IsOutterConnected)
                //{
                //    gateTracker.AddStep(currentGate.GoThrough().OutwardGates);

                //    //Debug.Log($"Function exits with {visited.Count} tiles visited");



                //    //PathStorage.AddPath(path);
                //}
                //else
                //{

                //}


                //PathStorage.Path.CreatePath(initialGate, currentGate);
            }

            //TileSegment[] path = gateTracker.GetEvaluatedSteps().
            //                             Select(g => g.Segment)
            //                            .ToArray();

            //foreach (var p in path)
            //    Debug.Log(p.transform.parent.gameObject.name);
        }
    }
}
