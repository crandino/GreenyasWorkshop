using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static OwnershipPropagation;

public class PlayerOwnership : MonoBehaviour
{
    [SerializeField]
    private PathHighligther highligther;

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

    protected PropagationData data;

    public void InstantOwnerChange(Ownership newOwnership)
    {
        owner = newOwnership;
        highligther.Highlight(playerColors[newOwnership]);
    }

    public virtual async UniTask GraduallyOwnerChange(PropagationData data)
    {
        if (owner == data.newOwner)
            return;

        if (highligther.PreHighlight(playerColors[data.newOwner], data.forwardTraversalDirection))
        {
            owner = data.newOwner;
            await highligther.UpdateHighlight();
        }

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
        highligther = GetComponent<PathHighligther>();
    }
#endif
}
