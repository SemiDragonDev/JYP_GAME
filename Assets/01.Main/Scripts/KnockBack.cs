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

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 hitPos;
    private Vector3 rayHeight = new Vector3(0f, 20f, 0f);

    private NavMeshAgent agent;
    private Rigidbody rb;

    private void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        rb = GetComponentInParent<Rigidbody>();
    }

    public void SetKnockBackInfo()
    {
        stackedTime = 0f;
        knockBackDir = player.transform.forward;
        startPos = transform.position;
        endPos = startPos + knockBackDir * knockBackDistance;
        startPos += rayHeight;
        endPos += rayHeight;
    }

    public IEnumerator CoroutineKnockBack()
    {
        Debug.Log("넉백 실행");

        while (stackedTime <= 0.2f)
        {
            stackedTime += Time.deltaTime;
            var posUpdate = Vector3.Lerp(startPos, endPos, stackedTime * 5f);
            if(Physics.Raycast(posUpdate, Vector3.down, out RaycastHit hit, 30f, layerMask))
            {
                //  hit.point를 써야 ray가 맞은 위치를 반환한다. hit.transform.position을 쓸 경우 terrain 오브젝트의 position 값을 가져오게 된다.
                hitPos = hit.point;
            }
            agent.nextPosition = hitPos;
            yield return null;
        }
    }

    public void PlayingKnockBack()
    {
        SetKnockBackInfo();
        StartCoroutine(CoroutineKnockBack());
    }
}
