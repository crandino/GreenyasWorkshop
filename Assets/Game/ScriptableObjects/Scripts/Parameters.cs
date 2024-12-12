using UnityEngine;

namespace HexaLinks.Configuration
{
	[System.Serializable]
    public class Parameters
	{
		[SerializeField, Range(0,20)] private int numOfConnectionsToUnlockPropagator = 0;

		public int NumOfConnectionsToUnlockPropagator => numOfConnectionsToUnlockPropagator;
	} 
}
