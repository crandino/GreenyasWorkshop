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
}