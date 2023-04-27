using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalTileSegment : TileSegment
{
#if UNITY_EDITOR
	protected override int NumberOfNodes => 1; 
#endif
}
