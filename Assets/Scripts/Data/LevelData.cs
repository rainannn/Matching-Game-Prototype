using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
   public List<ItemSaveData> ItemSaveDatas = new();
   public SlotSaveData SlotSaveData;
}

[Serializable]
public class ItemSaveData
{
   [Header("Item ID")]
   public int ItemID;
   
   [Header("Position Elements")]
   public Vector3 position;
}

[Serializable]
public class SlotSaveData
{
   [Header("Slot Count")]
   public int slotCount;
}

public enum FruitType
{
   Apple,
   Banana,
   Coconut,
   None,
}