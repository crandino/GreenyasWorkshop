using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraversableTileSegment : TileSegment
{
#if UNITY_EDITOR
	protected override int NumberOfNodes => 2; 
#endif
}
