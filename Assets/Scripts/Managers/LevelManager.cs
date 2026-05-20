using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    [field: SerializeField] public int Level { get; private set; }
    [SerializeField] [Anywhere] public LevelData[] levels;
    public LevelData currentLevelData;


    private void Start()
    {
        //LoadLevel(Level);
    }

    private void LoadLevel(int levelIndex)
    {
        
        
        PlayerPrefs.SetInt("Level", levelIndex);
        currentLevelData = levels[levelIndex];
        
        EnvironmentManager.Instance.LoadData(currentLevelData);
        
        int levelDataName = 0;
        
        if (int.TryParse(currentLevelData.name, out int result))
        {
            levelDataName = result;
        }
        
        EventManager.Fire(new Events.OnLevelLoaded(currentLevelData, levelIndex, levelDataName));
    }

    public LevelData GetCurrentLevelData()
    {
        return currentLevelData;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        
        levels = EditorHelper.FindAssetsByType<LevelData>("Assets/Levels").ToArray();
    }
#endif

    private void Clear()
    {
      
    }
}
