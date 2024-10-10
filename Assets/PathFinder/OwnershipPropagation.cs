using Cysharp.Threading.Tasks;
using Hexalinks.PathFinder;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Hexalinks.PathFinder.PathStorage;

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
        owner = currentPath.unifiedPath[0][0].Ownership.Owner;

        PreparePropagation();
        TriggerPropagation();
    }

    private async static void TriggerPropagation()
    {
        foreach(Path.PathLink[] pathLinks in currentPath.unifiedPath)
        {
            //Path.PathLink[] current = currentPath.unifiedPath[i];

            //string log = "";
            //foreach(var x in currentPath.unifiedPath)
            //{
            //    x.Select(x => log += x.Ownership.transform.parent.name);
            //}

            //Debug.Log(log);

            List<UniTask> tasks = new();

            foreach (Path.PathLink c in pathLinks)
            {
                //Debug.Log(c.Ownership.transform.parent.name);
                tasks.Add(c.Ownership.GraduallyOwnerChange());
            }

            await UniTask.SwitchToMainThread();
            await UniTask.WhenAll(tasks);
        }       
    }

    private static void PreparePropagation()
    {
        foreach(var p in currentPath.unifiedPath)
        {
            foreach(var c in p)
                c.Ownership.Prepare(new PropagationData(owner, c.entryGate.ForwardTraversalDir));

        }
    }
}
