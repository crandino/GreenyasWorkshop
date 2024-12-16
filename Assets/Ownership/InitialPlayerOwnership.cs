using HexaLinks.Tile;
using UnityEngine;

namespace HexaLinks.Ownership
{
    public class InitialPlayerOwnership : PlayerOwnership
    {
        [SerializeField]
        private PlayerOwnership[] childrenOwnership;
        
        private void Awake()
        {
            //foreach (var child in childrenOwnership)
            //    child.OnOnwerChanged += TriggerNewPropagation;
        }   

        private void TriggerNewPropagation()
        {
            //TilePropagator parentTile = transform.GetTransformUpUntil<TilePropagator>().GetComponent<TilePropagator>();
            //parentTile.PreparePropagation();
        }

        // -------------------------------------------------------------------------------------
        // NOTE! Do not delete that part. It's an old mechanic and could be useful in the future.
        // -------------------------------------------------------------------------------------

        //private void UpdateOwnership()
        //{
        //    if (IsWinnerOwner(out Owner newOwner) && newOwner != Owner)
        //    {
        //        PendingOwner = newOwner;

        //        TilePropagator parentTile = transform.GetTransformUpUntil<TilePropagator>().GetComponent<TilePropagator>();
        //        Path.Finder.PathFinder.Init(parentTile);
        //    }
        //}    

        //private bool IsWinnerOwner(out Owner winner)
        //{
        //    winner = Owner.None;

        //    int[] ownershipCount = Enumerable.Repeat(0, 3).ToArray();

        //    foreach (var ownership in childrenOwnership)
        //        ownershipCount[(int)ownership.Owner]++;

        //    int winnerOwnership = ownershipCount.Max();
        //    bool tie = ownershipCount.Skip(1).Count(o => o == winnerOwnership) != 1;

        //    if (!tie)
        //        winner = (Owner)Array.LastIndexOf(ownershipCount, winnerOwnership);

        //    return !tie && winner != Owner.None;

        //}
    }
}