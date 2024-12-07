using Hexagon.Tile.Debug;
using HexaLinks.Ownership;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HexaLinks.Tile
{
    public class Gate : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        protected TileSegment parentSegment;

        [SerializeField]
        private Gate inwardGate;

        [SerializeReference]
        protected List<Gate> outwardGates = new List<Gate>();

        public bool IsEnd => inwardGate == null;
        public virtual Vector3 WorldPos => parentSegment.transform.position;

        public readonly struct ExposedGate
        {
            private readonly Gate gate;

            public readonly ExposedGate[] OutwardGates => gate.outwardGates.ToExposedGates();
            public readonly PlayerOwnership Ownership { get; }
            public readonly bool ForwardTraversalDir { get; }

            public uint Hash => gate.parentSegment.Hash;

            public ExposedGate(Gate gate)
            {
                this.gate = gate;
                Ownership = gate.parentSegment.GetComponent<PlayerOwnership>();
                ForwardTraversalDir = gate.parentSegment.IsTraversalForward(gate);
            }

            public bool GoThrough(out ExposedGate[] connectedGates)
            {
                connectedGates = null;

                if (gate.parentSegment.CanBeCrossed)
                    connectedGates = gate.parentSegment.GoThrough(gate).outwardGates.ToExposedGates();

                return connectedGates != null && connectedGates.Length != 0;
            }
        }

        public bool GoThrough(out ExposedGate[] connectedGates)
        {
            connectedGates = null;

            if (IsEnd)
                return false;

            connectedGates = inwardGate.outwardGates.ToExposedGates();
            return true;
        }

#if UNITY_EDITOR

        public virtual void DrawDebugInfo(Color tint)
        {
            // Node position for debug purposes
            if (TileDebugOptions.Instance.showNodes)
            {
                Gizmos.color = Color.yellow * tint;
                Gizmos.DrawSphere(WorldPos, 0.05f);
            }

            if(TileDebugOptions.Instance.showSegments && inwardGate != null)
            {
                Vector3 arrowDir = (inwardGate.WorldPos - WorldPos);
                Vector3 arrowDirNormalized = arrowDir.normalized;
                float magnitude = arrowDir.magnitude;

                Handles.color = CustomColors.darkGreen * tint;
                Vector3 lateralOffset = Vector3.Cross(arrowDir.normalized, Vector3.up) * 0.1f;
                Handles.ArrowHandleCap(0, WorldPos + lateralOffset, Quaternion.LookRotation(arrowDirNormalized, Vector3.up), magnitude, EventType.Repaint);
            }
        }

        private void Reset()
        {
            GetParentSegment();
        }

        [ContextMenu("Get Parent Segment")]
        private void GetParentSegment()
        {
            parentSegment = GetComponentInParent<TileSegment>();
        }
#endif
    }

    // TODO: Out of here!
    public static class GateExtensions
    {
        public static Gate.ExposedGate[] ToExposedGates(this IList<Gate> gates) => gates.Select(g => new Gate.ExposedGate(g)).ToArray();        
    }
}