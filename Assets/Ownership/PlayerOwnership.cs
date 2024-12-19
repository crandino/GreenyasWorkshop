using Cysharp.Threading.Tasks;
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
        private bool computesInPropagation = true;
        public bool ComputesInPropagation => computesInPropagation;

        [SerializeField]
        private ColorPropagator highligther;

        [SerializeField]
        private Owner owner = Owner.None;
        public Owner Owner => /*PendingOwner ?? */owner;

        private Owner? PendingOwner { set; get; } = null;

        private OnSegmentPropagatedArgs segmentPropagationArgs;

        public bool IsSameOwner(Owner owner) => this.owner == owner;

        public void InstantOwnerChange(Owner newOwnership)
        {
            owner = newOwnership;
            highligther.InstantPropagation(Game.Instance.GetSystem<Configuration>().colors[owner].pathColor);
        }

        public void PreparePropagation(Owner newOwner, bool forwardPropagation)
        {
            if(owner != newOwner)
                PendingOwner = newOwner;

            highligther.PrePropagation(Game.Instance.GetSystem<Configuration>().colors[newOwner].pathColor, forwardPropagation);
        }

        public void FinalizePropagation()
        {
            highligther.PostPropagation();
            TileEvents.OnSegmentPropagated.Call(new OnSegmentPropagatedArgs(owner, PendingOwner ?? owner, computesInPropagation));

            if (PendingOwner.HasValue)
            {
                owner = PendingOwner.Value;
                PendingOwner = null;
            }
        }

        public void UpdatePropagation(float normalizedTime)
        {
            highligther.UpdatePropagation(normalizedTime);            
        }

#if UNITY_EDITOR
        private void Reset()
        {
            highligther = GetComponent<ColorPropagator>();
        }
#endif
    }

}