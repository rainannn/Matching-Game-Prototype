using System;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour, IDespawnable, IClickable
{
    [SerializeField] [Self] private Despawner despawner;
    [SerializeField] [Self] public ItemController itemController;
    [SerializeField] [Self] public Rigidbody Rigidbody;
    [SerializeField] [Self] public ItemAnimator Animator;
    [SerializeField] [Child] public Mesh Mesh;


    public bool isJumping;
    public int itemID = -1;


    void Awake()
    {
        
    }

    private void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Despawn()
    {
        despawner.Despawn();
    }

}
