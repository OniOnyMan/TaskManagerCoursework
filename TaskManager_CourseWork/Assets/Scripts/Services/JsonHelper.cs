using System;
using UnityEngine;

public class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
    public static T[] GetArrayFromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    public static string GetJsonFromArray<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.array = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static T GetFromJson<T>(string json)
    {
        var temp = GetArrayFromJson<T>(json);
        return temp.Length > 0 ? temp[0] : default(T);
    }
}