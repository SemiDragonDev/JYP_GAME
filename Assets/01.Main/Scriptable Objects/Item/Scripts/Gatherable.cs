using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour, IGatherable
{
    [SerializeField]
    private GameObject resourcePrefab;

    public void Gather()
    {
        Destroy(transform.gameObject);
        Instantiate(resourcePrefab, transform.transform.position + resourcePrefab.transform.position, resourcePrefab.transform.rotation);
    }
}
