using UnityEngine;

public class SingletonMonobehaviour<T> : MonoBehaviour where T : Object
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            TryAutoInitialization();
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    protected static void TryAutoInitialization()
    {
        instance ??= FindAnyObjectByType<T>();
    }

    private void Awake()
    {
        TryAutoInitialization();
    }
}
