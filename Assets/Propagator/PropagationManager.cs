using Cysharp.Threading.Tasks;

using System.Collections.Generic;
using static HexaLinks.Path.Finder.PathFinder;
using static HexaLinks.Path.Finder.PathFinder.Path;

namespace HexaLinks.Propagation
{
    using Ownership;
    using Tile.Events;
    using Turn;

    public class PropagationManager : Game.IGameSystem
    {
        private List<PathIterationStep> stepsToPropagate;

        public void InitSystem()
        {
            stepsToPropagate = new();
        }

        public void Start(PathIterationStep iterationStep)
        {
            stepsToPropagate.Add(iterationStep);

            if (Propagating)
                return;

            TriggerPropagation();           
        }

        public bool Propagating { private set; get; }

        private async void TriggerPropagation()
        {
            Propagating = true;

            for (int i = 0; i < stepsToPropagate.Count; i++)
            {
                PathIterationStep step = stepsToPropagate[i];
                step.Combine();

                SetNewOwnershipAlongPath(step);
                await UpdatePropagation(step);
                TileEvents.OnPropagationStep.UnregisterCallbacks(stepsToPropagate[i].PropagatorPrecursorTile);
            }

            stepsToPropagate.Clear();
            Propagating = false;

            TileEvents.OnPropagationEnded.Call(null);
        }

        private static void SetNewOwnershipAlongPath(PathIterationStep step)
        {
            Owner newOwner = Game.Instance.GetSystem<TurnManager>().CurrentPlayer;

            foreach (Link[] pathLinks in step.CombinedPaths)
            {
                foreach (Link c in pathLinks)
                   c.Ownership.PrepareOwnerChange(newOwner);
            }
        }

        private async static UniTask UpdatePropagation(PathIterationStep step)
        {
            foreach (Link[] pathLinks in step.CombinedPaths)
            {
                List<UniTask> tasks = new();

                foreach (Link c in pathLinks)
                    tasks.Add(c.Ownership.UpdatePropagation(c.ForwardTraversal));

                await UniTask.SwitchToMainThread();
                await UniTask.WhenAll(tasks);

                TileEvents.OnPropagationStep.Call(step.PropagatorPrecursorTile, null);
            }
        }
    }
}
