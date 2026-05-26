using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KBCore.Refs;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class ItemAnimator : MonoBehaviour
{
    [SerializeField] [Anywhere] private ParticleData _particleData;
    [SerializeField] [Self] private Item item;
    
    
    public void Pop()
    {
        transform.DOMove(transform.position + Vector3.up / 2, .5f)
            .OnComplete(()=>
        {
            transform.gameObject.SetActive(false);
            VFX();
            item.Despawn();
        });
    }

    
    
    private void VFX()
    {
        var particle = VFXManager.Instance.ItemPop(transform.position);
        particle.SetColor(_particleData.apple);
        particle.GetComponent<ParticleSystem>().Play();
    }
}
