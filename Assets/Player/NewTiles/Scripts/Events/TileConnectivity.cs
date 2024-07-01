using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;
using static Hexalinks.Tile.Node;

namespace Hexalinks.Tile
{
    public class TileConnectivity : MonoBehaviour
    {
        [SerializeField]
        private TileSegment[] segments = null;

        public Gate[] Gates
        {
            get
            {
                List<Node.Gate> gates = new List<Gate>();

                for (int i = 0; i < segments.Length; i++)
                    gates.AddRange(segments[i].Gates);

                return gates.ToArray();
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < segments.Length; ++i)
                segments[i].Initialize();
        }

        public void ConnectTile(Tile tile)
        {
            for (int i = 0; i < Gates.Length; ++i)
            {
                Gate from = Gates[i];
                CubeCoord neighborHexCoord = tile.Coord + CubeCoord.GetToNeighborCoord(from.Node.Side);

                if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile neighborTileData))
                    from.Connect(neighborTileData.Gates);
            }
        }

        public void DisconnectTile()
        {
            for (int i = 0; i < Gates.Length; ++i)
                Gates[i].Disconnect();
        }

        [ContextMenu("Get Segments")]
        private void GetSegments()
        {
            segments = GetComponentsInChildren<TileSegment>();
        }
    }
}