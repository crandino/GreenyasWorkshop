using Hexalinks.Tile;
using System.Collections;
using UnityEngine;

public abstract class TileModifier
{
    public TileCoordinates Coordinates { private set; get; }

    private Coroutine coroutine = null;

    protected TileModifier(TileCoordinates currentTile)
    {
        Coordinates = currentTile;
    }

    protected void Start()
    {
        OnStart();
        coroutine = CoroutineManager.Start(Update());
    }

    private IEnumerator Update()
    {
        yield return null;

        while (OnUpdate()) yield return null;
        Finish();
    }

    protected void Finish()
    {
        OnFinish();
        if(coroutine != null) 
            CoroutineManager.Stop(coroutine);
    }

    protected virtual void OnStart() { }
    protected virtual bool OnUpdate() { return false; }
    protected virtual void OnFinish() { }
}
