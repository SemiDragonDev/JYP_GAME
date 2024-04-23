using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : MonoBehaviour
{
    public GameObject player;
    public Vector3 knockBackDir;
    public float stackedTime;
    public float knockBackDistance;
    public LayerMask layerMask;
    
    private WaitForSeconds waitTime;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 posToMove;

    public bool isAgentAvailable;

    private void Awake()
    {
        waitTime = new WaitForSeconds(Time.deltaTime);
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public IEnumerator CoroutineKnockBack()
    {
        agent.enabled = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        isAgentAvailable = false;

        stackedTime = 0f;
        knockBackDir = player.transform.forward;
        startPos = transform.position;
        endPos = (startPos + knockBackDir).normalized * knockBackDistance;

        while (stackedTime <= 1f)
        {
            yield return waitTime;
            stackedTime += Time.deltaTime;
            var posPerTime =  Vector3.Lerp(startPos, endPos, stackedTime);
            if(Physics.Raycast(posPerTime, Vector3.down, out RaycastHit hit, 10f, layerMask))
            {
                posToMove = hit.transform.position;
            }
            transform.position = posToMove;
        }

        agent.enabled = true;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    public void PlayingKnockBack()
    {
        StartCoroutine(CoroutineKnockBack());
        isAgentAvailable = true;
    }
}
