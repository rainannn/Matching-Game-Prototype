using System;
using KBCore.Refs;
using UnityEngine;

public class LevelArea : MonoBehaviour
{
    [SerializeField] [Child] private LevelCollider[] colliders;
    
    public struct LevelBounds
    {
        public float Top;
        public float Bottom;
        public float Right;
        public float Left;
    }
    
    public LevelBounds Bounds => new LevelBounds()
    {
        Left = colliders[0].transform.position.x,
        Right = -colliders[0].transform.position.x,
        Top = colliders[2].gameObject.transform.position.z,
        Bottom = -colliders[2].transform.position.z
    };


}
