[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int itemCount;

    public InventoryItem(Item item, int itemCount)
    {
        this.item = item;
        this.itemCount = itemCount;
    }
}
