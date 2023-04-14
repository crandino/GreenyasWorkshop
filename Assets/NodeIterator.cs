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

    public static void LookForClosedPaths(Link[] startingLinks)
    {
        StepTracker<Link> tracker = new StepTracker<Link>();

        for (int i = 0; i < startingLinks.Length; i++)
        {
            Link initialLinkPoint = startingLinks[i];         

            //if (initialLinkPoint.IsStarter)
            //{
            //    Debug.Log("One via is closed!");
            //}
            //if (initialLinkPoint.CurrentLinks.Length > 0)
            //    tracker.AddStep(initialLinkPoint.CurrentLinks);

            //while (tracker.MoveNext())
            //{
            //    Link linkPoint = tracker.GetCurrentStep();
            //    if (linkPoint.IsStarter)
            //    {
            //        Debug.Log("One via is closed!");
            //        Link[] links = tracker.GetEvaluatedSteps();
            //    }
            //    else
            //    {
            //        Link[] links = linkPoint.OppositeLinks;
            //        if(links.Length > 0)
            //            tracker.AddStep(links);
            //    }
            //}
        }
    }
}
