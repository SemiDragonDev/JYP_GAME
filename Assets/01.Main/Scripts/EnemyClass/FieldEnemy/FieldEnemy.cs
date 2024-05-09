using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldEnemy : Enemy
{
    private float minDistToSpawn = 10f;
    private float maxDistToSpawn = 20f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 playerPos;

    public virtual void SpawnAtNight(int spawnNum, string name)
    {
        // ���� �㿡�� �����ȴ�.
        // �÷��̾� �߽� 10���� ���ٴ� �ָ�, 20���ֺ��ٴ� ����� ������ ������ �Ǳ�� ����
        // 5~10�� ���� �Ÿ� üũ �� 30���ֺ��� �ִٸ� Ǯ�� return
        // return�� ��ŭ �ٽ� ������ ������ ���� ������

        NavMesh.SamplePosition(playerPos, out NavMeshHit hit, maxDistToSpawn, layerMask);
    }

    public void BurnAtDay()
    {

    }
}
