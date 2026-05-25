using KBCore.Refs;
using Lean.Pool;
using UnityEngine;

public class PoolableParticle : MonoBehaviour
{
    [SerializeField] [Self] ParticleSystem particle;
    public void OnParticleSystemStopped()
    {
        LeanPool.Despawn(this);
    }
}
