using UnityEngine;

namespace HexaLinks.Configuration
{
	[System.Serializable]
    public class Parameters
	{
		[SerializeField] private int numOfConnectionsToUnlockPropagator = 0;

		public int NumOfConnectionsToUnlockPropagator => numOfConnectionsToUnlockPropagator;
	} 
}
