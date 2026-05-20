using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using Lean.Pool;
using UnityEngine;

public class Spawner : SingletonMonoBehaviour<Spawner>
{
    public Item CreateItem(Vector2 position, int itemID)
    {
        return CreateInstance(Items[itemID], position);
    }

    public LevelArea CreateLevelArea(Vector2 position)
    {
        return CreateInstance(LevelArea, position);
    }


    private T CreateInstance<T>(T prefab, Vector3 pos) where T : MonoBehaviour
    {
        T spawned;
#if UNITY_EDITOR
        if (Application.isPlaying)
            spawned = LeanPool.Spawn(prefab, pos, prefab.transform.rotation, transform);
        //spawned = Instantiate(prefab, pos, Quaternion.identity);
        else
            spawned = Instantiate(prefab, pos, prefab.transform.rotation, transform);
#else
        spawned = LeanPool.Spawn(prefab, pos, prefab.transform.rotation, transform);
#endif

        return spawned;
    }

    public Transform CreateInstance(Transform prefab, Vector3 pos)
    {
        Transform spawned;

#if UNITY_EDITOR
        if (Application.isPlaying)
            spawned = LeanPool.Spawn(prefab, pos, Quaternion.identity, transform);
        else
            spawned = Instantiate(prefab, pos, Quaternion.identity, transform);
#else
        spawned = LeanPool.Spawn(prefab, pos, Quaternion.identity, transform);
#endif

        return spawned;
    }

    #region Referances

    [Header("Fruits")] 
    [SerializeField] [Anywhere] public List<Item> Items = new();

    [Header("Level Elements")] [SerializeField] [Anywhere]
    public LevelArea LevelArea;


    #endregion
}