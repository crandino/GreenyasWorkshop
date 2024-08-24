using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math 
{
    public static float Remap(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin));
    }
}
