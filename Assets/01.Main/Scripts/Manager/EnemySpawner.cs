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

    // �÷��̾� �ݰ� 10unit~20unit ���� Point�� random���� �޾ƿ���.
    // �� ��ġ�� x,z ���� �ް� y�� 200 ���� ������ �Ʒ� ���� Ray�� �� hit point�� �޾ƿ´�.
    // �� ��ġ�� �� ������Ű��

    public Vector3 GetRandomRayPointAroundPlayer()
    {
        playerPos = GameObject.FindGameObjectWithTag(playerTag).transform.position;
        Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);
        var randomPos = playerXZ + Random.insideUnitCircle * maxDistFromPlayer;
        var rayPos = new Vector3(randomPos.x, heightOfTheRay, randomPos.y);
        return rayPos;
    }
}
