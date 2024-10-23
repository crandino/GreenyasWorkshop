using Cysharp.Threading.Tasks;
using Hexalinks.PathFinder;
using Hexalinks.Tile;
using UnityEngine;

namespace HexaLinks.Ownership
{
    public class InitialPlayerOwnership : PlayerOwnership
    {
        [SerializeField]
        private bool forceChange = false;

        [SerializeField]
        private PlayerOwnership[] childrenOwnership;

        private void Awake()
        {
            if (forceChange)
                InstantOwnerChange(Ownership.PlayerOne);
        }

        private bool UpdateOwnership(out Ownership winner)
        {
            return OwnershipCounter.IsWinnerOwner(childrenOwnership, out winner);
        }

        public override async UniTask GraduallyOwnerChange(Ownership newOwner, bool forwardPropagation)
        {
            if (!UpdateOwnership(out Ownership winner) || owner == newOwner)
                return;

            await base.GraduallyOwnerChange(newOwner, true);

            Tile parentTile = transform.GetTransformUpUntil<Tile>().GetComponent<Tile>();

            PathStorage.InitNewPropagation();
            PathIterator.FindPathsFrom(parentTile);
        }
    }

}