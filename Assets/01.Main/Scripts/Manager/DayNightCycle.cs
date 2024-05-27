using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Transform sunLight;
    //  timeScale = 1일 경우 하루는 360초
    [SerializeField] private float timeSpeed;
    [SerializeField] [Range(0f,360f)] [Tooltip("200이상 350미만이 밤입니다.")] private float timeStacked;
    public static float xRotation;

    public bool isNight;
    public bool isDay;

    private void Start()
    {
        sunLight.transform.localRotation = Quaternion.identity;
    }
    private void Update()
    {
        LightRotation();
    }

    private void LightRotation()
    {
        timeStacked += Time.deltaTime;
        xRotation = timeStacked * timeSpeed;
        if (xRotation >= 360) timeStacked = 0;
        if (xRotation >= 190 && xRotation <350) isNight = true;
        else isNight = false;
        if (xRotation >= 350 && xRotation < 190) isDay = true;
        else isDay = false;
        SetTimeForDebug();
        sunLight.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void SetTimeForDebug()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            timeStacked = 189f;
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            timeStacked = 349f;
        }
    }
}
