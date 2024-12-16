using System.Runtime.CompilerServices;
using UnityEngine;

public static class TransformExtensions
{
    public static Transform GetTransformRoot(this Transform t)
    {
        while(t.parent != null) 
            t = t.parent;
        return t;
    }

    public static T GetTransformUpUntil<T>(this Transform t) where T : Component
    {
        T component;

        do
        {
            component = t.gameObject.GetComponent<T>();
            if (component != null)
                return component;
        }
        while ((t = t.parent) != null);

        return null;
        
    }
}