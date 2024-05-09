using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    // �������� Ǯ�� ����Ʈ�� ����
    [SerializeField] private List<PooledObject> pooledObjectsList;
    // Ǯ�� �������� ��� Dict ���� ��� Ǯ�� ã�� �� �˾ƾ���
    private Dictionary<string, Stack<PooledObject>> poolsDict = new Dictionary<string, Stack<PooledObject>>();
    // Ǯ�� ã���� �� �� ���� ������Ʈ�� ã�� Dict
    private Dictionary<Stack<PooledObject>, PooledObject> objectsDict = new Dictionary<Stack<PooledObject>, PooledObject>();

    // ������ Ǯ�� �������� ����;
    private Stack<PooledObject> stack;

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
            stack = new Stack<PooledObject>();

            // Default ũ�⸸ŭ GameObject�� Instantiate �� �� ���ÿ� Push��
            for (int j=0; j<pooledObjectsList[i].defSize; j++)
            {
                instance = Instantiate(pooledObjectsList[i]);
                instance.Pool = this;
                instance.gameObject.SetActive(false);
                stack.Push(instance);
            }

            //Dict �� Ǯ�� ���� (objectName�� Key�� ����� ���ϴ� Ǯ�� ã�´�)
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

    // ���ÿ��� ������Ʈ�� ������ �� �� �޼��带 ����
    public PooledObject GetPooledObject(string objName)
    {
        //// pool�� �۾Ƽ� stack�� ����ְ� �Ǹ�, �� object�� instantiate ����
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


        // �׷��� ������, �׳� ���ÿ��� �ϳ� �����´�.
        PooledObject nextInstance = poolStack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    // ���ÿ� �ٽ� ������Ʈ�� ���� �� Release �޼��带 ����� �� �Լ��� ȣ���Ѵ�
    public void ReturnToPool(PooledObject pooledObject)
    {
        poolsDict.TryGetValue(pooledObject.name.Split('(')[0], out var poolStack);
        poolStack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}
