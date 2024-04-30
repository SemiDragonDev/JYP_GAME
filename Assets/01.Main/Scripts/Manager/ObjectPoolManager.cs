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

    IObjectPool<ParticleSystem> pool;

    public IObjectPool<ParticleSystem> Pool
    {
        get
        {
            if(pool == null)
            {
                pool = new ObjectPool<ParticleSystem>(CreatePooledObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize, maxPoolSize);
            }
            return pool;
        }
    }

    private ParticleSystem CreatePooledObject()
    {
        throw new NotImplementedException();
    }

    private void OnTakeFromPool(ParticleSystem system)
    {
        throw new NotImplementedException();
    }

    private void OnReturnedToPool(ParticleSystem system)
    {
        throw new NotImplementedException();
    }

    private void OnDestroyPoolObject(ParticleSystem system)
    {
        throw new NotImplementedException();
    }
}
