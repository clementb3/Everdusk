using UnityEngine;

public class ItemChooser : MonoBehaviour
{
    public ItemData item;

    public void ChooseItem()
    {
        Debug.Log(item);
        if (item != Inventory.instance.GetSelecteditem())
            Inventory.instance.OpenItemAction(item);
        else
            Inventory.instance.CloseItemAction();
    }
}
