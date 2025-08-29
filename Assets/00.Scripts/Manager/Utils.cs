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
}
