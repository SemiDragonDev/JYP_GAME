[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int stackSize;

    public InventoryItem(Item item, int stackSize)
    {
        this.item = item;
        this.stackSize = stackSize;
    }
}
