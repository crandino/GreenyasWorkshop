using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static OwnershipPropagation;

public class PlayerOwnership : MonoBehaviour
{
    [SerializeField]
    private bool forceChange = false;

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

    public Ownership Owner { protected set; get; } = Ownership.None;    

    private void Awake()
    {
        if (forceChange)
            InstantOwnerChange(Ownership.PlayerOne);
    }

    protected PropagationData data;

    public void Prepare(PropagationData data)
    {
        this.data = data;
        highligther.Configure(playerColors[data.newOwner], data.forwardTraversalDirection);
    }

    public void InstantOwnerChange(Ownership newOwnership)
    {
        Owner = newOwnership;
        highligther.Highlight(playerColors[newOwnership]);
    }   

    public virtual async UniTask GraduallyOwnerChange()
    {
        if (Owner == data.newOwner)
            return;

        Owner = data.newOwner;
        await highligther.UpdateHighlight();

        return;
    }
#if UNITY_EDITOR

    private void Reset()
    {
        highligther = GetComponent<PathHighligther>();
    } 
#endif

    public static class OwnershipCounter
    {
        public static bool IsWinnerOwner(PlayerOwnership[] playerOwnerships, out Ownership winner)
        {
            winner = Ownership.None;

            int[] ownershipCount = Enumerable.Repeat(0, 3).ToArray();

            foreach (var ownership in playerOwnerships)
                ownershipCount[(int)ownership.Owner]++;

            int winnerOwnership = ownershipCount.Max();
            bool tie = ownershipCount.Skip(1).Count(o => o == winnerOwnership) != 1;

            if (!tie)
                winner = (Ownership)Array.LastIndexOf(ownershipCount, winnerOwnership);

            return !tie && winner != Ownership.None;

        }
    }
}
