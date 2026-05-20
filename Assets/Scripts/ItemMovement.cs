using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KBCore.Refs;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] [Self] private Item _item;
    private Sequence jumpSequence;

    public readonly float jumpDurationMultiplier = 0.5f;
    private static Vector3 rotationValue = new Vector3(0, 0, 360);
    private static Vector3 scaleValue = Vector3.zero;

    private void Awake()
    {
        
    }

    void Start()
    {
        var slot = SlotController.Instance.GetSlot(0);
        
        if (scaleValue == Vector3.zero)
        { 
            scaleValue = new Vector3(slot.transform.localScale.x * 3 , slot.transform.localScale.y * 3 , slot.transform.localScale.z * 3 );
        }

        if (jumpSequence != null)
        {
            DOTween.Kill(jumpSequence);
        }
    }


    public void Jump(Slot slot, float duration, float jumpPower)
    {
        //var duration = Vector3.Distance(transform.position, slot.transform.position) * jumpDurationMultiplier;
        transform.DOJump(slot.transform.position, jumpPower, 1, duration)
            .SetEase(Ease.Linear)
            .OnComplete(slot.Bump);
             
    }


    public async UniTask JumpToSlot(Slot slot)
    {
        
        Vector3 startPos = transform.position;

        //_item.Rigidbody.useGravity = false;
        _item.Rigidbody.isKinematic = true;
        
        /*transform.DORotateQuaternion(Quaternion.identity, riseDuration)
            .SetEase(Ease.Linear);
        
        while (elapsed < riseDuration)
        {
            float time = elapsed / jumpDuration;
            time = Mathf.SmoothStep(0f, 1f, time);
            
            _item.Rigidbody.MovePosition(Vector3.Lerp(startPos, startPos + Vector3.up / 1.25f, time));

            elapsed += Time.deltaTime;
            await UniTask.NextFrame();
        }*/

        //transform.position = startPos + Vector3.up / 2;
        
        
        
        
        jumpSequence = DOTween.Sequence();



        jumpSequence.AppendCallback(() => Jump(slot, 0.5f, 1));
        jumpSequence.Join(transform.DORotate(rotationValue, 0.5f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.Linear));
        jumpSequence.Join(_item.Mesh.transform.DOScale(scaleValue, 0.5f))
            .SetEase(Ease.Linear)
            .OnComplete(() =>
                {
                    transform.SetParent(slot.transform);
                    _item.isJumping = false;
                }
        );
       

        await jumpSequence.AsyncWaitForCompletion();
        
        
        


    }
}