using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<FieldEnemy> fieldEnemiesList = new List<FieldEnemy>();

    private Vector3 playerPos;
    private string playerTag = "Player";
    private string skeletonTag = "Skeleton";
    private string burnEffectTag = "BurnEffect";

    private float minDistFromPlayer = 30f;
    private float maxDistFromPlayer = 50f;
    private float heightOfTheRay = 200f;
    private float distBtwPlayerAndPoint;

    private List<GameObject> listOfGameObj = new List<GameObject>();

    private DayNightCycle dayNightCycle;

    void Start()
    {
        dayNightCycle = GetComponent<DayNightCycle>();
    }

    private void Update()
    {
        if (dayNightCycle.isNight)
        {
            SpawnEnemy(skeletonTag);
        }
        else
        {
            BurnEnemy(skeletonTag);
        }
    }

    // 플레이어 반경 30unit~50unit 사이 Point를 random으로 받아오자.
    // 그 위치의 x,z 값을 받고 y값 200 정도 위에서 아래 방향 Ray를 쏴 hit point를 받아온다.
    // 그 위치에 적 스폰시키기

    public void SpawnEnemy(string name)
    {
        // 현재 Active 상태인 풀의 Object의 Count와
        // 그 Object의 풀의 Default Size를 받아온다
        ObjectPool.Instance.CountActiveObjectsInList(name, out int count);
        int defaultSize = ObjectPool.Instance.GetDefSize(name);

        playerPos = GameObject.FindGameObjectWithTag(playerTag).transform.position;
        Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);

        // 만약 Active 되어 있는 수가 Default Size보다 적은 경우 그 수만큼 Pool에서 불러와 스폰시킨다.
        if (count < defaultSize)
        {
            for (int i = 0; i < defaultSize - count; i++)
            {
                var randomPos = playerXZ + UnityEngine.Random.insideUnitCircle * maxDistFromPlayer;
                distBtwPlayerAndPoint = (playerXZ - randomPos).magnitude;
                var gameObj = ObjectPool.Instance.GetPooledObject(name);
                do
                {
                    if (distBtwPlayerAndPoint < minDistFromPlayer)
                    {
                        randomPos = playerXZ + UnityEngine.Random.insideUnitCircle * maxDistFromPlayer;
                    }
                    distBtwPlayerAndPoint = (playerXZ - randomPos).magnitude;
                } while (distBtwPlayerAndPoint < minDistFromPlayer);
                var rayPos = new Vector3(randomPos.x, heightOfTheRay, randomPos.y);
                if (Physics.Raycast(rayPos, Vector3.down, out RaycastHit hit, 500f, NavMesh.AllAreas))
                {
                    gameObj.transform.position = hit.point;
                }
            }
        }
    }

    // 낮이 되면 태우는 메서드
    public void BurnEnemy(string name)
    {
        int size = ObjectPool.Instance.GetDefSize(name);
        List<PooledObject> list = ObjectPool.Instance.GetListOfPool(name);
        for (int i = 0; i < size; i++)
        {
            var gameObj = ObjectPool.Instance.GetPooledObject(burnEffectTag);
            gameObj.transform.position = list[i].transform.position;
        }
    }
}
