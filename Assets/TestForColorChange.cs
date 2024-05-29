using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForColorChange : MonoBehaviour
{
    Renderer[] charRenderer;

    private void Start()
    {
        charRenderer = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in charRenderer)
        {
            r.material.color = Color.red;
        }
    }
}
