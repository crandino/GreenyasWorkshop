using UnityEngine;

public class SingletonMonobehaviour<T> : MonoBehaviour where T : Object
{
    public static T Instance { private set; get; }   

    private static void TryAutoInitialization()
    {
        Instance ??= FindAnyObjectByType<T>();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"There's already an instance of {Instance.name}");
            Destroy(gameObject);
            return;
        }

        TryAutoInitialization();
        OnInitialization();
    }

    protected virtual void OnInitialization() { }
}
