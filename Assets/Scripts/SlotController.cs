using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.UIElements;

public class SlotController : SingletonMonoBehaviour<SlotController>
{
    #region Properties

    [Header("Lists")] [SerializeField] [Child]
    public Slot[] slots;
    
    [SerializeField] private List<Item> activeItems = new();
    [SerializeField] private List<Item> poppingItems = new();

    
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        Clear();
    }

    #endregion

    #region Event Handlers

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

        StartCoroutine(PopItems(index));
    }

    #endregion
    
    #region Helper Methods

    //Ordered By method's return type.
    public Slot GetSlot(int index)
    {
        return slots[index];
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
    
    public bool HasItem(Item item)
    {
        return activeItems.Contains(item);
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
    
    private void Clear()
    {
        activeItems.Clear();
        poppingItems.Clear();
    }

    #endregion

    #region Game Logic

    public void ShiftRightItems(int index)
    {
        for (int i = index; i < activeItems.Count; i++)
        {
            Item activeItem = activeItems[i];

            if (activeItem == null) break;

            slots[i].SetOccupation(true);
            slots[i].SetItem(activeItem);

            activeItem.itemController.ShiftRight(slots[i]);
        }
    }
    
    private void ShiftLeftItems(int slotIndex)
    {
        
        for (int i = slotIndex; i < activeItems.Count; i++)
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
            
        }
        
        activeItems[slotIndex].itemController.ShiftLeft(slots[slotIndex], slots[slotIndex + 1], slots[slotIndex + 2], slotIndex);
        
    }
    
    private bool TryPop(int index)
    {
        Item landed = activeItems[index];
        
        if (landed == null) return false;
        
        int landedID = landed.itemID;

        int checkID1 = activeItems[index - 1].itemID;
        int checkID2 = activeItems[index - 2].itemID;

        if (landedID != checkID1 || landedID != checkID2) return false;

        Debug.Log("pop");
        return true;
    }
    
    private IEnumerator PopItems(int index)
    {
        Debug.Log(index);
        
        Item item0 = activeItems[index - 2];
        Item item1 = activeItems[index - 1];
        Item item2 = activeItems[index];
        
        yield return BetterWaitForSeconds.Wait(Slot.BumpDuration);

        
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
        
        if (activeItems.Count !> 0) yield break; 
        
        ShiftLeftItems(index - 2);
    }
    
    
    #endregion
}