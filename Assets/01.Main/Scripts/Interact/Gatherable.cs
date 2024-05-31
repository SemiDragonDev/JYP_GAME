using UnityEngine;

public class Gatherable : MonoBehaviour, IInteractable
{
    public GameObject[] drops; // Objects to drop when this is gathered

    public void Interact()
    {
        DropItems();
        Destroy(gameObject);
    }

    private void DropItems()
    {
        foreach (GameObject drop in drops)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }
}
