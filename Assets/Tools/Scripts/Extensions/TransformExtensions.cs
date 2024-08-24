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

    public static Transform GetTransformUpUntil<T>(this Transform t) where T : Component
    {
        do
        {
            if (t.gameObject.GetComponent<T>() != null)
                return t;
        }
        while ((t = t.parent) != null);

        return null;
        
    }
}