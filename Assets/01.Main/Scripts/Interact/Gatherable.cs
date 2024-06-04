using UnityEngine;

public class Gatherable : MonoBehaviour, IInteractable
{
    public GameObject[] drops;

    private Vector3 dropPos = new Vector3(0f, 0.9f, 0f);

    public void Interact()
    {
        DropItems();
        Destroy(gameObject);
    }

    private void DropItems()
    {
        foreach (GameObject drop in drops)
        {
            Instantiate(drop, transform.position + dropPos, Quaternion.identity);
        }
    }
}
