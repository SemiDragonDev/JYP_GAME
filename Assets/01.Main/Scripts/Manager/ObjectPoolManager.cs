using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//  OnParticlaSystemStopped 이벤트 호출시 파티클 시스템을 풀에 반환해주는 컴퍼넌트
[RequireComponent(typeof(ParticleSystem))]
public class ReturnToPool : MonoBehaviour
{
    public ParticleSystem system;
    public IObjectPool<ParticleSystem> pool;

    private void Start()
    {
        system = GetComponent<ParticleSystem>();
        var main = system.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnParticleSystemStopped()
    {
        pool.Release(system);
    }
}

public class ObjectPoolManager : MonoBehaviour
{
    public bool collectionChecks = true;
    public int maxPoolSize = 10;

    [SerializeField] private List<GameObject> listOfObjects = new List<GameObject>();

    IObjectPool<ParticleSystem> pool;

    public IObjectPool<ParticleSystem> Pool
    {
        get
        {
            if(pool == null)
            {
                pool = new ObjectPool<ParticleSystem>(CreatePooledObject, OnGetFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 5, maxPoolSize);
            }
            return pool;
        }
    }

    private ParticleSystem CreatePooledObject()
    {
        throw new NotImplementedException();
    }

    private void OnGetFromPool(ParticleSystem system)
    {
        system.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(ParticleSystem system)
    {
        system.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(ParticleSystem system)
    {
        Destroy(system.gameObject);
    }
}
