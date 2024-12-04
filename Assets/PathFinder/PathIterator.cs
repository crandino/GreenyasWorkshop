using HexaLinks.Path.Finder.Tools;
using HexaLinks.Tile;
using System.Linq;

using Gate = HexaLinks.Tile.Gate.ExposedGate;

namespace HexaLinks.Path.Finder
{
    public static class PathIterator
    {
        public static void FindPathsFrom(TilePropagator initialTile)
        {
            TileStepTracker<Gate> gateTracker = new TileStepTracker<Gate>();

            Gate[] initialGates = initialTile.StartingGates;
            int maxPropagationSteps = initialTile.currentPropagatorStrength;

            for (int i = 0; i < initialGates.Length; i++)
            {
                gateTracker.AddStep(initialGates);
                gateTracker.MoveNext();

                Gate currentGate = gateTracker.GetCurrentStep();
                gateTracker.AddStep(currentGate.OutwardGates);

                while (gateTracker.MoveNext())
                {
                    currentGate = gateTracker.GetCurrentStep();

                    if (currentGate.GoThrough(out Gate[] nextGates) && gateTracker.NumAccumulatedSteps <= maxPropagationSteps )
                        gateTracker.AddStep(nextGates);
                    else
                        PathFinder.Add(new(gateTracker.GetEvaluatedSteps().ToArray()));
                }
            }

            PathFinder.StartPropagation();
        }
    } 
}
