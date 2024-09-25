using Hexalinks.Tile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ITileOwnership;

public class FillerCenter : MonoBehaviour
{
    [SerializeField]
    private TileSegment segments;



    public void UpdateOwnership()
    {
        
    }
}

public interface ITileOwnership
{
    public enum Owner
    {
        None = 0,
        PlayerOne = 1,
        PlayerTwo = 2
    }

    Owner Ownership { set; get; }

    void UpdateOwnership();
}

public abstract class TileOwnership
{
    public enum Owner
    {
        None = 0,
        PlayerOne = 1,
        PlayerTwo = 2
    }

    public Owner Ownership { private set; get; }


}
