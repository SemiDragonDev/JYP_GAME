using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<FieldEnemy> fieldEnemiesList = new List<FieldEnemy>();

    private Vector3 playerPos;
    private string playerTag = "Player";

    private float minDistFromPlayer = 30f;
    private float maxDistFromPlayer = 50f;
    private float heightOfTheRay = 200f;
    private float distBtwPlayerAndPoint;

    private DayNightCycle dayNightCycle;

    private void Awake()
    {
        dayNightCycle = GetComponent<DayNightCycle>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            SpawnEnemy("Skeleton");
        }
    }

    // 플레이어 반경 30unit~50unit 사이 Point를 random으로 받아오자.
    // 그 위치의 x,z 값을 받고 y값 200 정도 위에서 아래 방향 Ray를 쏴 hit point를 받아온다.
    // 그 위치에 적 스폰시키기

    public void SpawnEnemy(string name)
    {
        var gameObj = ObjectPool.Instance.GetPooledObject(name);
        playerPos = GameObject.FindGameObjectWithTag(playerTag).transform.position;
        Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);

        //for (int i = 0; i < gameObj.defSize; i++) ;
        //{

        //}
        var randomPos = playerXZ + Random.insideUnitCircle * maxDistFromPlayer;
        distBtwPlayerAndPoint = (playerXZ - randomPos).magnitude;
        do
        {
            if (distBtwPlayerAndPoint < minDistFromPlayer)
            {
                randomPos = playerXZ + Random.insideUnitCircle * maxDistFromPlayer;
            }
            distBtwPlayerAndPoint = (playerXZ - randomPos).magnitude;
        } while (distBtwPlayerAndPoint < minDistFromPlayer);
        var rayPos = new Vector3(randomPos.x, heightOfTheRay, randomPos.y);
        if(Physics.Raycast(rayPos, Vector3.down, out RaycastHit hit, 300f, NavMesh.AllAreas))
        {
            gameObj.transform.position = hit.point;
        }
    }


}
