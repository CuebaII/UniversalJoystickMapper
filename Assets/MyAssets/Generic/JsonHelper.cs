using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JsonHelper
{
    //Usage:
    //YouObject[] objects = JsonHelper.getJsonArray<YouObject> (jsonString);
    public static T[] getJsonArray<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.array;
    }
    //Usage:
    //string jsonString = JsonHelper.arrayToJson<YouObject>(objects);
    public static string arrayToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.array = array;
        //Converts and returns the wrapper of the class T to a JSON string with "pretty print" (nice indenting).
        return JsonUtility.ToJson(wrapper, true);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}