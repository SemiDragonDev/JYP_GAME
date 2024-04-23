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

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 posToMove;

    private void Awake()
    {
        waitTime = new WaitForSeconds(Time.deltaTime);
    }

    public IEnumerator CoroutineKnockBack()
    {
        Debug.Log("³Ë¹é ½ÇÇà");

        stackedTime = 0f;
        knockBackDir = player.transform.forward;
        startPos = transform.position;
        endPos = (startPos + knockBackDir).normalized * knockBackDistance;

        while (stackedTime <= 1f)
        {
            
            stackedTime += Time.deltaTime;
            var posPerTime =  Vector3.Lerp(startPos, endPos, stackedTime);
            if(Physics.Raycast(posPerTime, Vector3.down, out RaycastHit hit, 10f, layerMask))
            {
                posToMove = hit.transform.position;
            }
            transform.position = posToMove;
            yield return waitTime;
        }
    }

    public void PlayingKnockBack()
    {
        StartCoroutine(CoroutineKnockBack());
    }
}
