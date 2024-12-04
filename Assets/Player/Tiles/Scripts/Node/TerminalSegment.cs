using UnityEngine;

namespace HexaLinks.Tile
{
    public class TerminalSegment : TileSegment
    {
        [SerializeField]
        private Gate gate;

        public override Gate[] AllGates => new[] { gate };

        private readonly static SideGate[] EmptySideGates = new SideGate[0];
        protected override SideGate[] SideGates => EmptySideGates;

        public override bool CanBeCrossed => false;

        public override Gate GoThrough(Gate enterGate)
        {
            throw new System.NotImplementedException();
        }

#if UNITY_EDITOR
        [ContextMenu("Create Terminal Segment")]
        private void CreateTermianlSegmentGate()
        {
            gate = CreateGate();
        }
#endif        
    }
}