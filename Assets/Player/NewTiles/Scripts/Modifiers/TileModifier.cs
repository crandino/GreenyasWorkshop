using Hexalinks.Tile;
using System.Collections;
using UnityEngine;

public abstract class TileModifier
{
    public Tile Current { get; set; }

    private Coroutine coroutine = null;

    protected TileModifier(Tile currentTile)
    {
        Current = currentTile;
    }

    protected void Start()
    {
        OnStart();
        coroutine = Current.StartCoroutine(Update());
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
            Current.StopCoroutine(coroutine);
    }

    protected virtual void OnStart() { }
    protected virtual bool OnUpdate() { return false; }
    protected virtual void OnFinish() { }
}
