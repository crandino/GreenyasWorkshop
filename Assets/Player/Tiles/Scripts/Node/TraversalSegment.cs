#if UNITY_EDITOR
using Hexagon.Tile.Debug;
#endif

using UnityEditor;
using System.Linq;
using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;
using static Hexalinks.Tile.TileConnectivity;

namespace Hexalinks.Tile
{
    public class TraversalSegment : TileSegment
    {
        [SerializeField]
        private SideGate[] sideGates;

        public override Gate[] AllGates => sideGates;
        protected override SideGate[] SideGates => AllGates.Cast<SideGate>().ToArray();

        //public override List<TileQueryResult> GetCandidates(CubeCoord fromCoord)
        //{
        //    List<TileQueryResult> candidates = new List<TileQueryResult>();

        //    for (int i = 0; i < AllGates.Length; ++i)
        //    {
        //        CubeCoord neighborHexCoord = fromCoord + CubeCoord.GetToNeighborCoord(SideGates[i].WorldSide);

        //        if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile neighborTileData))
        //            candidates.Add(new()
        //            {
        //                tile = neighborTileData,
        //                gate = SideGates[i]
        //            });
        //    }

        //    return candidates;
        //}

        public override Gate GoThrough(Gate enterGate)
        {
            return AllGates.Where(g => g != enterGate).First();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (TileDebugOptions.Instance.showSegments)
            {
                Handles.color = CustomColors.darkOrange;
                Handles.DrawLine(AllGates[0].WorldDebugPos, AllGates[1].WorldDebugPos, 2f);
            }
        }

#if UNITY_EDITOR

        protected override void InitializeGates()
        {
            sideGates = new[]
            {
                gameObject.AddComponent<SideGate>(), 
                gameObject.AddComponent<SideGate>()
            };
        }       
#endif

    }
}