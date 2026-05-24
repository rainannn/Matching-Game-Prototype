using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : SingletonMonoBehaviour<EnvironmentManager>
{
    public readonly List<Item> Items = new();
    public readonly List<Slot> Slots = new();

    
    public void LoadData(LevelData levelData)
    {
        Clear();
        SpawnItems(levelData);
    }


    private void SpawnItems(LevelData levelData)
    {
        var itemSaveDatas = levelData.ItemSaveDatas;
        foreach (var itemSaveData in itemSaveDatas)
        {
            var spawned = Spawner.Instance.CreateItem(itemSaveData.position, itemSaveData.ItemID);
            Items.Add(spawned);
            spawned.transform.parent = transform;
        }
    }


    private void Clear()
    {
        Clear(Items);
        Clear(Slots);
    }


    private static void Clear<T>(List<T> list) where T : IDespawnable
    {
        var childCount = list.Count;

        for (var i = childCount - 1; i >= 0; i--)
            try
            {
                list[i].Despawn();
            }
            catch (MissingReferenceException e)
            {
                Debug.LogWarning("MissingReferenceException:" + e);
            }

        list.Clear();
    }
}