using HexaLinks.Path.Finder.Tools;
using HexaLinks.Tile;
using System.Diagnostics;
using System.Linq;

using Gate = HexaLinks.Tile.Gate.ExposedGate;

namespace HexaLinks.Path.Finder
{
    public static class PathIterator
    {
        public static void FindPathsFrom(TilePropagator initialTile)
        {
            UnityEngine.Debug.Log("Starting new iteration");

            TileStepTracker<Gate> gateTracker = new TileStepTracker<Gate>();

            Gate initialGate = initialTile.StartingGate;
            int maxPropagationSteps = initialTile.currentPropagatorStrength;

            //for (int i = 0; i < initialGates.Length; i++)
            //{
            gateTracker.AddStep(initialGate);
            gateTracker.MoveNext();
            gateTracker.AddStep(initialGate.OutwardGates);
            //gateTracker.MoveNext();
            //Gate currentGate = gateTracker.GetCurrentStep();

            while (gateTracker.MoveNext())
            {
                Gate currentGate = gateTracker.GetCurrentStep();

                if (currentGate.GoThrough(out Gate[] nextGates) && gateTracker.NumAccumulatedSteps <= maxPropagationSteps)
                    gateTracker.AddStep(nextGates);
                else
                {
                    UnityEngine.Debug.Log("Adding new path");
                    PathFinder.Add(new(gateTracker.GetEvaluatedSteps().ToArray()));
                }
            }
            //}

            PathFinder.StartPropagation();
        }
    }
}
