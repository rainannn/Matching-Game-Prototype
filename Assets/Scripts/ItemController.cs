using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KBCore.Refs;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class ItemController : MonoBehaviour
{
    
    [SerializeField] [Self] private ItemMovement itemMovement;
    [SerializeField] [Self] private ItemAnimator itemAnimator;
    [SerializeField] [Self] private Item item;
    
    public List<Slot> slotsQueue = new(3);

    private const float ShiftingJumpDuration = 0.2f;
    private const float ShiftingJumpPower = 0.5f;
    private const float TotalTweenDurationPerJump = ShiftingJumpDuration + 0.4f;
    private const int DefaultIndex = -1;


    private void OnEnable()
    {
        EventManager.Subscribe<Events.OnItemClicked>(OnItemClicked);
    }


    private void OnDisable()
    {
        EventManager.Unsubscribe<Events.OnItemClicked>(OnItemClicked);
    }


    public void ShiftRight(Slot slot)
    {
        transform.SetParent(slot.transform);
        itemMovement.Jump(slot, ShiftingJumpDuration, ShiftingJumpPower);
    }

    public void ShiftLeft(Slot slot1, Slot slot2, Slot slot3, int itemIndex)
    {
        transform.SetParent(slot1.transform);

        slotsQueue.Add(slot3);
        slotsQueue.Add(slot2);
        slotsQueue.Add(slot1);
        
        var nextItemSlot1 = SlotController.Instance.GetSlot(SlotController.Instance.GetIndex(slot1) + 1);
        var nextItemSlot2 = SlotController.Instance.GetSlot(SlotController.Instance.GetIndex(slot1) + 2);
        var nextItemSlot3 = SlotController.Instance.GetSlot(SlotController.Instance.GetIndex(slot1) + 3);

        Sequence shift = DOTween.Sequence();

        shift.AppendCallback(() => itemMovement.Jump(slotsQueue[0], ShiftingJumpDuration, ShiftingJumpPower));
        
        shift.InsertCallback(TotalTweenDurationPerJump * 0.2f, () =>
        {
            itemIndex++;
            CallNext(nextItemSlot1, nextItemSlot2,nextItemSlot3, itemIndex);
        });
        
        shift.InsertCallback(ShiftingJumpDuration, () =>
        {
            itemMovement.Jump(slotsQueue[1], ShiftingJumpDuration, ShiftingJumpPower);
        });
        
        shift.InsertCallback(ShiftingJumpDuration * 2,
            () => itemMovement.Jump(slotsQueue[2], ShiftingJumpDuration, ShiftingJumpPower));

        shift.OnComplete(()=>
        {
            slotsQueue.Clear();
        });
    }



    private void CallNext(Slot nextItemSlot1, Slot nextItemSlot2, Slot nextItemSlot3, int itemIndex)
    {
        if (itemIndex > SlotController.Instance.slots.Length) return;
            
        var item = SlotController.Instance.slots[itemIndex].item;
            
        if (item != null)
        {
            item.itemController.ShiftLeft(nextItemSlot1, nextItemSlot2, nextItemSlot3, itemIndex);
        }
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


            EventManager.Fire(new Events.OnItemLanded(suitableSlot));
            return;
        }
        
        Slot slot = SlotController.Instance.GetSlot(false, item);

        SlotController.Instance.AddToList(false, item, DefaultIndex);

        slot.SetItem(item);
        slot.SetOccupation(true);

        await itemMovement.JumpToSlot(slot);
        
        EventManager.Fire(new Events.OnItemLanded(slot));
    }
}