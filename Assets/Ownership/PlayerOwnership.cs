using Cysharp.Threading.Tasks;
using HexaLinks.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexaLinks.Ownership
{
    public class PlayerOwnership : MonoBehaviour
    {
        [SerializeField]
        private ColorPropagator highligther;

        private static readonly Dictionary<Ownership, Color> playerColors = new Dictionary<Ownership, Color>
        (
            new[]
            {
            new KeyValuePair<Ownership, Color>(Ownership.None, Color.black),
            new KeyValuePair<Ownership, Color>(Ownership.PlayerOne, new Color(0f, 0.5f, 0f, 1f)),
            new KeyValuePair<Ownership, Color>(Ownership.PlayerTwo, Color.yellow)
            }
        );

        public enum Ownership
        {
            None = 0,
            PlayerOne,
            PlayerTwo
        }

        [SerializeField]
        protected Ownership owner = Ownership.None;

        public Ownership Owner => owner;

        public void InstantOwnerChange(Ownership newOwnership)
        {
            owner = newOwnership;
            highligther.InstantPropagation(playerColors[newOwnership]);
        }

        public virtual async UniTask GraduallyOwnerChange(Ownership newOwner, bool forwardPropagation)
        {
            highligther.PrePropagation(playerColors[newOwner], forwardPropagation);

            if (owner == newOwner)
                return;

            owner = newOwner;
            await highligther.UpdatePropagation();
            highligther.PostPropagation();

            return;
        }

        public static class OwnershipCounter
        {
            public static bool IsWinnerOwner(PlayerOwnership[] playerOwnerships, out Ownership winner)
            {
                winner = Ownership.None;

                int[] ownershipCount = Enumerable.Repeat(0, 3).ToArray();

                foreach (var ownership in playerOwnerships)
                    ownershipCount[(int)ownership.owner]++;

                int winnerOwnership = ownershipCount.Max();
                bool tie = ownershipCount.Skip(1).Count(o => o == winnerOwnership) != 1;

                if (!tie)
                    winner = (Ownership)Array.LastIndexOf(ownershipCount, winnerOwnership);

                return !tie && winner != Ownership.None;

            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            highligther = GetComponent<ColorPropagator>();
        }
#endif
    }

}