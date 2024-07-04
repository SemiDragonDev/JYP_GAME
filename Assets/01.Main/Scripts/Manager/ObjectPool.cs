using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    // 여러개의 풀을 리스트로 관리
    [SerializeField] private List<PooledObject> pooledObjectsList;
    // 풀이 여러개일 경우 Dict 통해 어느 풀을 찾는 지 알아야함
    private Dictionary<string, List<PooledObject>> poolsDict = new Dictionary<string, List<PooledObject>>();
    // 풀을 찾았을 때 그 안의 오브젝트를 찾을 Dict
    private Dictionary<List<PooledObject>, PooledObject> objectsDict = new Dictionary<List<PooledObject>, PooledObject>();

    private List<PooledObject> list;

    private void Start()
    {
        SetupPool();
    }

    private void SetupPool()
    {
        PooledObject instance = null;

        // 필요한 풀의 개수만큼
        for(int i=0; i<pooledObjectsList.Count; i++)
        {
            // 스택 생성, 메모리 확보
            list = new List<PooledObject>();

            // Default 크기만큼 GameObject를 Instantiate 후 각 스택에 Push함
            for (int j=0; j<pooledObjectsList[i].defSize; j++)
            {
                instance = Instantiate(pooledObjectsList[i]);
                instance.Pool = this;
                instance.gameObject.SetActive(false);
                list.Add(instance);
            }

            //Dict 에 풀을 저장 (objectName을 Key로 사용해 원하는 풀을 찾는다)
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

    // 오브젝트 이름으로 Pool을 찾아 List에서 꺼내오기
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

    // 사용한 Object는 반환
    public void ReturnToPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // 알고자하는 Pool의 DefaultSize를 반환
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
