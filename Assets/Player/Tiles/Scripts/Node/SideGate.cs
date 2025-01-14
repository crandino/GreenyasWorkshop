using Greenyas.Hexagon;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using HexaLinks.Tile.Extensions.Hexside;

using static Greenyas.Hexagon.HexSide;

#if UNITY_EDITOR
using UnityEditor;
using Hexagon.Tile.Debug;
#endif

namespace HexaLinks.Tile
{
    using Events;

    [System.Serializable]
    public class SideGate : Gate
    {
        [SerializeField]
        private HexSide hexSide = new();

        public Side WorldSide => hexSide.GetWorldSide(parentSegment);

        public override Vector3 WorldPos
        {
            get
            {
                Vector2 localDir = CubeCoord.GetVectorToNeighborHexOn(WorldSide);
                Vector3 localPosition = new Vector3(localDir.x, 0.05f, localDir.y);
                return parentSegment.transform.position + localPosition * HexTools.hexagonSize;
            }
        }

        public static void Connect(SideGate gateA, SideGate gateB)
        {
            gateA.outwardGates.Add(gateB);
            gateB.outwardGates.Add(gateA);

            //Events.OnSegmentConnected.Call();
        }

        public static void Disconnect(SideGate gate)
        {
            foreach (SideGate otherGate in gate.outwardGates)
                otherGate.outwardGates.Clear();

            gate.outwardGates.Clear();
        }

        public void GetAlignedGatesAgainst(SideGate gate, List<SideGate> alignedGates)
        {
            if (AreFacingEachOther(this, gate) && !AreConnected(this, gate))
                alignedGates.Add(this);
        }

        private static bool AreFacingEachOther(SideGate gateA, SideGate gateB)
        {
            Assert.IsTrue(gateA.parentSegment != gateB.parentSegment, "Error: We're on the same Tile");
            return gateA.WorldSide.IsOpposite(gateB.WorldSide);
        }

        private static bool AreConnected(SideGate gateA, SideGate gateB)
        {
            return gateA.outwardGates.Contains(gateB) && gateB.outwardGates.Contains(gateA);
        }   
        
        //public static class Events
        //{
        //    public readonly static EventType OnSegmentConnected = new();
        //    public readonly static EventType OnSegmentBlocked = new();
        //}

#if UNITY_EDITOR

        public override void DrawDebugInfo(Color tint)
        {
            base.DrawDebugInfo(tint);

            // Outward node conntections
            if (TileDebugOptions.Instance.showConnections && Application.isPlaying)
            {
                Vector2 vec = CubeCoord.GetVectorToNeighborHexOn(WorldSide);
                Vector3 toNextTile = new Vector3(vec.x, 0f, vec.y);
                Quaternion arrowOrientatinon = Quaternion.LookRotation(toNextTile);

                GUIStyle textStyle = new GUIStyle();
                textStyle.fontSize = 18;
                Handles.color = textStyle.normal.textColor = outwardGates.Count != 0 ? Color.green : Color.red;
                Handles.Label(WorldPos + toNextTile * 0.3f, $"{outwardGates.Count}", textStyle);

                Handles.ArrowHandleCap(0, WorldPos, arrowOrientatinon, 0.2f, UnityEngine.EventType.Repaint);
            }
        }
#endif
    }
}