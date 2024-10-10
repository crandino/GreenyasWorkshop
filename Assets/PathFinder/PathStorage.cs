using Greenyas.Hexagon;
using Hexalinks.Tile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Hexalinks.PathFinder.PathStorage.Path;

namespace Hexalinks.PathFinder
{
    public static class PathStorage
    {
        private class PropagationStep
        {
            public int id;
            public List<Path> paths;
            public UnifiedPath unifiedPath;

            public PropagationStep(int id)
            {
                this.id = id;
                paths = new List<Path>();
            }

            public UnifiedPath UnifyPaths()
            {
                unifiedPath = new(paths);
                return unifiedPath;
            }
        }

        private readonly static List<PropagationStep> steps = new List<PropagationStep>();
        private static PropagationStep current = null;

        private static List<uint> oldPathsHashes = new();

        public static void InitNewPropagation(bool fullReset = false)
        {
            if (fullReset)
                ResetPropagationSteps();

            current = new PropagationStep(steps.Count);
            steps.Add(current);
        }

        private static void ResetPropagationSteps()
        {
            steps.Clear();
            current = null;
        }

        public static void Add(Path newPath)
        {
            if(!oldPathsHashes.Contains(newPath.HashID))
            {
                current.paths.Add(newPath);
                oldPathsHashes.Add(newPath.HashID);
                newPath.Log();
            }
        }

        public static void StartPropagation()
        {
            //RemoveIdenticalPaths();
            OwnershipPropagation.Start(current.UnifyPaths());
        }

        //private static void RemoveIdenticalPaths()
        //{
        //    foreach(PropagationStep step in steps)
        //    {
        //        if (step.id == current.id)
        //            continue;

        //        for(int i = step.paths.Count - 1; i >= 0; i--)
        //        {
        //            for (int j = current.paths.Count - 1; j >= 0; j--)
        //            {
        //                if (step.paths[i].HashID == current.paths[j].HashID)
        //                    current.paths.RemoveAt(j);
        //            }
        //        }
        //    }
        //}

        public class UnifiedPath
        {
            public List<PathLink[]> unifiedPath;

            public UnifiedPath(List<Path> paths)
            {
                int longestPath = paths.Max(p => p.Links.Length);
                List<PathLink>[] upath = new List<PathLink>[longestPath];

                for (int i = 0; i < upath.Length; ++i)
                {
                    upath[i] = new List<PathLink>();
                }                    

                for (int i = 0; i < paths.Count; ++i)
                {
                    for (int j = 0; j < paths[i].Links.Length; ++j)
                    {
                        upath[j].Add(paths[i].Links[j]);
                    }
                }

                unifiedPath = new List<PathLink[]>();
                unifiedPath = upath.Select(p => p.Distinct(new PathLinkComparer()).ToArray()).ToList();
            }

            class PathLinkComparer : IEqualityComparer<PathLink>
            {
                public bool Equals(PathLink x, PathLink y)
                {
                    return x.Ownership == y.Ownership;              
                }

                public int GetHashCode(PathLink product)
                {
                    return product.Ownership.GetHashCode();
                }
            }
        }


        public class Path
        {
            public uint HashID { private set; get; } = 0;

            public readonly struct PathLink 
            {
                public readonly Gate.ExposedGate entryGate;
                public readonly PlayerOwnership Ownership => entryGate.Ownership;

                public PathLink(Gate.ExposedGate gate)
                {
                    entryGate = gate;
                }               
            }

            public PathLink[] Links { private set; get; }
          
            public Path(Gate.ExposedGate[] gates)
            {
                Links = gates.Select(s => new PathLink(s)).ToArray();
                Array.ForEach(Links, link =>
                {
                    CubeCoord coord = link.Ownership.transform.GetTransformUpUntil<Tile.Tile>().GetComponent<Tile.Tile>().Coord;
                    HashID += (uint)((coord.Q * coord.Q) + (coord.Q * coord.Q) * 3 +
                                     (coord.R * coord.R) * 13 + (coord.R * coord.R) * 4+ 
                                     (coord.S * coord.S) * 7 + (coord.S * coord.S) * 2);
                });
            }

            public void Log()
            {
                int counter = 1;
                string text = $"Path ID: {HashID}\n";

                foreach (var link in Links)
                    text += $"{counter++} - {link.Ownership.transform.parent.name}:{link.Ownership.name}\n";

                Debug.Log(text);
            }      
        }
    }

}