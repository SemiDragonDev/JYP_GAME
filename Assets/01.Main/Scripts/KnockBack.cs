using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Vector3 knockBackDir;
    private float stackedTime;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float knockBackDistance;
    [SerializeField]
    private float knockBackTime;
    private float makeItOne;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 hitPos;
    private Vector3 rayHeight = new Vector3(0f, 20f, 0f);

    private int animIDIsHit;

    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInParent<Animator>();
        makeItOne = 1f / knockBackTime;
        AssignAnimID();
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
        Debug.Log("�˹� ����");

        while (stackedTime <= knockBackTime)
        {
            stackedTime += Time.deltaTime;
            var posUpdate = Vector3.Lerp(startPos, endPos, stackedTime * makeItOne);
            if(Physics.Raycast(posUpdate, Vector3.down, out RaycastHit hit, 30f, layerMask))
            {
                //  hit.point�� ��� ray�� ���� ��ġ�� ��ȯ�Ѵ�. hit.transform.position�� �� ��� terrain ������Ʈ�� position ���� �������� �ȴ�.
                hitPos = hit.point;
            }
            agent.nextPosition = hitPos;
            yield return null;
        }
    }

    public void PlayingKnockBack()
    {
        SetKnockBackInfo();
        //animator.SetTrigger(animIDIsHit);
        StartCoroutine(CoroutineKnockBack());
    }

    private void AssignAnimID()
    {
        animIDIsHit = Animator.StringToHash("IsHit");
    }
}
