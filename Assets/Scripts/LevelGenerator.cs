using UnityEngine;
using KBCore.Refs;
using Random = UnityEngine.Random;


public class LevelGenerator : SingletonMonoBehaviour<LevelGenerator>
{
    [Header("Level Settings")] 
    [field: SerializeField] private int typeCount;
    [field: SerializeField] private int maxItemCount;
    
    [Header("Ref")] 
    [SerializeField] [Scene] LevelArea levelArea;


    private void GenerateLevel()
    {
        float horizontalMax = levelArea.Bounds.Right;
        float horizontalMin = levelArea.Bounds.Left;

        float verticalMax = levelArea.Bounds.Top;
        float verticalMin = levelArea.Bounds.Bottom;
        
        var horizontal = Random.Range(horizontalMin, horizontalMax);
        var vertical = Random.Range(verticalMin, verticalMax);
    }

    private int CalculateActualItemCount()
    {
        return maxItemCount - (maxItemCount % 3);
    }
}