using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerBody : MonoBehaviour
{
    [SerializeField]
    private Transform playerBody;

    private float mouseX;

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X");

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
