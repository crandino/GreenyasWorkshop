using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static OwnershipPropagation;

public class PlayerOwnership : MonoBehaviour
{
    [SerializeField]
    private bool forceChange = false;

    [SerializeField]
    private PathHighligther highligther;

    public event Action OnOwnershipChange;

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

    public Ownership Owner { private set; get; } = Ownership.None;    

    private void Awake()
    {
        if (forceChange)
            InstantOwnerChange(Ownership.PlayerOne);
    }

    private PropagationData data;

    public void Prepare(PropagationData data)
    {
        this.data = data;
        highligther.Configure(playerColors[data.newOwner], data.forwardTraversalDirection);
    }

    public void InstantOwnerChange(Ownership newOwnership)
    {
        Owner = newOwnership;
        highligther.Highlight(playerColors[newOwnership]);

        OnOwnershipChange?.Invoke();
    }   

    // TODO 
    /*
     
    - Limpiar todo el código comentado.
    - Colocar nombres variables correctamente en el PathStorage.
    - Quizá, separar en varios archivos toda la clase PathStorage
    - Añadir en los prefabs, las distancias de los segmentos
    - Probar con una colorist de más salidas
    - Añadir una colorist al final y buscar la manera de que vuelva a arrancar todo de nuevo
    */


    public async UniTask OwnerChange(Ownership newOwnership)
    {
        if (Owner == data.newOwner)
            return;

        Owner = newOwnership;
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
        private static readonly Dictionary<Ownership, int> ownershipCount = new Dictionary<Ownership, int>
        (
        new[]
        {
            new KeyValuePair<Ownership, int>(Ownership.None, 0),
            new KeyValuePair<Ownership, int>(Ownership.PlayerOne, 0),
            new KeyValuePair<Ownership, int>(Ownership.PlayerTwo, 0)
        });

        public static void Calculate(PlayerOwnership[] playerOwnerships)
        {
            Clear();

            foreach (var ownership in playerOwnerships)
                ownershipCount[ownership.Owner]++;
        }

        private static void Clear()
        {
            ownershipCount[Ownership.PlayerTwo] = 0;
            ownershipCount[Ownership.PlayerOne] = 0;
            ownershipCount[Ownership.None] = 0;
        }
    }
}
