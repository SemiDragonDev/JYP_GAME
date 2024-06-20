using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light directionalLight;

    [Tooltip("하루 시간에 대해 초 단위로 입력")]
    public float dayLength = 120f;

    private float currentTime = 0f;
    private float timeMultiplier;

    public bool isNight;
    public bool isDay;

    private void Start()
    {
        timeMultiplier = 360f / dayLength;
    }

    private void Update()
    {
        currentTime += Time.deltaTime * timeMultiplier;
        if (currentTime >= 360f) currentTime -= 360f;

        UpdateLighting(currentTime);
    }

    void UpdateLighting(float time)
    {
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(time, 170, 0));

        // Night Time
        if (time > 180)
        {
            directionalLight.intensity = Mathf.Lerp(3, 2, (time - 180) / 180);
            directionalLight.color = Color.Lerp(new Color(1.0f, 0.6f, 0.3f), new Color(0.2f, 0.2f, 0.6f), (time - 180) / 180);
            isNight = true;
            isDay = false;
        }
        // Day Time
        else
        {
            directionalLight.intensity = Mathf.Lerp(2, 3, time / 180);
            directionalLight.color = Color.Lerp(new Color(0.2f, 0.2f, 0.6f), new Color(1.0f, 0.95f, 0.8f), time / 180);
            isNight = false;
            isDay=true;
        }
    }
}
