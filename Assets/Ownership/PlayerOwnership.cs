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

        [SerializeField]
        private ColorPropagator highligther;

        [SerializeField]
        private Owner owner = Owner.None;

        public bool ComputesInPropagation => computesInPropagation;

        public Owner Owner => PendingOwner ?? owner;

        public Owner? PendingOwner { protected set; get; } = null;

        private OnSegmentPropagatedArgs segmentPropagationArgs;

        public void InstantOwnerChange(Owner newOwnership)
        {
            owner = newOwnership;
            highligther.InstantPropagation(Game.Instance.GetSystem<Configuration>().colors[owner].pathColor);
        }

        public void PreparePropagation(Owner newOwner, bool forwardPropagation)
        {
            if(owner == newOwner)
                PendingOwner = newOwner;

            highligther.PrePropagation(Game.Instance.GetSystem<Configuration>().colors[newOwner].pathColor, forwardPropagation);
            segmentPropagationArgs = new OnSegmentPropagatedArgs(owner, newOwner, computesInPropagation);
        }

        public void FinalizePropagation()
        {
            highligther.PostPropagation();

            if (PendingOwner.HasValue)
            {
                owner = PendingOwner.Value;
                PendingOwner = null;
            }

            TileEvents.OnSegmentPropagated.Call(segmentPropagationArgs);
        }

        public void UpdatePropagation(float normalizedTime)
        {
            //if (owner == newOwnership)
            //    return false;

            //PendingOwner = newOwnership;

            //OnSegmentPropagatedArgs args = new OnSegmentPropagatedArgs(owner, PendingOwner.Value);

            //highligther.PrePropagation(Game.Instance.GetSystem<Configuration>().colors[PendingOwner.Value].pathColor, forwardPropagation);
            highligther.UpdatePropagation(normalizedTime);
            //highligther.PostPropagation();

            //if(PendingOwner.HasValue)
            //{
            //    owner = PendingOwner ?? owner;
            //    PendingOwner = null;

            //    if(computesInPropagation)
            //        TileEvents.OnSegmentPropagated.Call(args);
            //}           

            //return computesInPropagation && true;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            highligther = GetComponent<ColorPropagator>();
        }
#endif
    }

}