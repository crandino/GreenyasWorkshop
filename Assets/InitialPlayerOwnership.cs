using Cysharp.Threading.Tasks;
using Hexalinks.PathFinder;
using Hexalinks.Tile;
using UnityEngine;
using static OwnershipPropagation;

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

    private bool UpdateOwnership()
    {
        if (OwnershipCounter.IsWinnerOwner(childrenOwnership, out Ownership winner))
        {
            data = new OwnershipPropagation.PropagationData(winner, true);
            return true;
        }
        return false;
    }

    public override async UniTask GraduallyOwnerChange(PropagationData data)
    {
        if (!UpdateOwnership() || owner == data.newOwner)
            return;

        await base.GraduallyOwnerChange(data);

        Tile parentTile = transform.GetTransformUpUntil<Tile>().GetComponent<Tile>();

        PathStorage.InitNewPropagation();
        PathIterator.FindPathsFrom(parentTile);
    }
}
