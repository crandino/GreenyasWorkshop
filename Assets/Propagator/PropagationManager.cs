using Cysharp.Threading.Tasks;

using System.Collections.Generic;
using static HexaLinks.Path.Finder.PathFinder.Path;

namespace HexaLinks.Propagation
{
    using Ownership;
    using Tile.Events;
    using Turn;

    public class PropagationManager : Game.IGameSystem
    {
        private List<List<Link[]>> unifiedPaths;

        public void InitSystem()
        {
            unifiedPaths = new List<List<Link[]>>();
        }

        public void Start(List<Link[]> path)
        {
            unifiedPaths.Add(path);

            if (Propagating)
                return;

            TriggerPropagation();           
        }

        public bool Propagating { private set; get; }

        private async void TriggerPropagation()
        {
            Propagating = true;

            for (int i = 0; i < unifiedPaths.Count; i++)
            {
                //Owner propagationOwner = ((InitialPlayerOwnership)unifiedPaths[i][0][0].Ownership).Owner;
                Owner propagationOwner = Game.Instance.GetSystem<TurnManager>().CurrentPlayer;
                SetNewOwnershipAlongPath(propagationOwner, unifiedPaths[i]);
                await UpdatePropagation(unifiedPaths[i]);
                TileEvents.OnPropagationStep.Call(null);
            }

            unifiedPaths.Clear();
            Propagating = false;

            TileEvents.OnPropagationEnded.Call(null);
        }

        private static void SetNewOwnershipAlongPath(Owner newOwner, List<Link[]> unifiedPath)
        {
            foreach (Link[] pathLinks in unifiedPath)
            {
                foreach (Link c in pathLinks)
                   c.Ownership.PrepareOwnerChange(newOwner);
            }
        }

        private async static UniTask UpdatePropagation(List<Link[]> unifiedPath)
        {
            foreach (Link[] pathLinks in unifiedPath)
            {
                List<UniTask> tasks = new();

                foreach (Link c in pathLinks)
                    tasks.Add(c.Ownership.UpdatePropagation(c.ForwardTraversal));

                await UniTask.SwitchToMainThread();
                await UniTask.WhenAll(tasks);
            }
        }
    }
}
