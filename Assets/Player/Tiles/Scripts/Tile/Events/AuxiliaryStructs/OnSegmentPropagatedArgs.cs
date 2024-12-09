using HexaLinks.Ownership;


namespace HexaLinks.Tile.Events
{
    public struct OnSegmentPropagatedArgs
    {
        public PlayerOwnership.Ownership lastSegmentOwner;
        public PlayerOwnership.Ownership newSegmentOwner;

        public readonly int GetScoreIncrement(PlayerOwnership.Ownership scoreOwner)
        {
            if (lastSegmentOwner != newSegmentOwner)
            {
                if (scoreOwner == lastSegmentOwner)
                    return -1;

                if (scoreOwner == newSegmentOwner)
                    return +1;
            }

            return 0;
        }
    } 
}
