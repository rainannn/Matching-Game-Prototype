using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.UIElements;

public class SlotController : SingletonMonoBehaviour<SlotController>
{
    [SerializeField] [Child] Slot[] slots;
    [SerializeField] private List<Item> activeItems = new();
    [SerializeField] private List<Item> poppingItems = new();

    private void OnEnable()
    {
        EventManager.Subscribe<Events.OnItemLanded>(OnItemLanded);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<Events.OnItemLanded>(OnItemLanded);
    }

    private void OnItemLanded(Events.OnItemLanded obj)
    {
        Slot slot = obj.Slot;
        var index = GetIndex(slot);

        if (index < 2) return;

        if (!TryPop(index)) return;

        PopItems(index);
    }

    private void Start()
    {
        Clear();
    }

    public bool ContainsType(Item item)
    {
        foreach (Item activeItem in activeItems)
        {
            if (activeItem.itemID != item.itemID) continue;
            return true;
        }

        return false;
    }

    public Slot GetSlot(bool contains, Item item)
    {
        if (!contains)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                Slot slot = slots[i];
                if (slot.IsOccupied())
                {
                    continue;
                }

                return slot;
            }
        }

        for (int i = activeItems.Count - 1; i >= 0; i--)
        {
            Item activeItem = activeItems[i];
            if (item.itemID != activeItem.itemID) continue;

            return slots[++i];
        }

        return null;
    }


    public void ShiftRightItems(int index)
    {
        for (int i = index; i < activeItems.Count; i++)
        {
            Item activeItem = activeItems[i];

            if (activeItem == null) break;


            activeItem.itemController.ShiftRight(slots[i]);
        }
    }

    private IEnumerator ShiftLeftItems(int index)
    {
        int waitTimeMultiplier = 0;
        for (int i = index; i < activeItems.Count; i++)
        {
            if (activeItems[i] == null) break;


            int oldSlotIndex = i + 3;

            if (oldSlotIndex < slots.Length)
            {
                slots[oldSlotIndex].SetOccupation(false);
                slots[oldSlotIndex].SetItem(null);
            }


            slots[i].SetItem(activeItems[i]);
            slots[i].SetOccupation(true);


            activeItems[i].itemController.ShiftLeft(slots[i], slots[i + 1], slots[i + 2]);
            yield return new WaitForSeconds(0.1f * waitTimeMultiplier);
            waitTimeMultiplier++;
        }
    }

    private bool TryPop(int index)
    {
        Item landed = activeItems[index];
        int landedID = landed.itemID;

        int checkID1 = activeItems[index - 1].itemID;
        int checkID2 = activeItems[index - 2].itemID;

        if (landedID != checkID1 || landedID != checkID2) return false;

        Debug.Log("pop");
        return true;
    }

    public int GetIndex(Slot slot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == slot)
            {
                return i;
            }
        }

        return -1;
    }

    public void AddToList(bool shifting, Item item, int index)
    {
        if (!shifting)
        {
            activeItems.Add(item);
            Debug.Log("Adding item to list");
            return;
        }

        activeItems.Insert(index, item);
    }

    private int FindPopIndex()
    {
        for (int i = 2; i < activeItems.Count; i++)
        {
            // Skip nulls
            if (activeItems[i] == null) continue;
            if (activeItems[i - 1] == null) continue;
            if (activeItems[i - 2] == null) continue;

            int id = activeItems[i].itemID;

            if (activeItems[i - 1].itemID == id &&
                activeItems[i - 2].itemID == id)
            {
                return i; // Return the index of the 3rd matching item
            }
        }

        return -1; // No 3-in-a-row found
    }


    private void PopItems(int index)
    {
        Item item0 = activeItems[index - 2];
        Item item1 = activeItems[index - 1];
        Item item2 = activeItems[index];

        // Clear slots BEFORE removing from list
        slots[index].SetOccupation(false);
        slots[index].SetItem(null);

        slots[index - 1].SetOccupation(false);
        slots[index - 1].SetItem(null);

        slots[index - 2].SetOccupation(false);
        slots[index - 2].SetItem(null);


        activeItems.RemoveRange(index - 2, 3);


        item0.Animator.Pop();
        item1.Animator.Pop();
        item2.Animator.Pop();


        StartCoroutine(ShiftLeftItems(index - 2));
    }

    public Slot GetSlot(int index)
    {
        return slots[index];
    }

    public bool HasItem(Item item)
    {
        if (activeItems.Contains(item)) return true;
        return false;
    }

    public int GetSameItemCount(int itemID)
    {
        int count = 0;
        for (int i = 0; i < activeItems.Count; i++)
        {
            if (activeItems[i].itemID != itemID) continue;
            count++;
        }

        return count;
    }

    private void Clear()
    {
        activeItems.Clear();
    }
}