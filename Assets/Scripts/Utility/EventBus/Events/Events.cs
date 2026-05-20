using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{

    public struct OnLevelLoaded : IEvent
    {
        public readonly LevelData LevelData;
        public readonly int Level;
        public int LevelDataName;

        public OnLevelLoaded(LevelData levelData, int level, int levelDataName)
        {
            LevelData = levelData;
            Level = level;
            LevelDataName = levelDataName;
        }
    }
    public struct OnLevelFailed : IEvent{}

    public struct OnLevelCompleted : IEvent
    {
    }  
    public struct OnItemClicked : IEvent
    {
        public Item item;

        public OnItemClicked(Item item)
        {
            this.item = item;
        }
    }
    
    public struct OnItemsShift : IEvent
    {
        public int index;

        public OnItemsShift(int index)
        {
            this.index = index;
        }
    }
    
    public struct OnItemLanded : IEvent
    {
        public Slot Slot;

        public OnItemLanded(Slot slot)
        {
            Slot = slot;
        }
    } 
    public struct OnItemsPopped : IEvent
    {
        public List<Item> Items;

        public OnItemsPopped(List<Item> Items)
        {
            this.Items = Items;
        }
    }
}