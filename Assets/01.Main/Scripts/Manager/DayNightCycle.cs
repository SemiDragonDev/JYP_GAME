using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Transform sunLight;
    //  timeScale = 1�� ��� �Ϸ�� 360��
    [SerializeField] private float timeSpeed;
    [SerializeField] [Range(0f,360f)] [Tooltip("200�̻� 350�̸��� ���Դϴ�.")] private float timeStacked;
    public float xRotation;

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
        if (xRotation >= 199 && xRotation <200) isNight = true;
        else isNight = false;
        if (xRotation >= 349 && xRotation < 350) isDay = true;
        else isDay = false;
        sunLight.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
