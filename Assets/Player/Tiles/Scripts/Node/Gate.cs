using HexaLinks.Ownership;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using Hexagon.Tile.Debug;
using UnityEditor;
#endif

namespace HexaLinks.Tile
{
    public class Gate : MonoBehaviour
    {
        public class ReadOnlyGate
        {
            private readonly Gate gate;

            public PlayerOwnership Ownership { get; }
            public bool ForwardTraversalDir { get; }
            public uint Hash { get; }

            public ReadOnlyGate[] OutwardGates => gate.outwardGates.ToExposedGates();

            public TilePropagator Propagator
            {
                get
                {
                    Assert.IsTrue(IsEnd);
                    return gate.GetComponentInParent<TilePropagator>();
                }
            }

            public bool GoThrough(out ReadOnlyGate[] gates)
            {
                gates = null;

                if (!gate.IsEnd && gate.inwardGate.IsConnected)
                {
                    gates = gate.inwardGate.outwardGates.ToExposedGates();
                    return true;
                }

                return false;
            }

            public bool IsEnd => gate.IsEnd;
            public bool IsConnected => gate.IsConnected;

            public ReadOnlyGate(Gate gate)
            {
                this.gate = gate;

                Hash = gate.parentSegment.Hash;
                Ownership = gate.parentSegment.GetComponent<PlayerOwnership>();
                ForwardTraversalDir = gate.parentSegment.IsTraversalForward(gate);
            }

            public static bool operator==(ReadOnlyGate x, ReadOnlyGate y)
            {
                return x.Ownership == y.Ownership && x.ForwardTraversalDir == y.ForwardTraversalDir;
            }

            public static bool operator !=(ReadOnlyGate x, ReadOnlyGate y)
            {
                return x.Ownership != y.Ownership || x.ForwardTraversalDir != y.ForwardTraversalDir;
            }
        }


        [SerializeField, ReadOnly]
        protected TileSegment parentSegment;

        [SerializeField]
        private Gate inwardGate;

        [SerializeReference]
        protected List<Gate> outwardGates = new List<Gate>();

        public bool IsEnd => inwardGate == null;
        public bool IsConnected => outwardGates.Count > 0;

        public virtual Vector3 WorldPos => parentSegment.transform.position;

        //private ReadOnlyGate[] GoThrough()
        //{
        //    return inwardGate.outwardGates.ToExposedGates();
        //}

#if UNITY_EDITOR

        public virtual void DrawDebugInfo(Color tint)
        {
            // Node position for debug purposes
            if (TileDebugOptions.Instance.showNodes)
            {
                Gizmos.color = Color.yellow * tint;
                Gizmos.DrawSphere(WorldPos, 0.05f);
            }

            if (TileDebugOptions.Instance.showSegments && inwardGate != null)
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
}