using Hexalinks.PathFinder.Tools;
using System.Linq;

using Gate = Hexalinks.Tile.Gate.ExposedGate;

namespace Hexalinks.PathFinder
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
                    {
                        PathStorage.Path path = new(gateTracker.GetEvaluatedSteps()
                                            .ToArray());
                        

                        PathStorage.Add(path);
                    }
                }
            }

            PathStorage.StartPropagation();
        }
    } 
}
