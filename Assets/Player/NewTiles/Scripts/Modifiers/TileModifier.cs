using Hexalinks.Tile;
using System.Collections;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
using UnityEngine;
#else
using UnityEngine;
#endif

public abstract class TileModifier
{
    public Tile Current { get; set; }

#if UNITY_EDITOR
    private EditorCoroutine coroutine;
#else
    private Coroutine coroutine;
#endif

    protected TileModifier(Tile currentTile)
    {
        Current = currentTile;
    }

    protected void Start()
    {
        OnStart();
#if UNITY_EDITOR
        coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(Update());
#else
        coroutine = Current.StartCoroutine(Update());
#endif
    }

    private IEnumerator Update()
    {
        while (true)
        {
            OnUpdate();
            yield return null;
        }
    }

    protected void Finish()
    {
        OnFinish();
#if UNITY_EDITOR
        EditorCoroutineUtility.StopCoroutine(coroutine);
#else
        Current.StopCoroutine(coroutine);
#endif
    }

    protected virtual void OnStart() { }
    protected abstract void OnUpdate();
    protected virtual void OnFinish() { }
}
