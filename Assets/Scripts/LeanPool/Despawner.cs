using Lean.Pool;
using UnityEngine;

public class Despawner : MonoBehaviour, IDespawnable
{
    public virtual void Despawn()
    {
        if (!gameObject || !gameObject.activeSelf) return;
#if UNITY_EDITOR
        if (Application.isPlaying)
            LeanPool.Despawn(this);
        else
            DestroyImmediate(gameObject);
#else
        LeanPool.Despawn(this);
#endif
    }
}