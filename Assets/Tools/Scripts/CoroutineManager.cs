using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager instance = null;

    public static void Start(IEnumerator coroutine)
    {
        TryInitialize();
        instance.StartCoroutine(coroutine);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private static void TryInitialize()
    {
        if (instance == null)
            instance = new GameObject("CoroutineManager").AddComponent<CoroutineManager>();
    }    
}
