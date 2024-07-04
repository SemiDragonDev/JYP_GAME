using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    // �������� Ǯ�� ����Ʈ�� ����
    [SerializeField] private List<PooledObject> pooledObjectsList;
    // Ǯ�� �������� ��� Dict ���� ��� Ǯ�� ã�� �� �˾ƾ���
    private Dictionary<string, List<PooledObject>> poolsDict = new Dictionary<string, List<PooledObject>>();
    // Ǯ�� ã���� �� �� ���� ������Ʈ�� ã�� Dict
    private Dictionary<List<PooledObject>, PooledObject> objectsDict = new Dictionary<List<PooledObject>, PooledObject>();

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
            poolsDict.Add(pooledObjectsList[i].objectName, list);
            objectsDict.Add(list, instance);
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
        poolsDict.TryGetValue(objName, out var poolList);

        for (int i = 0; i < poolList.Count; i++)
        {
            if (!poolList[i].gameObject.activeSelf)
            {
                PooledObject nextInstance = poolList[i];
                nextInstance.gameObject.SetActive(true);
                return nextInstance;
            }
        }
        objectsDict.TryGetValue(poolList, out var pooledObject);
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
        poolsDict.TryGetValue(objName, out var poolList);
        objectsDict.TryGetValue(poolList, out var pooledObject);
        return pooledObject.defSize;
    }

    public void CountActiveObjectsInList(string objName, out int count)
    {
        count = 0;
        poolsDict.TryGetValue(objName, out var pooledObjectsList);
        for (int i = 0; i < pooledObjectsList.Count; i++)
        {
            if (pooledObjectsList[i].gameObject.activeSelf) count++;
        }
    }

    public List<PooledObject> GetListOfPool(string objName)
    {
        poolsDict.TryGetValue(objName, out var pooledObjectsList);
        return pooledObjectsList;
    }
}
