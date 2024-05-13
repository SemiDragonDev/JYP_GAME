using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<FieldEnemy> fieldEnemiesList = new List<FieldEnemy>();

    private Vector3 playerPos;
    private string playerTag = "Player";

    private float minDistFromPlayer = 20f;
    private float maxDistFromPlayer = 50f;
    private float heightOfTheRay = 200f;
    private float distBtwPlayerAndPoint;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            SpawnEnemy("Skeleton");
        }
    }

    // �÷��̾� �ݰ� 10unit~20unit ���� Point�� random���� �޾ƿ���.
    // �� ��ġ�� x,z ���� �ް� y�� 200 ���� ������ �Ʒ� ���� Ray�� �� hit point�� �޾ƿ´�.
    // �� ��ġ�� �� ������Ű��

    public void SpawnEnemy(string name)
    {
        playerPos = GameObject.FindGameObjectWithTag(playerTag).transform.position;
        Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);
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
            var gameObj = ObjectPool.Instance.GetPooledObject(name);
            gameObj.transform.position = hit.point;
        }
    }
}
