using System.Collections.Generic;
using UnityEngine.Pool;

public partial class TileSegment
{
    public class Gate
    {
        public Node Node { private set; get; }
        public TileSegment Segment { private set; get; }

        public int ID = -1;

        private static int IDs = 0;

        public static GatePool Pool { private set; get; } = new GatePool();
       
        public class GatePool
        {
            private ObjectPool<Gate> pool;

            public GatePool()
            {
                pool = new ObjectPool<Gate>(CreateGate);
            }

            private static Gate CreateGate()
            {
                return new Gate()
                {
                    ID = ++IDs,
                };
            }

            public Gate CreateGate(TileSegment segment, Node node)
            {
                Gate gate = pool.Get();
                gate.Node = node;
                gate.Segment = segment;
                //Debug.Log($"Getting: {gate.ID} ID");
                return gate;
            }

            public void Release(List<Gate> gates)
            {
                pool.Release(gates);
            }
        }

        public Node GoThrough()
        {
            return Segment.GoThrough(Node);
        }
    }
}
