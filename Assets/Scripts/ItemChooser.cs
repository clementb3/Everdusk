using UnityEngine;

public class ItemChooser : MonoBehaviour
{
    [HideInInspector]
    public ItemData item;      // Reference to the item chosen.
    public void ChooseItem()
    {
        if (item != ItemActionSystem.instance.GetSelectedItem())
            ItemActionSystem.instance.OpenItemAction(item);
        else
            ItemActionSystem.instance.CloseItemAction();
    }
}
