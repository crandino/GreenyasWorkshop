#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine.Assertions;

public static class SerializedPropertyExtensions
{
    public static void ChangeArraySizeAndInitialize<T>(this SerializedProperty array, int newSize) where T : new()
    {
        Assert.IsTrue(array.isArray, $"Serialized Property {array.name} is not an array");

        if (newSize > array.arraySize)
        {
            int diff = newSize - array.arraySize;
            for (int i = 0; i < diff; i++)
            {
                int lastElementIndex = array.arraySize;
                array.InsertArrayElementAtIndex(lastElementIndex);
                array.GetArrayElementAtIndex(lastElementIndex).boxedValue = new T();
            }
        }
        else if (newSize < array.arraySize)
            array.arraySize = newSize;
    }

    public static void ChangeArraySizeAndInitialize<T>(this SerializedProperty array, int newSize, params object[] constructorArgs)
    {
        Assert.IsTrue(array.isArray, $"Serialized Property {array.name} is not an array");

        if (newSize > array.arraySize)
        {
            int diff = newSize - array.arraySize;
            for (int i = 0; i < diff; i++)
            {
                int lastElementIndex = array.arraySize;
                array.InsertArrayElementAtIndex(lastElementIndex);
                array.GetArrayElementAtIndex(lastElementIndex).boxedValue = Activator.CreateInstance(typeof(T), constructorArgs);
            }
        }
        else if (newSize < array.arraySize)
            array.arraySize = newSize;
    }

} 
#endif
