using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private const string TimeFormat = "mm':'ss";
    public static float TimeCount = 0;
    public TMP_Text textTime;

    void Update()
    {
        //Si el juego esta pausado no actualizamos el tiempo
        if (!PlayerAvatar.IsPaused)
        {
            TimeCount += Time.deltaTime;
            textTime.text = "Time: " + TimeSpan.FromSeconds(TimeCount).ToString(TimeFormat);
        }
    }
}
