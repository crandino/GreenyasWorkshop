using Cysharp.Threading.Tasks;

using System;
using UnityEngine;

namespace HexaLinks.Ownership
{
    using Configuration;
    using Propagation;
    using Tile.Events;

    public enum Owner
    {
        None = 0,
        PlayerOne,
        PlayerTwo
    }

    public class PlayerOwnership : MonoBehaviour
    {
        [SerializeField]
        private ColorPropagator highligther;

        [SerializeField]
        private Owner owner = Owner.None;

        public Owner Owner => PendingOwner.HasValue ? PendingOwner.Value : owner;

        public Owner? PendingOwner { protected set; get; } = null;

        public event Action OnOnwerChanged = delegate { };

        //private Colors colors = null;

        //private void Start()
        //{
        //    colors = Game.Instance.GetSystem<Configuration>().colors;
        //}

        public void InstantOwnerChange(Owner newOwnership)
        {
            owner = newOwnership;
            highligther.InstantPropagation(Game.Instance.GetSystem<Configuration>().colors[owner]);
        }     
        
        public void PrepareOwnerChange(Owner newOwnership)
        {
            if(owner != newOwnership)
                PendingOwner = newOwnership;
        }

        public async UniTask UpdatePropagation(bool forwardPropagation)
        {
            if (!PendingOwner.HasValue)
                return;

            OnSegmentPropagatedArgs args = new OnSegmentPropagatedArgs(owner, PendingOwner.Value);

            highligther.PrePropagation(Game.Instance.GetSystem<Configuration>().colors[PendingOwner.Value], forwardPropagation);
            await highligther.UpdatePropagation();
            highligther.PostPropagation();

            owner = PendingOwner.Value;
            PendingOwner = null;

            TileEvents.OnSegmentPropagated.Call(args);

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