using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using HexaLinks.Ownership;
using static Hexalinks.PathFinder.PathStorage;
using static Hexalinks.PathFinder.PathStorage.Path;

namespace HexaLinks.Propagation
{
    public static class PropagationManager
    {
        private static List<Link[]> unifiedPath;

        private static PlayerOwnership.Ownership owner;

        public readonly struct PropagationData
        {
            public readonly PlayerOwnership.Ownership newOwner;
            public readonly bool? forwardTraversalDirection;

            public PropagationData(PlayerOwnership.Ownership owner, bool? propagationDirection)
            {
                newOwner = owner;
                forwardTraversalDirection = propagationDirection;
            }
        }

        public static void Start(List<Link[]> path)
        {
            unifiedPath = path;
            owner = unifiedPath[0][0].Ownership.Owner;

            TriggerPropagation(owner);
        }

        private async static void TriggerPropagation(PlayerOwnership.Ownership owner)
        {
            foreach (Path.Link[] pathLinks in unifiedPath)
            {
                List<UniTask> tasks = new();

                foreach (Path.Link c in pathLinks)
                {
                    tasks.Add(c.Ownership.GraduallyOwnerChange(owner, c.ForwardPropagation));
                }

                await UniTask.SwitchToMainThread();
                await UniTask.WhenAll(tasks);
            }
        }
    }
}
