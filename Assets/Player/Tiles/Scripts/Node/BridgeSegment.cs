using System.Linq;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class BridgeSegment : TileSegment
    {
        [SerializeField]
        private Gate gate;

        [SerializeField]
        private SideGate sideGate;

        public override Gate[] AllGates => new Gate[] { gate, sideGate };
        protected override SideGate[] SideGates => new SideGate[] { sideGate };        

        public override Gate GoThrough(Gate enterGate)
        {
            return AllGates.Where(g => g != enterGate).First();
        }

#if UNITY_EDITOR
        protected override void InitializeGates()
        {
            sideGate = gameObject.AddComponent<SideGate>();
            gate = gameObject.AddComponent<Gate>();
        }
#endif
    }
}
    
