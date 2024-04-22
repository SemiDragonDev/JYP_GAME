using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    Camera cam;
    [SerializeField]
    float fixedWidth = 1920f;
    [SerializeField]
    float fixedHeight = 1080f;
    float fixedAspectRatio;

    private void Start()
    {
        cam = GetComponent<Camera>();
        fixedAspectRatio = fixedWidth / fixedHeight;
        var curAspectRatio = (float)Screen.width / (float)Screen.height;

        if(curAspectRatio == fixedAspectRatio)
        {
            cam.rect = new Rect(0f, 0f, 1f, 1f);
        }
        else if(curAspectRatio > fixedAspectRatio)
        {
            float width = fixedAspectRatio / curAspectRatio;
            float x = (1f - width) / 2f;
            cam.rect = new Rect(x, 0f, width, 1f);
        }
        else if(curAspectRatio < fixedAspectRatio)
        {
            float height = curAspectRatio / fixedAspectRatio;
            float y = (1f - height) / 2f;
            cam.rect = new Rect(0f, y, 1f, height);
        }
    }
}
