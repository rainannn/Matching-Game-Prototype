using KBCore.Refs;
using Lean.Pool;
using UnityEngine;

public class PoolableParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem glow;
    [SerializeField] private ParticleSystem star;
    public void OnParticleSystemStopped()
    {
        LeanPool.Despawn(this);
    }

    public void SetColor(Color color)
    {
        ParticleSystem.MainModule glowMain = glow.main;
        ParticleSystem.MainModule starMain = star.main;
        
        glowMain.startColor = color;
        starMain.startColor = color;
    }
}
