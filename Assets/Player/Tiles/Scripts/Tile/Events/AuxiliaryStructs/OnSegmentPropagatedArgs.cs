using HexaLinks.Ownership;

namespace HexaLinks.Events.Arguments
{
    public readonly struct OnSegmentPropagatedArgs
    {
        private readonly Owner lastSegmentOwner;
        private readonly Owner newSegmentOwner;
        private readonly bool computePropagation;

        public OnSegmentPropagatedArgs(Owner lastOwner, Owner newOwner, bool compute = true)
        {
            lastSegmentOwner = lastOwner;
            newSegmentOwner = newOwner;
            computePropagation = compute;
        }

        public readonly int GetScoreIncrement(Owner scoreOwner)
        {
            if (computePropagation && lastSegmentOwner != newSegmentOwner)
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
