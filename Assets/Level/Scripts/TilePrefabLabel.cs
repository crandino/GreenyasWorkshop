using UnityEngine;
using UnityEngine.UIElements;

public class TilePrefabLabel : Label
{
    public new class UxmlFactory : UxmlFactory<TilePrefabLabel, UxmlTraits> { }

    // Traits class
    public new class UxmlTraits : Label.UxmlTraits
    {
        public UxmlStringAttributeDescription m_PrefabPath = new() { name = "assetPath" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            string path = "";
            if (m_PrefabPath.TryGetValueFromBag(bag, cc, ref path))
            {
                ((TilePrefabLabel)ve).AssetPrefab = path;
            }
        }
    }

    [SerializeField]
    public string AssetPrefab { get; set; }
}