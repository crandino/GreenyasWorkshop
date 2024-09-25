using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace  Hexalinks.Tile
{
    public class TileFiller : Tile
    {
        public Gate.ExposedGate[] Gates => connectivity.Gates;

#if UNITY_EDITOR
        protected override void InternalSetup()
        {
            connectivity.InitializeSegments(1);
        }
#endif
    }
}
