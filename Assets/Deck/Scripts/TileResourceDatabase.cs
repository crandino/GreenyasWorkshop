using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Game;

[CreateAssetMenu(fileName = "Tile Resource Set")]
public class TileResourceDatabase : GameSystemScriptableObject
{
    [SerializeField]
    private TileResource[] resources;

    public TileResource FindBy(string name)
    {
        return resources.First(r => r.name == name);
    }
}
