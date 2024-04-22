using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : MonoBehaviour
{
    public GameObject player;
    public Vector3 knockBackDir;
    WaitForSeconds waitTime;

    private void Awake()
    {
        knockBackDir = player.transform.forward;
        waitTime = new WaitForSeconds(Time.deltaTime);  
    }

    public IEnumerator CoroutineKnockBack()
    {
        yield return waitTime;
    }
}
