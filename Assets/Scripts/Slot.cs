using DG.Tweening;
using KBCore.Refs;
using UnityEngine;

public class Slot : MonoBehaviour, IDespawnable
{
    [SerializeField] [Self] private Despawner despawner;
    [SerializeField] [Anywhere] private ColorData colorData;
    [SerializeField] [Anywhere] private MeshRenderer[] colorable;
    
    private bool _isOccupied;
    public Item item;
    private static Vector3 initialPos;
    public const float BumpDuration = 0.1f;
    private Sequence _bumpSequence;


    private void Start()
    {
        Init();
    }

    public void Bump()
    {
        //transform.DOKill();

        _bumpSequence = DOTween.Sequence();

        _bumpSequence.Append(transform.DOMoveY(initialPos.y - 0.05f, BumpDuration)
            .SetEase(Ease.Linear));
        _bumpSequence.Append(transform.DOMoveY(initialPos.y, BumpDuration)
            .SetEase(Ease.Linear));

        _bumpSequence.InsertCallback(0, () => SetColor(colorData.transparentMat));
        _bumpSequence.InsertCallback(BumpDuration * 1.2f, () => SetColor(colorData.initialMat));
    }

    public Item GetItem()
    {
        return item;
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
        this.item = item;
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
        despawner.Despawn();
    }

    private void Clear()
    {
        _isOccupied = false;
    }
}