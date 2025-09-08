using System;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string Timer(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string timer = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);

        return timer;
    }

    public static T FindBase<T>(Transform parent, string key)
    {
        return parent.Find(key).GetComponent<T>();
    }

    public static void SetLayer(string layer, GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer(layer);   
    }
}
