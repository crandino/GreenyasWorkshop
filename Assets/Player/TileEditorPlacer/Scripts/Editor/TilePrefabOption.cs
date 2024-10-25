using HexaLinks.Tile;
using UnityEngine;
using UnityEngine.UIElements;

public class TilePrefabOption : VisualElement
{
    public Tile TilePrefab { get; set; }

    public new class UxmlFactory : UxmlFactory<TilePrefabOption, UxmlTraits> { }

    // Traits class
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        protected UxmlAssetAttributeDescription<Tile> prefab = new() { name = "tile-prefab" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((TilePrefabOption)ve).TilePrefab = prefab.GetValueFromBag(bag, cc);
        }
    }    
}