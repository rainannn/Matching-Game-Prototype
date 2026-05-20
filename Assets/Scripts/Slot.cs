using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KBCore.Refs;
using UnityEngine;

public class Slot : MonoBehaviour, IDespawnable
{
    
    [SerializeField] [Self] private Despawner _despawner;
    [SerializeField] [Anywhere] private ColorData colorData;
    [SerializeField] [Anywhere] private MeshRenderer[] colorable;
    private bool _isOccupied;
    private Item _item;
    private static Vector3 initialPos;

  

    void Start()
    {
        Init();
    }

    public void  Bump()
    {
        transform.DOKill();
        
        Sequence bumpSequence = DOTween.Sequence();
        bumpSequence.Append(transform.DOMoveY(initialPos.y - 0.05f, .2f).SetEase(Ease.Linear));
        bumpSequence.Append(transform.DOMoveY(initialPos.y, .2f).SetEase(Ease.InBack));
        
        bumpSequence.InsertCallback(0, ()=> SetColor(colorData.transparentMat));
        bumpSequence.InsertCallback(.4f, ()=> SetColor(colorData.initialMat));
        
        
    }

    public Item GetItem()
    {
        return _item;
    }

    private void Init()
    {
        SetOccupation(false);
        SetLookAngle();
        initialPos = transform.position;
    }

    private void SetLookAngle()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetItem(Item item)
    {
        _item = item;
    }

    private void SetPosition(int x)
    {
        var pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }

    public void SetOccupation(bool isOccupied)
    {
        _isOccupied = isOccupied;
    }

    public void SetColor(Material colorDataMat)
    {
        colorable[0].material = colorDataMat;
        colorable[1].material = colorDataMat;
    }


    public bool IsOccupied()
    {
        if (_isOccupied) return true;
        return false;
    }

    public void Despawn()
    {
        _despawner.Despawn();
    }

    private void Clear()
    {
        _isOccupied = false;
    }
}


