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

        public Ownership PendingOwner { protected set; get; } = Ownership.None;

        public event Action OnOnwerChanged = delegate { };

        public void InstantOwnerChange(Ownership newOwnership)
        {
            PendingOwner = owner = newOwnership;
            highligther.InstantPropagation(playerColors[owner]);
        }     
        
        public virtual void OwnerChange(Ownership newOwnership)
        {
            PendingOwner = newOwnership;
        }

        public async UniTask UpdatePropagation(bool forwardPropagation)
        {
            if (PendingOwner == owner)
                return;

            highligther.PrePropagation(playerColors[PendingOwner], forwardPropagation);
            await highligther.UpdatePropagation();
            highligther.PostPropagation();

            owner = PendingOwner;
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