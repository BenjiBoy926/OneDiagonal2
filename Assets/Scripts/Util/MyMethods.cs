using UnityEngine;
using System.Collections;

public static class MyMethods
{
    public static Type[,] DeepCopy<Type>(Type[,] source)
    {
        Type[,] copy = new Type[source.GetLength(0), source.GetLength(1)];

        for(int i = 0; i < copy.GetLength(0); i++)
        {
            for(int j = 0; j < copy.GetLength(1); j++)
            {
                copy[i, j] = source[i, j];
            }
        }

        return copy;
    }
}
