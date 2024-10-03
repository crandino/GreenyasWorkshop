using Greenyas.Hexagon;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using static Greenyas.Hexagon.HexSide;
#if UNITY_EDITOR
using UnityEditor;
using Hexagon.Tile.Debug; 
#endif

namespace Hexalinks.Tile
{
    [System.Serializable]
    public class SideGate : Gate
    {
        [SerializeField]
        private HexSide hexSide = new();

        public Side WorldSide => hexSide.GetWorldSide(Segment);
       
        public void TryConnect(SideGate againstGate)
        {
            // There's any previous connection between those gates
            if (outwardGates.Contains(againstGate) && againstGate.outwardGates.Where(g => g == againstGate).Count() == 0)
                return;

            if (IsFacingOtherGate(againstGate))
            {
                outwardGates.Add(againstGate);
                againstGate.outwardGates.Add(this);
            };
        }

        private bool IsFacingOtherGate(SideGate gateTo)
        {
            Assert.IsTrue(Segment != gateTo.Segment);
            return WorldSide.IsOpposite(gateTo.WorldSide);
        }

#if UNITY_EDITOR

        public override Vector3 WorldDebugPos
        {
            get
            {
                Vector2 localDir = CubeCoord.GetVectorToNeighborHexOn(WorldSide);
                Vector3 localPosition = new Vector3(localDir.x, 0.05f, localDir.y);
                return Segment.transform.position + localPosition * HexTools.hexagonSize;
            }
        }

        public override void DrawDebugInfo()
        {
            base.DrawDebugInfo();

            // Outward node conntections
            if (TileDebugOptions.Instance.showConnections && Application.isPlaying)
            {
                Vector2 vec = CubeCoord.GetVectorToNeighborHexOn(WorldSide);
                Vector3 toNextTile = new Vector3(vec.x, 0f, vec.y);
                Quaternion arrowOrientatinon = Quaternion.LookRotation(toNextTile);

                GUIStyle textStyle = new GUIStyle();
                textStyle.fontSize = 18;
                Handles.color = textStyle.normal.textColor = outwardGates.Count != 0 ? Color.green : Color.red;
                Handles.Label(WorldDebugPos + toNextTile * 0.3f, $"{outwardGates.Count}", textStyle);

                Handles.ArrowHandleCap(0, WorldDebugPos, arrowOrientatinon, 0.2f, EventType.Repaint);
            }
        }
    }
#endif
}