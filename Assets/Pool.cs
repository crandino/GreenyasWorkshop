using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "StrengthIndicatorPool")]
public class Pool : ScriptableObject
{
    [SerializeField]
    private StrenghtIndicator indicatorTemplate;

    private ObjectPool<StrenghtIndicator> pool = null;

    public void InitPool()
    {
        pool = new(OnCreate, null, OnRelease);
        pool.Clear();
    }

    public StrenghtIndicator Get()
    {
        return pool.Get();
    }

    public void Release(StrenghtIndicator indicator)
    {
        pool.Release(indicator);
    }

    private StrenghtIndicator OnCreate()
    {
        return Instantiate(indicatorTemplate);
    }

    private void OnRelease(StrenghtIndicator indicator)
    {
        indicator.SetText("");
        indicator.TransformToFollow = null;
    }    

    private void OnDestroy()
    {
        pool.Clear();
    }

#if UNITY_EDITOR
    [ContextMenu("Show Info")]
    private void ShowInformation()
    {
        Debug.Log($"CountAll: {pool.CountAll}: {pool.CountActive} active and {pool.CountInactive} inactive");
    }

#endif
}
