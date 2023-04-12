using System.Collections.Generic;

public static class StackExtensions 
{
    public static void Push<T>(this Stack<T> stack, T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            stack.Push(array[i]);
        }
    }
}
