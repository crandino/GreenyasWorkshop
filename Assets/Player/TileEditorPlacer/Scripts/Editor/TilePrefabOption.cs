using UnityEngine.UIElements;

public class TileResourceOption : VisualElement
{
    private TileResource tileResource;

    public TileResource TileResource
    {
        get => tileResource;
        set
        {
            tileResource = value;
            if (tileResource != null)
                style.backgroundImage = Background.FromSprite(tileResource.Icon);
        }
    }

    public void SizeChanged(GeometryChangedEvent ev)
    {
        layout.size.Set(ev.newRect.width, ev.newRect.height);
    }


    public new class UxmlFactory : UxmlFactory<TileResourceOption, UxmlTraits> { }

    // Traits class
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        protected UxmlAssetAttributeDescription<TileResource> resource = new() { name = "tile-resource" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            TileResourceOption tro = ((TileResourceOption)ve);

            tro.TileResource = resource.GetValueFromBag(bag, cc);

            tro.style.marginLeft = tro.style.marginRight = new StyleLength(5);

            tro.style.width = 50f;
            tro.style.height = 50f;

            tro.focusable = true;
            
        }
    }
}