using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using static Hexalinks.PathFinder.PathStorage;
using static Hexalinks.PathFinder.PathStorage.Path;

public static class OwnershipPropagation
{
    private static List<PathLink[]> unifiedPath;

    private static PlayerOwnership.Ownership owner;

    public readonly struct PropagationData
    {
        public readonly PlayerOwnership.Ownership newOwner;
        public readonly bool forwardTraversalDirection;

        public PropagationData(PlayerOwnership.Ownership owner, bool propagationDirection)
        {
            newOwner = owner;
            forwardTraversalDirection = propagationDirection;
        }
    }

    public static void Start(List<PathLink[]> path)
    {
        unifiedPath = path;
        owner = unifiedPath[0][0].Ownership.Owner;

        PreparePropagation();
        TriggerPropagation();
    }

    private async static void TriggerPropagation()
    {
        foreach(Path.PathLink[] pathLinks in unifiedPath)
        {
            List<UniTask> tasks = new();

            foreach (Path.PathLink c in pathLinks)
            {
                tasks.Add(c.Ownership.GraduallyOwnerChange());
            }

            await UniTask.SwitchToMainThread();
            await UniTask.WhenAll(tasks);
        }       
    }

    private static void PreparePropagation()
    {
        foreach(var p in unifiedPath)
        {
            foreach(var c in p)
                c.Ownership.Prepare(new PropagationData(owner, c.entryGate.ForwardTraversalDir));

        }
    }
}
