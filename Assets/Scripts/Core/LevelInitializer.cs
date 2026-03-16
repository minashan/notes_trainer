using UnityEngine;
using NotesTrainer;

public class LevelInitializer : MonoBehaviour
{
    [Header("Level Lists")]
    [SerializeField] private LevelData[] trebleLevels;
    [SerializeField] private LevelData[] bassLevels;
    
    private void Awake()
    {
        // Загружаем имя уровня, которое сохранили при выборе
        string levelName = PlayerPrefs.GetString("SelectedLevelName", "");
        
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogError("No level name found in PlayerPrefs!");
            return;
        }
        
        // Ищем уровень по имени
        LevelData selectedLevel = FindLevelByName(levelName);
        
        if (selectedLevel == null)
        {
            Debug.LogError($"Level '{levelName}' not found!");
            return;
        }
        
        // Передаём уровень напрямую в SmartNoteGenerator
        SmartNoteGenerator generator = FindFirstObjectByType<SmartNoteGenerator>();
        if (generator != null)
        {
            generator.SetLevel(selectedLevel);
            Debug.Log($"LevelInitializer: set generator level to {selectedLevel.name}");
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