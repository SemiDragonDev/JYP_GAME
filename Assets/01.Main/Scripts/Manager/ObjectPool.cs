using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    // �������� Ǯ�� ����Ʈ�� ����
    [SerializeField] private List<PooledObject> pooledObjectsList;
    // Ǯ�� �������� ��� Dict ���� ��� Ǯ�� ã�� �� �˾ƾ���
    private Dictionary<string, List<PooledObject>> poolsDictionary = new Dictionary<string, List<PooledObject>>();
    // Ǯ�� ã���� �� �� ���� ������Ʈ�� ã�� Dict
    private Dictionary<List<PooledObject>, PooledObject> objectsDictionary = new Dictionary<List<PooledObject>, PooledObject>();

    // ������ Ǯ�� �������� ����;
    private List<PooledObject> list;

    private void Start()
    {
        SetupPool();
    }

    private void SetupPool()
    {
        PooledObject instance = null;

        // �ʿ��� Ǯ�� ������ŭ
        for(int i=0; i<pooledObjectsList.Count; i++)
        {
            // ���� ����, �޸� Ȯ��
            list = new List<PooledObject>();

            // Default ũ�⸸ŭ GameObject�� Instantiate �� �� ���ÿ� Push��
            for (int j=0; j<pooledObjectsList[i].defSize; j++)
            {
                instance = Instantiate(pooledObjectsList[i]);
                instance.Pool = this;
                instance.gameObject.SetActive(false);
                list.Add(instance);
            }

            //Dict �� Ǯ�� ���� (objectName�� Key�� ����� ���ϴ� Ǯ�� ã�´�)
            poolsDictionary.Add(pooledObjectsList[i].objectName, list);
            objectsDictionary.Add(list, instance);
        }

        //for(int i = 0; i<defPoolSize; i++)
        //{
        //    instance = Instantiate(objectToPool);
        //    instance.Pool = this;
        //    instance.gameObject.SetActive(false);
        //    stack.Push(instance);
        //}
    }

    // ������Ʈ �̸����� Pool�� ã�� List���� ��������
    public PooledObject GetPooledObject(string objName)
    {
        poolsDictionary.TryGetValue(objName, out var poolList);

        for (int i = 0; i < poolList.Count; i++)
        {
            if (!poolList[i].gameObject.activeSelf)
            {
                PooledObject nextInstance = poolList[i];
                nextInstance.gameObject.SetActive(true);
                return nextInstance;
            }
        }
        objectsDictionary.TryGetValue(poolList, out var pooledObject);
        PooledObject newInstance = Instantiate(pooledObject);
        newInstance.Pool = this;
        return newInstance;
    }

    // ����� Object�� ��ȯ
    public void ReturnToPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // �˰����ϴ� Pool�� DefaultSize�� ��ȯ
    public int GetDefSize(string objName)
    {
        poolsDictionary.TryGetValue(objName, out var poolList);
        objectsDictionary.TryGetValue(poolList, out var pooledObject);
        return pooledObject.defSize;
    }

    public void CountActiveObjectsInList(string objName)
    {
        
    }
}
