using Cysharp.Threading.Tasks;

using System.Collections.Generic;
using static HexaLinks.Path.Finder.PathFinder;

namespace HexaLinks.Propagation
{
    using HexaLinks.Tile;
    using Ownership;
    using System.Linq;
    using Tile.Events;
    using Turn;

    public class PropagationManager : Game.IGameSystem
    {
        public void InitSystem()
        {
            //Nothing here!            
        }       

        public async void TriggerPropagation(PathIterationStep iterationStep)
        {
            iterationStep.Combine();
            await UpdatePropagation(iterationStep);
            
            TileEvents.OnPropagationStepEnded.Call(null);           
        }       

        private async static UniTask UpdatePropagation(PathIterationStep step)
        {
            Owner newOwner = Game.Instance.GetSystem<TurnManager>().CurrentPlayer;

            foreach (Gate.ReadOnlyGate[] pathGates in step.CombinedPaths)
            {
                List<UniTask<bool>> tasks = new();

                foreach (Gate.ReadOnlyGate c in pathGates)
                    tasks.Add(c.Ownership.UpdatePropagation(newOwner, c.ForwardTraversalDir));

                await UniTask.SwitchToMainThread();
                bool[] successfulPropagations = await UniTask.WhenAll(tasks);                

                if (successfulPropagations.Any(x => x))
                {
                    UnityEngine.Debug.Log("DEcreasing");
                    TileEvents.OnPropagationStep.Call(step.Precursor, null);
                }
            }

            TileEvents.OnPropagationStep.UnregisterCallbacks(step.Precursor);
        }
    }
}
