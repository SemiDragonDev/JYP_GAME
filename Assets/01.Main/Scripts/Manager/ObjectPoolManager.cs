using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public int defaultPoolSize = 5;
    public int maxPoolSize = 10;

    public IObjectPool<GameObject> Pool { get; private set; }

    [SerializeField]
    private List<GameObject> listOfPoolObjects = new List<GameObject>();

    private Dictionary<GameObject, IObjectPool<GameObject>> poolDict = new Dictionary<GameObject, IObjectPool<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }

    private void Init()
    {
        Pool = new ObjectPool<GameObject>(CreatePooledObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, defaultPoolSize, maxPoolSize);

        for(int i=0; i< listOfPoolObjects.Count; i++)
        {
            poolDict.Add(listOfPoolObjects[i], Pool);
        }
    }

    private GameObject CreatePooledObject()
    {
        throw new NotImplementedException();
    }

    private void OnTakeFromPool(GameObject @object)
    {
        throw new NotImplementedException();
    }

    private void OnReturnedToPool(GameObject @object)
    {
        throw new NotImplementedException();
    }

    private void OnDestroyPoolObject(GameObject @object)
    {
        throw new NotImplementedException();
    }
}
