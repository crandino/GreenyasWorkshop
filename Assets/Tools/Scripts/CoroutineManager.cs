using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager instance = null;

    private static CoroutineManager Instance
    {
        get
        {
            TryInitialize();
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public static Coroutine Start(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }

    public static void Stop(Coroutine coroutine)
    {
        Instance.StopCoroutine(coroutine);
    }

    //public static void Start(IEnumerator coroutine)
    //{
    //    Instance.StartCoroutine(Core(coroutine));
    //}

    //public static void Core(IEnumerator coroutine)
    //{
    //    yield return coroutine;
    //}

    private void OnDestroy()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = null;
    }

    private static void TryInitialize()
    {
        if (instance == null)
        {
            instance = new GameObject("CoroutineManager").AddComponent<CoroutineManager>();
            instance.gameObject.isStatic = true;
            instance.hideFlags = HideFlags.NotEditable;
        }
    }    
}
