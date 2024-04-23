using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : MonoBehaviour
{
    public GameObject player;
    public Vector3 knockBackDir;
    
    private WaitForSeconds waitTime;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private void Awake()
    {
        knockBackDir = player.transform.forward;
        waitTime = new WaitForSeconds(Time.deltaTime);
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public IEnumerator CoroutineKnockBack()
    {
        agent.enabled = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        while(true)
        {
            yield return waitTime;

        }
    }
}
