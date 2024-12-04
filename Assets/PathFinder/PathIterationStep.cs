using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static HexaLinks.Path.Finder.PathFinder.Path;

namespace HexaLinks.Path.Finder
{
    public static partial class PathFinder
    {
        private class PathIterationStep
        {
            public int id;
            public List<Path> paths;

            private readonly List<uint> stepUsedHashes = new();

            public PathIterationStep(int id)
            {
                this.id = id;
                paths = new List<Path>();
            }

            public void Add(Path newPath)
            {
                paths.Add(newPath);
                stepUsedHashes.Add(newPath.HashID);

                Debug.Log($"On step {id}, added {newPath}");
            }

            /*
             * TODO: Que hacemos con los hashes? Un nuevo propagator puede arrancar de nuevo el camino 
             * inverso ya hecho. 
             * EL InitialPlayerOwnership debería suscribir eventos en los PlayerOwnership y que el camino no llegue al centro.
             * Si que salga de él, pero no llegue. Que sean los PlayerOwnership de alrededor los que disparen una nueva
             * búsqueda. 
             * Los Hashes, echar un ojo a esto a ver si da ideas:
             * https://en.wikibooks.org/wiki/A-level_Computing/AQA/Paper_1/Fundamentals_of_data_structures/Hash_tables_and_hashing
             */

            public List<Link[]> UnifyPaths()
            {
                if (paths.Count == 0)
                    return null;

                int longestPath = paths.Max(p => p.Links.Length);
                List<Link>[] upath = new List<Link>[longestPath];

                for (int i = 0; i < upath.Length; ++i)
                    upath[i] = new List<Link>();

                for (int i = 0; i < paths.Count; ++i)
                {
                    for (int j = 0; j < paths[i].Links[paths[i].PropagationRange].Length; ++j)
                    {
                        if (paths[i].Equals(paths[i].Links[j]))
                            break;

                        upath[j].Add(paths[i].Links[j]);
                    }
                }

                return upath.Select(p => p.Distinct(new PathLinkComparer()).ToArray()).ToList();
            }

            class PathLinkComparer : IEqualityComparer<Link>
            {
                public bool Equals(Link x, Link y)
                {
                    return x.Ownership == y.Ownership && x.ForwardTraversal == y.ForwardTraversal;
                }

                public int GetHashCode(Link product)
                {
                    return product.Ownership.GetHashCode();
                }
            }
        }
    }

}