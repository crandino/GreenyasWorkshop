using Greenyas.Hexagon;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using static Greenyas.Hexagon.HexSide;
using System;

#if UNITY_EDITOR
using UnityEditor;
using Hexagon.Tile.Debug;
using HexaLinks.Tile.Extensions.Hexside;
#endif

namespace HexaLinks.Tile
{
    [System.Serializable]
    public class SideGate : Gate
    {
        [SerializeField]
        private HexSide hexSide = new();

        public Side WorldSide => hexSide.GetWorldSide(Segment);

        public class ConnectionCandidates
        {
            private readonly struct ConnectionPair
            {
                private readonly SideGate gate;
                private readonly SideGate otherGate;

                public ConnectionPair(SideGate gate, SideGate otherGate)
                {
                    this.gate = gate;
                    this.otherGate = otherGate;
                }

                public void Connect()
                {
                    gate.outwardGates.Add(otherGate);
                    otherGate.outwardGates.Add(gate);
                }
            }

            private ConnectionPair[] candidates = new ConnectionPair[0];

            public static Func<ConnectionCandidates, bool> AtLeastOneConnection => (c) => c.candidates.Length > 0;

            public bool Check(Func<ConnectionCandidates, bool> predicate) => predicate(this);            

            public void Connect()
            {
                for(int i = 0; i < candidates.Length; ++i)
                    candidates[i].Connect();
            }

            public void AddPair(SideGate gate, SideGate otherGate)
            {
                candidates = candidates.Append(new(gate, otherGate)).ToArray();
            }
        }


        public void GetPossibleConnections(SideGate againstGate, ConnectionCandidates candidatesResult )
        {
            // There's any previous connection between those gates
            if (outwardGates.Contains(againstGate) && againstGate.outwardGates.Where(g => g == againstGate).Count() == 0)
                return;

            if (IsFacingOtherGate(againstGate))
                candidatesResult.AddPair(this, againstGate);
        }
       
        public void TryConnect(SideGate againstGate)
        {
            // There's any previous connection between those gates
            if (outwardGates.Contains(againstGate) && againstGate.outwardGates.Where(g => g == againstGate).Count() == 0)
                return;

            if (IsFacingOtherGate(againstGate))
            {
                outwardGates.Add(againstGate);
                againstGate.outwardGates.Add(this);
            };
        }

        private bool IsFacingOtherGate(SideGate gateTo)
        {
            Assert.IsTrue(Segment != gateTo.Segment);
            return WorldSide.IsOpposite(gateTo.WorldSide);
        }

#if UNITY_EDITOR

        public override Vector3 WorldDebugPos
        {
            get
            {
                Vector2 localDir = CubeCoord.GetVectorToNeighborHexOn(WorldSide);
                Vector3 localPosition = new Vector3(localDir.x, 0.05f, localDir.y);
                return Segment.transform.position + localPosition * HexTools.hexagonSize;
            }
        }

        public override void DrawDebugInfo()
        {
            base.DrawDebugInfo();

            // Outward node conntections
            if (TileDebugOptions.Instance.showConnections && Application.isPlaying)
            {
                Vector2 vec = CubeCoord.GetVectorToNeighborHexOn(WorldSide);
                Vector3 toNextTile = new Vector3(vec.x, 0f, vec.y);
                Quaternion arrowOrientatinon = Quaternion.LookRotation(toNextTile);

                GUIStyle textStyle = new GUIStyle();
                textStyle.fontSize = 18;
                Handles.color = textStyle.normal.textColor = outwardGates.Count != 0 ? Color.green : Color.red;
                Handles.Label(WorldDebugPos + toNextTile * 0.3f, $"{outwardGates.Count}", textStyle);

                Handles.ArrowHandleCap(0, WorldDebugPos, arrowOrientatinon, 0.2f, EventType.Repaint);
            }
        }
    }
#endif
}