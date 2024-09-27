using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPlayerOwnership : PlayerOwnership
{
    [SerializeField]
    private PlayerOwnership[] childrenOwnership;

    private void Start()
    {
        foreach (var owner in childrenOwnership)
            owner.OnOwnershipChange += UpdateOwnership;
    }

    private void UpdateOwnership()
    {
        OwnershipCounter.Calculate(childrenOwnership);
    }
}
