using HexaLinks.PathFinder;
using System;
using System.Linq;
using UnityEngine;

namespace HexaLinks.Ownership
{
    public class InitialPlayerOwnership : PlayerOwnership
    {
        [SerializeField]
        private bool forceChange = false;

        [SerializeField]
        private PlayerOwnership[] childrenOwnership;

        public Ownership StartingOwner { private set; get; }

        private void Awake()
        {
            if (forceChange)
            {
                StartingOwner = Ownership.PlayerOne;
                InstantOwnerChange(Ownership.PlayerOne);
            }

            foreach (var child in childrenOwnership)
                child.OnOnwerChanged += UpdateOwnership;
        }   
        
        private void UpdateOwnership()
        {
            if (IsWinnerOwner(out Ownership newOwner) && newOwner != StartingOwner)
            {
                StartingOwner = PendingOwner = newOwner;

                Tile.Tile parentTile = transform.GetTransformUpUntil<Tile.Tile>().GetComponent<Tile.Tile>();
                PathStorage.Init(parentTile);
            }
        }       

        private bool IsWinnerOwner(out Ownership winner)
        {
            winner = Ownership.None;

            int[] ownershipCount = Enumerable.Repeat(0, 3).ToArray();

            foreach (var ownership in childrenOwnership)
                ownershipCount[(int)ownership.Owner]++;

            int winnerOwnership = ownershipCount.Max();
            bool tie = ownershipCount.Skip(1).Count(o => o == winnerOwnership) != 1;

            if (!tie)
                winner = (Ownership)Array.LastIndexOf(ownershipCount, winnerOwnership);

            return !tie && winner != Ownership.None;

        }
    }

}