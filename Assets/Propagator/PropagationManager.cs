using Cysharp.Threading.Tasks;
using HexaLinks.Ownership;
using System.Collections.Generic;
using static HexaLinks.Path.Finder.PathFinder.Path;

namespace HexaLinks.Propagation
{
    public static class PropagationManager
    {
        private static List<List<Link[]>> unifiedPaths = new List<List<Link[]>>();

        public static void Start(List<Link[]> path)
        {
            unifiedPaths.Add(path);

            if (propagating)
                return;

            TriggerPropagation();           
        }

        private static bool propagating = false;

        private static async void TriggerPropagation()
        {
            propagating = true;

            for (int i = 0; i < unifiedPaths.Count; i++)
            {
                SetNewOwnershipAlongPath(((InitialPlayerOwnership)unifiedPaths[i][0][0].Ownership).Owner, unifiedPaths[i]);
                await UpdatePropagation(unifiedPaths[i]);
            }

            unifiedPaths.Clear();
            propagating = false;
        }

        private static void SetNewOwnershipAlongPath(PlayerOwnership.Ownership newOwner, List<Link[]> unifiedPath)
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
