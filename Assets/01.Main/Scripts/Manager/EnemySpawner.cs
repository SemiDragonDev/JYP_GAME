using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<FieldEnemy> fieldEnemiesList = new List<FieldEnemy>();

    private Vector3 playerPos;
    private string playerTag = "Player";

    private float minDistFromPlayer = 10f;
    private float maxDistFromPlayer = 20f;
    private float heightOfTheRay = 200f;

    private void Start()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            SpawnEnemy("Skeleton");
        }
    }

    // 플레이어 반경 10unit~20unit 사이 Point를 random으로 받아오자.
    // 그 위치의 x,z 값을 받고 y값 200 정도 위에서 아래 방향 Ray를 쏴 hit point를 받아온다.
    // 그 위치에 적 스폰시키기

    public void SpawnEnemy(string name)
    {
        playerPos = GameObject.FindGameObjectWithTag(playerTag).transform.position;
        Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);
        var randomPos = playerXZ + Random.insideUnitCircle * maxDistFromPlayer;
        var rayPos = new Vector3(randomPos.x, heightOfTheRay, randomPos.y);
        if(Physics.Raycast(rayPos, Vector3.down, out RaycastHit hit, 300f, NavMesh.AllAreas))
        {
            var gameObj = ObjectPool.Instance.GetPooledObject(name);
            gameObj.transform.position = hit.point;
        }
    }
}
