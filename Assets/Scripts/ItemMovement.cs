using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] [Self] private Item item;
    private Sequence _jumpSequence;

    public const float JumpDurationMultiplier = 0.5f;
    private readonly Vector3 _rotationValue = new Vector3(0, 0, 360);
    private Vector3 _scaleValue;


    void Start()
    {
        var slot = SlotController.Instance.GetSlot(0);

        if (_scaleValue == Vector3.zero)
        {
            _scaleValue = new Vector3(slot.transform.localScale.x * 3, slot.transform.localScale.y * 3,
                slot.transform.localScale.z * 3);
        }

        if (_jumpSequence != null)
        {
            DOTween.Kill(_jumpSequence);
        }
    }


    public void Jump(Slot slot, float duration, float jumpPower)
    {
        transform.DOJump(slot.transform.position, jumpPower, 1, duration)
            .SetEase(Ease.Linear)
            .InsertCallback(duration,slot.Bump);
        
    }


    public async UniTask JumpToSlot(Slot slot)
    {
        item.Rigidbody.isKinematic = true;


        _jumpSequence = DOTween.Sequence();

        
        _jumpSequence.AppendCallback(() => Jump(slot, 0.5f, 1));
        _jumpSequence.Join(transform.DORotate(_rotationValue, 0.5f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.Linear));
        _jumpSequence.Join(item.Mesh.transform.DOScale(_scaleValue, 0.5f))
            .SetEase(Ease.Linear)
            .OnComplete(() =>
                {
                    transform.SetParent(slot.transform);
                    item.isJumping = false;
                }
            );


        await _jumpSequence.AsyncWaitForCompletion();
    }
}