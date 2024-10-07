using Hexalinks.PathFinder;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Hexalinks.PathFinder.PathStorage;
using Cysharp.Threading.Tasks;
using System.Collections;

public static class OwnershipPropagation
{
    private static UnifiedPath currentPath;
    //private static Path.PathLink current;

    private static int linkSteps = 0;
    private static int currentStep = 0;

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

    public static void Start(PathStorage.UnifiedPath path)
    {
        currentPath = path;
        //linkSteps = path.Links.Length;

        owner = currentPath.unifiedPath[0][0].ownership.Owner;

        PreparePropagation();
        TriggerPropagation();
    }

    private async static void TriggerPropagation()
    {
        await UniTask.SwitchToMainThread();

        for (int i = 0; i < currentPath.unifiedPath.Count; ++i)
        {
            Path.PathLink[] current = currentPath.unifiedPath[i];

            List<UniTask> tasks = new();

            foreach (var c in current)
            {
                tasks.Add(c.ownership.OwnerChange(owner));
            }

            await UniTask.SwitchToMainThread();
            await UniTask.WhenAll(tasks);
        }       

        //List<Task> tasks = new();

        //foreach(var c in current)
        //{
        //    tasks.Add(c.ownership.OwnerChange(owner));
        //}

        //await Task.WhenAll(tasks);
        ////current.ownership.OwnerChange(owner, TriggerPropagation);
        ////Task task = new Task(() => { });
        ////Task.en
    }

    private static void PreparePropagation()
    {
        foreach(var p in currentPath.unifiedPath)
        {
            foreach(var c in p)
                c.ownership.Prepare(new PropagationData(owner, c.entryGate.ForwardTraversalDir));

        }
    }
}
