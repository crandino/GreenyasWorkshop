using HexaLinks.Ownership;

namespace HexaLinks.Tile.Events
{
    public readonly struct OnSegmentPropagatedArgs
    {
        private readonly Owner lastSegmentOwner;
        private readonly Owner newSegmentOwner;

        public OnSegmentPropagatedArgs(Owner lastOwner, Owner newOwner)
        {
            lastSegmentOwner = lastOwner;
            newSegmentOwner = newOwner;
        }

        public readonly int GetScoreIncrement(Owner scoreOwner)
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
