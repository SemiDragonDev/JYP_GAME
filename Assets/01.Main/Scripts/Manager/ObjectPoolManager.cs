using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectPoolManager : MonoBehaviour
{
    [Serializable]
    public class ThingsToPool
    {
        public string name;
        public GameObject prefab;
        public int defaultSize;
        public int maxSize;

        private int curSize;
        public int CurSize
        {
            get { return curSize; }
            set { curSize = value; }
        }
    }

    public static ObjectPoolManager instance;

    public ThingsToPool[] itemsToPool;
    public List<GameObject>[] objectsPoolList;

    private void Awake()
    {
        instance = this;

        objectsPoolList = new List<GameObject>[itemsToPool.Length];

        for(int i=0; i< itemsToPool.Length; i++)
        {
            objectsPoolList[i] = new List<GameObject> ();

            int index = 0;
            for(int j=0; j < itemsToPool[i].defaultSize; j++)
            {
                GameObject go = Instantiate(itemsToPool[i].prefab);

                string suffix = "_" + index;
                ReturnToPool(i, go, suffix);
                ++index;
            }
        }
    }

    private GameObject GetFromPool(string nameToGet)
    {
        for(int itemIdx = 0;  itemIdx < objectsPoolList.Length; itemIdx++)
        {
            if (itemsToPool[itemIdx].prefab.name == nameToGet)
            {
                int listIdx = 0;
                for (listIdx = 0; listIdx < objectsPoolList[itemIdx].Count; listIdx++)
                {
                    if (objectsPoolList[itemIdx][listIdx] == null)
                        return null;
                    if (objectsPoolList[itemIdx][listIdx].activeInHierarchy == false)
                        return objectsPoolList[itemIdx][listIdx];
                }
                break;
            }
        }
        return null;
    }

    private void ReturnToPool(int i, GameObject go, string suffix)
    {
        go.name += suffix;
        go.SetActive(false);
        go.transform.parent = transform;
        objectsPoolList[i].Add(go);
    }
}
