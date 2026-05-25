using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using Lean.Pool;
using UnityEngine;

public class VFXManager : SingletonMonoBehaviour<VFXManager>
{
   [SerializeField] [Anywhere] private PoolableParticle particle;
   //[SerializeField] [Anywhere] private ParticleData particleData;

   public PoolableParticle ItemPop(Vector3 pos)
   {
      var spawned = LeanPool.Spawn(particle, pos, Quaternion.identity);
      return spawned;
   }
}
