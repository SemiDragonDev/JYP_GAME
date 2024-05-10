using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldEnemy : Enemy
{
    public string Name { get; private set; }
    public FieldEnemy(string name)
    {
        Name = name;
    }

    private float minDistToSpawn = 10f;
    private float maxDistToSpawn = 20f;
    private string playerTag = "Player";
    private string layerName = "Ground";

    public virtual void SpawnAtNight(int spawnNum, string name)
    {
        // ���� �㿡�� �����ȴ�.
        // �÷��̾� �߽� 10���� ���ٴ� �ָ�, 20���ֺ��ٴ� ����� ������ ������ �Ǳ�� ����
        // 5~10�� ���� �Ÿ� üũ �� 30���ֺ��� �ִٸ� Ǯ�� return
        // return�� ��ŭ �ٽ� ������ ������ ���� ������
        var playerPos = GameObject.FindGameObjectWithTag(playerTag).transform.position;
        NavMesh.SamplePosition(playerPos, out NavMeshHit hit, maxDistToSpawn, LayerMask.NameToLayer(layerName));
    }

    public void BurnAtDay()
    {

    }
}
