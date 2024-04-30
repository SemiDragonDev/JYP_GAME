using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Transform light;
    //  timeScale = 1일 경우 하루는 360초
    [SerializeField] private float timeScale;
    private float timeStacked;

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
        float xRotation = timeStacked * timeScale;
        if (xRotation >= 360) timeStacked = 0;
        
        light.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
