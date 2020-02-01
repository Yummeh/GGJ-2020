using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTools
{
    public static T WrapAroundArrayReturn<T>(List<T> list, int index)
    {
        int newIndex = index;

        if (newIndex > list.Count - 1)
            newIndex = newIndex % list.Count;
        else if (newIndex < 0)
            newIndex = list.Count + newIndex;

        return list[newIndex];
    }

    public static int WrapAroundArrayIndex<T>(List<T> list, int index)
    {
        int newIndex = index;

        if (newIndex > list.Count - 1)
            newIndex = newIndex % list.Count;
        else if (newIndex < 0)
            newIndex = list.Count + newIndex;

        return newIndex;
    }

    public static int ReturnFirstEmptyValueIndex<T>(T[] array, T emptyValue)
    {   
        return ReturnIndexOfObject(array, emptyValue);
    }

    public static int ReturnIndexOfObject<T>(T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++)
            if (array[i].Equals(value))
                return i;

        // Failed
        return -1;
    }

    /// <summary>
    /// Return the vector2 on the x and z axis of a vector3
    /// </summary>
    public static Vector2 GetVector2FromVector3(Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    /// <summary>
    /// Return the vector3 with the values of a vector2 on the x and z axis
    /// </summary>
    public static Vector3 MakeVector3FromVector2(Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}
