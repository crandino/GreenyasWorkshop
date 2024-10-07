using UnityEngine;

//#if UNITY_EDITOR
//using System.Reflection;
//#endif

namespace Hexalinks.Tile
{
    public class TerminalSegment : TileSegment
    {
        [SerializeField]
        private Gate gate;

        public override Gate[] AllGates => new[] { gate };

        public override bool CanBeCrossed => false;

        private readonly static SideGate[] EmptySideGates = new SideGate[0];

        protected override SideGate[] SideGates => EmptySideGates;

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

        //#if UNITY_EDITOR
        //        protected override void InitializeGates()
        //        {
        //            gate = gameObject.AddComponent<Gate>();
        //        }

        //[ContextMenu("Get References")] 
        //private void GetReferences()
        //{
        //    BridgeSegment[] bridges = GetComponentsInChildren<BridgeSegment>();

        //    foreach(var b in bridges)
        //    {
        //        FieldInfo bridgeGateInfo = b.GetType().GetField("gate", BindingFlags.Instance | BindingFlags.NonPublic);
        //        FieldInfo terminalOutwardGateInfo = gate.GetType().GetField("outwardGates", BindingFlags.Instance | BindingFlags.NonPublic);

        //        var x = terminalOutwardGateInfo.GetValue(gate);
        //        Gate bridgeGate = (Gate)bridgeGateInfo.GetValue(b);
        //        x.GetType().GetMethod("Add").Invoke(x, new[] { bridgeGate });


        //        FieldInfo bridgeOutwardGatesInfo = bridgeGateInfo.FieldType.GetField("outwardGates", BindingFlags.Instance | BindingFlags.NonPublic);
        //        var y = bridgeOutwardGatesInfo.GetValue(bridgeGate);
        //        y.GetType().GetMethod("Add").Invoke(y, new[] { gate });
        //    }
        //}
        //#endif
    }
}