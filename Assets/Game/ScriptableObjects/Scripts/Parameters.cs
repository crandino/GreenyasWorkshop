using UnityEngine;

namespace HexaLinks.Configuration
{
	[System.Serializable]
    public class Parameters
	{
		[SerializeField, Range(0,20)] private int numOfConnectionsToUnlockPropagator = 0;
		public int NumOfConnectionsToUnlockPropagator => numOfConnectionsToUnlockPropagator;

		[SerializeField, Range(1, 100)] private int minScoreToWin = 50;
        public int MinScoreToWin => minScoreToWin;
    } 
}
