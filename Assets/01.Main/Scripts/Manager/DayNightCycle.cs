using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Transform light;
    //  timeScale = 1일 경우 하루는 360초
    [SerializeField] private float timeSpeed;
    [SerializeField] private float timeStacked;

    public bool isNight;

    private void Start()
    {
        light.transform.localRotation = Quaternion.identity;
    }
    private void Update()
    {
        LightRotation();
    }

    private void LightRotation()
    {
        timeStacked += Time.deltaTime;
        float xRotation = timeStacked * timeSpeed;
        if (xRotation >= 360) timeStacked = 0;
        if (xRotation >= 200 && xRotation <350) isNight = true;
        else isNight = false;
        light.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
