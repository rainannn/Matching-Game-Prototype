using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KBCore.Refs;
using UnityEngine;

public class ItemAnimator : MonoBehaviour
{
    [SerializeField] [Self] private Item item;
    
    
    public void Pop()
    {
        transform.DOMove(transform.position + Vector3.up / 2, .5f)
            .OnComplete(()=>
        {
            transform.gameObject.SetActive(false);
            //TODO
            //CONFETTI
            item.Despawn();
        });
    }
}
