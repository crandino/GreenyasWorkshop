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

        private bool markedToRelease = true;

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

            public Gate GenerateGate(TileSegment segment, Node node)
            {
                Gate gate = pool.Get();
                //Debug.Log($"Getting: {gate.ID} ID");
                gate.Node = node;
                gate.Segment = segment;
                gate.markedToRelease = true;
                return gate;
            }

            public void TryRelease(List<Gate> gates)
            {
                pool.Release(gates, g => g.markedToRelease);
                //Debug.Log($"Total {pool.CountAll}, active {pool.CountActive} inactive {pool.CountInactive}");
            }

            public void Release(List<Gate> gates)
            {
                pool.Release(gates, g => true);
                //Debug.Log($"Total {pool.CountAll}, active {pool.CountActive} inactive {pool.CountInactive}");
            }
        }
        public Node GoThrough()
        {
            return Segment.GoThrough(Node);
        }

        public void Connect(List<Gate> toGates, bool twoWay = true)
        {
            Node.Connections.AddRange(toGates);
            toGates.ForEach(g => g.markedToRelease = false);

            if (twoWay)
            {
                markedToRelease = false;
                for (int i = 0; i < toGates.Count; i++)
                    toGates[i].Node.Connections.Add(this);
            }

            Pool.TryRelease(toGates);
        }

        public void Disconnect()
        {
            for (int i = 0; i < Node.Connections.Count; i++)
            {
                Pool.Release(Node.Connections[i].Node.Connections);
                Node.Connections[i].Node.Connections.Clear();
            }

            Pool.Release(Node.Connections);
            Node.Connections.Clear();
        }
    }
}
