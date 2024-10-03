using Hexagon.Tile.Debug;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor; 
#endif
using UnityEngine;

namespace Hexalinks.Tile
{
    [System.Serializable]
    public class Gate : MonoBehaviour
    {
        //[SerializeField]
        //protected TileSegment segment;

        [SerializeReference]
        protected List<Gate> outwardGates = new List<Gate>();

        public TileSegment Segment => gameObject.GetComponent<TileSegment>();

        public void Disconnect()
        {
            foreach (Gate conn in outwardGates)
                conn.outwardGates.Clear();

            outwardGates.Clear();
        }

        public readonly struct ExposedGate
        {
            private readonly Gate gate;

            public readonly ExposedGate[] OutwardGates => gate.outwardGates.ToExposedGates();

            public ExposedGate(Gate gate)
            {
                this.gate = gate;
            }

            public bool GoThrough(out ExposedGate[] connectedGates)
            {
                connectedGates = null;

                if (gate.Segment.CanBeCrossed)
                    connectedGates = gate.Segment.GoThrough(gate).outwardGates.ToExposedGates();

                return connectedGates != null && connectedGates.Length != 0;
            }

            public void Log()
            {
                Debug.Log($"Segment on tile {gate.Segment.transform.parent.gameObject.name}");
            }

            public TileSegment Segment => gate.Segment;
        }

#if UNITY_EDITOR

        public virtual Vector3 WorldDebugPos => Segment.transform.position;       

        public virtual void DrawDebugInfo()
        {
            // Node position for debug purposes
            if (TileDebugOptions.Instance.showNodes)
            {
                Gizmos.color = CustomColors.darkOrange;
                Gizmos.DrawSphere(WorldDebugPos, 0.05f);
            }
        }      
#endif
    }

    public static class GateExtensions
    {
        public static Gate.ExposedGate[] ToExposedGates(this IList<Gate> gates) => gates.Select(g => new Gate.ExposedGate(g)).ToArray();        
    }
}