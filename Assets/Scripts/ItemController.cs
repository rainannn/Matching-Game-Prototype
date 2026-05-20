using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KBCore.Refs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

public class ItemController : MonoBehaviour
{
    [SerializeField] [Self] private ItemMovement itemMovement;
    [SerializeField] [Self] private ItemAnimator itemAnimator;
    [SerializeField] [Self] private Item item;
    public List<Slot> slotsQueue = new(3);
   
    

    private void OnEnable()
    {
        EventManager.Subscribe<Events.OnItemClicked>(OnItemClicked);
        EventManager.Subscribe<Events.OnItemsPopped>(OnItemsPop);
    }


    private void OnDisable()
    {
        EventManager.Unsubscribe<Events.OnItemClicked>(OnItemClicked);
        EventManager.Unsubscribe<Events.OnItemsPopped>(OnItemsPop);
    }

    private void OnItemsPop(Events.OnItemsPopped obj)
    {
        /*
        List<Item> poppingList = obj.Items;

        foreach (var item in poppingList)
        {
            if (item != this.item) return;
            itemAnimator.Pop();
        }*/
        
       
    }

    public  void ShiftRight(Slot slot)
    {
        transform.SetParent(slot.transform);
        itemMovement.Jump(slot,0.3f,0.5f);
    }
    public void ShiftLeft(Slot slot1, Slot slot2,Slot slot3)
    {
        transform.SetParent(slot1.transform);

        slotsQueue.Add(slot3);
        slotsQueue.Add(slot2);
        slotsQueue.Add(slot1);

        Sequence shift= DOTween.Sequence();
        
        shift.AppendCallback(()=>itemMovement.Jump(slotsQueue[0],0.3f, 0.5f));
        shift.InsertCallback(itemMovement.jumpDurationMultiplier,()=>itemMovement.Jump(slotsQueue[1],0.3f,0.5f));
        shift.InsertCallback(itemMovement.jumpDurationMultiplier * 2,()=>itemMovement.Jump(slotsQueue[2],0.3f,0.5f));
        
        shift.OnComplete(slotsQueue.Clear);

       
    }

    private void OnItemClicked(Events.OnItemClicked obj)
    {
        var clicked = obj.item;
        
        if (clicked != item) return;
        if (SlotController.Instance.HasItem(clicked)) return;
        
        clicked.isJumping = true;
        HandleItemClickedAsync(clicked).Forget();
        
    }
    
    
    
    private async UniTaskVoid HandleItemClickedAsync(Item clicked)
// UniTaskVoid = fire-and-forget async, perfect for event-driven entry points
    {
        if (SlotController.Instance.ContainsType(item) && 
            SlotController.Instance.GetSameItemCount(clicked.itemID) >= 1)
        {
            
            Slot suitableSlot = SlotController.Instance.GetSlot(true, item);
            suitableSlot.SetItem(null);
            
            int insertIndex = SlotController.Instance.GetIndex(suitableSlot);
            
            SlotController.Instance.AddToList(true, item, insertIndex);
            
            SlotController.Instance.ShiftRightItems(insertIndex + 1);
            
            suitableSlot.SetItem(item);
            suitableSlot.SetOccupation(true);
            
            await itemMovement.JumpToSlot(suitableSlot); 
            // ✅ Now actually waits for the full jump + landing
            // OnItemLanded fires INSIDE JumpToSlot after this completes

            EventManager.Fire(new Events.OnItemLanded(suitableSlot));
            return;
        }

        Debug.Log("yok");

        Slot slot = SlotController.Instance.GetSlot(false, item);

        SlotController.Instance.AddToList(false, item, -1);

        slot.SetItem(item);
        slot.SetOccupation(true);

        await itemMovement.JumpToSlot(slot); 
        EventManager.Fire(new Events.OnItemLanded(slot));
    }

  
}