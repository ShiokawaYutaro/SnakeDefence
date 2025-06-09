using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    int maxSlots = 5;

    public void AddItem(Item newItem, int amount = 1)
    {
        Debug.Log($"Adding Item: {newItem.itemName}, Amount: {amount}");

        if (newItem.isStackable)
        {
            var slot = slots.Find(s => s.item != null && s.item.itemName == newItem.itemName);
            if (slot != null)
            {
                slot.quantity += amount;
                Debug.Log($"Item {newItem.itemName} updated, new quantity: {slot.quantity}");
                return;
            }
        }

        if (slots.Count >= maxSlots)
        {
            Debug.Log("ƒCƒ“ƒxƒ“ƒgƒŠ‚ª–ž”t‚Å‚·");
            return;
        }

        slots.Add(new InventorySlot { item = newItem, quantity = amount });
        Debug.Log($"Item {newItem.itemName} added to inventory");
    }



    public void RemoveItem(int index, int amount = 1)
    {
        if (index < 0 || index >= slots.Count) return;

        slots[index].quantity -= amount;
        if (slots[index].quantity <= 0)
        {
            slots.RemoveAt(index);
        }
    }
}
