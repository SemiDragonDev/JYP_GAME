using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFPandTP : MonoBehaviour
{
    [SerializeField]
    private GameObject fpCamera;
    [SerializeField]
    private GameObject tpCamera;

    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
        fpCamera.SetActive(true);
        tpCamera.SetActive(false);
        camera.cullingMask = -1;
        PlayerToInvisible();
    }

    // Update is called once per frame
    void Update()
    {
        if (fpCamera.activeSelf == true && Input.GetKeyDown(KeyCode.F5))
        {
            fpCamera.SetActive (false);
            tpCamera.SetActive (true);
            camera.cullingMask = -1;
        }
        else if (tpCamera.activeSelf == true && Input.GetKeyDown(KeyCode.F5))
        {
            fpCamera.SetActive(true);
            tpCamera.SetActive(false);
            camera.cullingMask = -1;

            //Invoke("PlayerToInvisible", 0.5f);
            PlayerToInvisible();
        }
    }

    public void PlayerToInvisible()
    {
        camera.cullingMask = camera.cullingMask & ~(1 << LayerMask.NameToLayer("Player"));
    }
}
