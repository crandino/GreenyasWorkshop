using Hexalinks.Tile;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tripolygon.UModeler.Models;
using Tripolygon.UModelerX.Runtime.MessagePack.Resolvers;
using UnityEngine;
using static Hexalinks.PathFinder.PathStorage.Path;

namespace Hexalinks.PathFinder
{
    public static class PathStorage
    {
        private static List<Path> paths = new List<Path>();

        public struct UnifiedPath
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
                unifiedPath = upath.Select(p => p.Distinct(new ProductComparer()).ToArray()).ToList();
            }

            class ProductComparer : IEqualityComparer<PathLink>
            {
                // Products are equal if their names and product numbers are equal.
                public bool Equals(PathLink x, PathLink y)
                {
                    return x.ownership == y.ownership;
                    
                    ////Check whether the compared objects reference the same data.
                    //if (Object.ReferenceEquals(x, y)) return true;

                    ////Check whether any of the compared objects is null.
                    //if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    //    return false;

                    ////Check whether the products' properties are equal.
                    //return x.Code == y.Code && x.Name == y.Name;
                }

                // If Equals() returns true for a pair of objects
                // then GetHashCode() must return the same value for these objects.

                public int GetHashCode(PathLink product)
                {
                    return product.ownership.GetHashCode();

                    ////Check whether the object is null
                    //if (Object.ReferenceEquals(product, null)) return 0;

                    ////Get hash code for the Name field if it is not null.
                    //int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();

                    ////Get hash code for the Code field.
                    //int hashProductCode = product.Code.GetHashCode();

                    ////Calculate the hash code for the product.
                    //return hashProductName ^ hashProductCode;
                }
            }


        }

        public static void Clear()
        {
            paths.Clear();
        }

        public static void Add(Path newPath)
        {
            paths.Add(newPath);
        }

        public static void UnifyPaths()
        {
            UnifiedPath path = new UnifiedPath(paths);
            OwnershipPropagation.Start(path);
        }

        public class Path : IEnumerable<PathLink>
        {
            private readonly PathEnumerator enumerator;

            public readonly struct PathLink 
            {
                public readonly Gate.ExposedGate entryGate;
                public readonly PlayerOwnership ownership;

                public PathLink(Gate.ExposedGate gate)
                {
                    entryGate = gate;
                    ownership = gate.Ownership;
                }               
            }

            class PathEnumerator : IEnumerator<PathLink>
            {
                private readonly Path path;
                private int iterIndex = -1;

                public PathEnumerator(Path path)
                {
                    this.path = path;
                }

                public PathLink Current => path.Links[iterIndex];

                object IEnumerator.Current => path.Links[iterIndex];

                public void Dispose()
                {
                    throw new System.NotImplementedException();
                }

                public bool MoveNext()
                {
                    return ++iterIndex < path.Links.Length;                   
                }

                public void Reset()
                {
                    iterIndex = -1;
                }
            }

            public PathLink[] Links { private set; get; }
          
            public Path(Gate.ExposedGate[] gates)
            {
                Links = gates.Select(s => new PathLink(s)).ToArray();
                enumerator = new PathEnumerator(this);
            }

            //public void TriggerContamination()
            //{

                

            //    //OwnershipPropagation.Start(this);

            //    //PlayerOwnership.Ownership initialOwner = pathLinks[0].ownership.Owner;

            //    //foreach (var link in pathLinks)
            //    //{
            //    //    link.ownership.OwnerChange(initialOwner);
            //    //}
            //}

  

            public void Log()
            {
                int counter = 1;

                foreach (var link in Links)
                {
                    Debug.Log($"{counter++} - {link.ownership.transform.parent.name}:{link.ownership.name}");
                }
            }

            public IEnumerator<PathLink> GetEnumerator()
            {
                return enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerator;
            }           
        }
    }

}