using UnityEditor;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using Hexagon.Tile.Debug;
#endif

namespace Hexalinks.Tile
{
    public class TraversalSegment : TileSegment
    {
        [SerializeField]
        private Gate[] gates = new Gate[0];

        public override Gate[] AllGates => gates;
        protected override SideGate[] SideGates => AllGates.Where(g => g is SideGate).Cast<SideGate>().ToArray();

        public override Gate GoThrough(Gate enterGate)
        {
            return AllGates.Where(g => g != enterGate).First();
        }

#if UNITY_EDITOR

        [ContextMenu("Create Traversal Segment")]
        private void CreateTraversalSegment()
        {
            gates = gates.Append(CreateSideGate()).ToArray();
            gates = gates.Append(CreateSideGate()).ToArray();
        }

        [ContextMenu("Create Bridge Segment")]
        private void CreateBridgeSegment()
        {
            gates = gates.Append(CreateGate()).ToArray();
            gates = gates.Append(CreateSideGate()).ToArray();
        }       

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (TileDebugOptions.Instance.showSegments)
            {
                Handles.color = CustomColors.darkOrange;
                Vector3 arrowDir = (AllGates[1].WorldDebugPos - AllGates[0].WorldDebugPos);

                Handles.color = CustomColors.purple;
                Handles.ArrowHandleCap(0, AllGates[0].WorldDebugPos, Quaternion.LookRotation(arrowDir.normalized, Vector3.up), arrowDir.magnitude * 0.9f, EventType.Repaint);
            }
        }
#endif
    }
}