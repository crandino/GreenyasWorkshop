using Cysharp.Threading.Tasks;
using Hexalinks.PathFinder;
using Hexalinks.Tile;
using UnityEngine;

public class InitialPlayerOwnership : PlayerOwnership
{
    [SerializeField]
    private PlayerOwnership[] childrenOwnership;

    private bool UpdateOwnership()
    {
        if (OwnershipCounter.IsWinnerOwner(childrenOwnership, out Ownership winner))
        {
            data = new OwnershipPropagation.PropagationData(winner, true);
            return true;
        }
        return false;
    }

    public override async UniTask GraduallyOwnerChange()
    {
        if (!UpdateOwnership() || Owner == data.newOwner)
            return;

        await base.GraduallyOwnerChange();

        Tile parentTile = transform.GetTransformUpUntil<Tile>().GetComponent<Tile>();

        PathStorage.InitNewPropagation();
        PathIterator.FindPathsFrom(parentTile);
    }
}
