using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KBCore.Refs;
using UnityEngine;

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
            //TODO
            item.Despawn();
        });
    }

    
    [Obsolete("Obsolete")]
    private void VFX()
    {
        var particle = VFXManager.Instance.ItemPop(transform.position);
        ParticleSystem.MinMaxGradient color = _particleData.appleMat.color;
        //particle.GetComponent<ParticleSystem>().customData.SetColor(particle, color);
    }
}
