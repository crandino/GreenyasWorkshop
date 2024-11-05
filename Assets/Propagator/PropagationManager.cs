using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using HexaLinks.Ownership;
using static HexaLinks.PathFinder.PathStorage;
using static HexaLinks.PathFinder.PathStorage.Path;
using UnityEditor;
using static UnityEngine.UI.GridLayoutGroup;

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

            propagating = false;
            unifiedPaths.Clear();
        }

        private static void SetNewOwnershipAlongPath(PlayerOwnership.Ownership newOwner, List<Link[]> unifiedPath)
        {
            foreach (Path.Link[] pathLinks in unifiedPath)
            {
                foreach (Path.Link c in pathLinks)
                   c.Ownership.PrepareOwnerChange(newOwner);
            }
        }

        private async static UniTask UpdatePropagation(List<Link[]> unifiedPath)
        {
            foreach (Path.Link[] pathLinks in unifiedPath)
            {
                List<UniTask> tasks = new();

                foreach (Path.Link c in pathLinks)
                    tasks.Add(c.Ownership.UpdatePropagation(c.ForwardTraversal));

                await UniTask.SwitchToMainThread();
                await UniTask.WhenAll(tasks);
            }
        }
    }
}
