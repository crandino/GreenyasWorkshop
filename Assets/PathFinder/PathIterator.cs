using HexaLinks.PathFinder.Tools;
using System.Linq;

using Gate = HexaLinks.Tile.Gate.ExposedGate;

namespace HexaLinks.PathFinder
{
    public static class PathIterator
    {
        public static void FindPathsFrom(Tile.Tile startingTile)
        {
            TileStepTracker<Gate> gateTracker = new TileStepTracker<Gate>();

            Gate[] initialGates = startingTile.StartingGates;

            for (int i = 0; i < initialGates.Length; i++)
            {
                gateTracker.AddStep(initialGates);
                gateTracker.MoveNext();

                Gate currentGate = gateTracker.GetCurrentStep();
                gateTracker.AddStep(currentGate.OutwardGates);

                while (gateTracker.MoveNext())
                {
                    currentGate = gateTracker.GetCurrentStep();

                    if (currentGate.GoThrough(out Gate[] nextGates))
                        gateTracker.AddStep(nextGates);
                    else
                        PathStorage.Add(new(gateTracker.GetEvaluatedSteps().ToArray()));
                }
            }

            PathStorage.StartPropagation();
        }
    } 
}
