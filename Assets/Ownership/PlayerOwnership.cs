using Cysharp.Threading.Tasks;
using HexaLinks.Propagation;
using System;
using System.Collections.Generic;
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

        public Ownership Owner => PendingOwner.HasValue ? PendingOwner.Value : owner;

        public Ownership? PendingOwner { protected set; get; } = null;

        public event Action OnOnwerChanged = delegate { };

        public void InstantOwnerChange(Ownership newOwnership)
        {
            owner = newOwnership;
            highligther.InstantPropagation(playerColors[owner]);
        }     
        
        public void PrepareOwnerChange(Ownership newOwnership)
        {
            if(owner != newOwnership)
                PendingOwner = newOwnership;
        }

        public async UniTask UpdatePropagation(bool forwardPropagation)
        {
            if (!PendingOwner.HasValue)
                return;

            highligther.PrePropagation(playerColors[PendingOwner.Value], forwardPropagation);
            await highligther.UpdatePropagation();
            highligther.PostPropagation();

            owner = PendingOwner.Value;
            PendingOwner = null;

            OnOnwerChanged();

            return;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            highligther = GetComponent<ColorPropagator>();
        }
#endif
    }

}