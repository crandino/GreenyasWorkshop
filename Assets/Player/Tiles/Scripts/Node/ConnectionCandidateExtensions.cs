using HexaLinks.Tile;
public static class ConnectionCandidateExtensions
{
    public static void Connect(this ConnectionCandidate[] candidates)
    {
        for(int i = 0; i < candidates.Length; ++i)
        {
            candidates[i].Connect();
        }
    }

    public static bool AreValid(this ConnectionCandidate[] candidates)
    {
        bool valid = false;

        for (int i = 0; i < candidates.Length; ++i)
            valid = valid || candidates[i].AreValid;

        return valid;
    }
}
