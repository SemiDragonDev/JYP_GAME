using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    // 여러개의 풀을 리스트로 관리
    [SerializeField] private List<PooledObject> pooledObjectsList;
    // 풀이 여러개일 경우 Dict 통해 어느 풀을 찾는 지 알아야함
    private Dictionary<string, Stack<PooledObject>> poolsDict = new Dictionary<string, Stack<PooledObject>>();
    // 풀을 찾았을 때 그 안의 오브젝트를 찾을 Dict
    private Dictionary<Stack<PooledObject>, PooledObject> objectsDict = new Dictionary<Stack<PooledObject>, PooledObject>();

    // 각각의 풀은 스택으로 형성;
    private Stack<PooledObject> stack;

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
            stack = new Stack<PooledObject>();

            // Default 크기만큼 GameObject를 Instantiate 후 각 스택에 Push함
            for (int j=0; j<pooledObjectsList[i].defSize; j++)
            {
                instance = Instantiate(pooledObjectsList[i]);
                instance.Pool = this;
                instance.gameObject.SetActive(false);
                stack.Push(instance);
            }

            //Dict 에 풀을 저장 (objectName을 Key로 사용해 원하는 풀을 찾는다)
            poolsDict.Add(pooledObjectsList[i].objectName, stack);
            objectsDict.Add(stack, instance);
        }

        //for(int i = 0; i<defPoolSize; i++)
        //{
        //    instance = Instantiate(objectToPool);
        //    instance.Pool = this;
        //    instance.gameObject.SetActive(false);
        //    stack.Push(instance);
        //}
    }

    // 스택에서 오브젝트를 꺼내올 때 이 메서드를 쓴다
    public PooledObject GetPooledObject(string objName)
    {
        //// pool이 작아서 stack이 비어있게 되면, 새 object를 instantiate 해줌
        //if (stack.Count == 0)
        //{
        //    PooledObject newInstance = Instantiate(objectToPool);
        //    newInstance.Pool = this;
        //    return newInstance;
        //}
        poolsDict.TryGetValue(objName, out var poolStack);
        if(poolStack.Count == 0)
        {
            objectsDict.TryGetValue(poolStack, out var pooledObject);
            PooledObject newInstance = Instantiate(pooledObject);
            newInstance.Pool = this;
            return newInstance;
        }


        // 그렇지 않으면, 그냥 스택에서 하나 꺼내온다.
        PooledObject nextInstance = poolStack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    // 스택에 다시 오브젝트를 넣을 때 Release 메서드를 사용해 이 함수를 호출한다
    public void ReturnToPool(PooledObject pooledObject)
    {
        poolsDict.TryGetValue(pooledObject.name.Split('(')[0], out var poolStack);
        poolStack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}
