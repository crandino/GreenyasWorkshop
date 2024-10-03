using Hexalinks.PathFinder.Tools;
using System.Linq;

using Gate = Hexalinks.Tile.Gate.ExposedGate;

namespace Hexalinks.PathFinder
{
    public static class PathIterator
    {
       

        public static void ExplorePathsFrom(Tile.Tile startingTile)
        {
            TileStepTracker<Gate> gateTracker = new TileStepTracker<Gate>();
            PathStorage.Clear();

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
                        PathStorage.Path path = new(gateTracker.GetEvaluatedSteps().
                                             Select(g => g.Segment)
                                            .ToArray());
                        path.Log();
                        //path.TriggerContamination();
                    }
                }
            }
        }
    } 
}
