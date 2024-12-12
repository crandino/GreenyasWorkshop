using HexaLinks.Path.Finder.Tools;
using HexaLinks.Tile;
using HexaLinks.Tile.Events;
using System.Linq;

using Gate = HexaLinks.Tile.Gate.ExposedGate;

namespace HexaLinks.Path.Finder
{
    public static class PathIterator
    {
        public static void FindPathsFrom(TilePropagator initialTile)
        {
            TileStepTracker<Gate> gateTracker = new TileStepTracker<Gate>();

            Gate initialGate = initialTile.StartingGate;
            int maxPropagationSteps = initialTile.CurrentStrength;

            gateTracker.AddStep(initialGate);
            gateTracker.MoveNext();
            gateTracker.AddStep(initialGate.OutwardGates);

            while (gateTracker.MoveNext())
            {
                Gate currentGate = gateTracker.GetCurrentStep();

                if (currentGate.GoThrough(out Gate[] nextGates) && gateTracker.NumAccumulatedSteps <= maxPropagationSteps)
                    gateTracker.AddStep(nextGates);
                else
                    PathFinder.Add(new(gateTracker.GetEvaluatedSteps().ToArray()));
            }

            PathFinder.StartPropagation();
        }
    }
}
