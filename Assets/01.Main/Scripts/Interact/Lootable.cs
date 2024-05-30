using UnityEngine;

public class Lootable : MonoBehaviour, IInteractable
{
    public Item item;
    public int count = 1;

    public void Interact()
    {
        bool wasPickedUp = Inventory.Instance.AddItem(item, count);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }

    public string GetInteractType()
    {
        return "Lootable";
    }
}
