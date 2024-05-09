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
        // 적은 밤에만 스폰된다.
        // 플레이어 중심 10유닛 보다는 멀리, 20유닛보다는 가까운 지역에 리스폰 되기로 지정
        // 5~10초 마다 거리 체크 후 30유닛보다 멀다면 풀에 return
        // return한 만큼 다시 지정된 지역에 랜덤 리스폰

        NavMesh.SamplePosition(playerPos, out NavMeshHit hit, maxDistToSpawn, layerMask);
    }

    public void BurnAtDay()
    {

    }
}
