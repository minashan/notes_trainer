using UnityEngine;
using NotesTrainer;

public class LevelInitializer : MonoBehaviour
{
    [Header("Level Lists")]
    [SerializeField] private LevelData[] trebleLevels;
    [SerializeField] private LevelData[] bassLevels;
    
    private void Awake()
{
    string levelName = PlayerPrefs.GetString("SelectedLevelName", "");
    if (string.IsNullOrEmpty(levelName)) return;
    
    LevelData selectedLevel = FindLevelByName(levelName);
    if (selectedLevel == null) return;
    
    // Передаём уровень в SmartNoteGenerator
    SmartNoteGenerator generator = FindFirstObjectByType<SmartNoteGenerator>();
    if (generator != null)
    {
        generator.SetLevel(selectedLevel);
        Debug.Log($"LevelInitializer: set generator level to {selectedLevel.name}");
    }
    
    // 🔥 НОВОЕ: передаём уровень в LevelManager для отображения названия
    LevelManager levelManager = FindFirstObjectByType<LevelManager>();
    if (levelManager != null)
    {
        levelManager.SetDisplayLevel(selectedLevel);
        Debug.Log($"LevelInitializer: set display level to {selectedLevel.name}");
    }
}
    
    private LevelData FindLevelByName(string name)
    {
        // Ищем в массиве скрипичных уровней
        foreach (var level in trebleLevels)
        {
            if (level != null && level.name == name)
                return level;
        }
        
        // Ищем в массиве басовых уровней
        foreach (var level in bassLevels)
        {
            if (level != null && level.name == name)
                return level;
        }
        
        return null;
    }
}