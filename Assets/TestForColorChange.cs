using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForColorChange : MonoBehaviour
{
    Renderer charRenderer;

    private void Start()
    {
        charRenderer = GetComponent<Renderer>();
        charRenderer.material.color = Color.red;
    }
}
