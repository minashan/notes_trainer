using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace NotesTrainer
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Уровни")]
        [SerializeField] private LevelData[] levels;
        
        [Header("Текущее состояние")]
        [SerializeField] private int currentLevelIndex = 0;
        [SerializeField] private int currentLevelScore = 0;
        
        private GameManager gameManager;
        private UIManager uiManager;
        private SmartNoteGenerator _smartNoteGenerator;
        
        public LevelData CurrentLevel => (levels != null && currentLevelIndex < levels.Length) ? levels[currentLevelIndex] : null;
        public int CurrentLevelNumber => currentLevelIndex + 1;
        public int TotalLevels => levels.Length;

        private bool _isLevelCompleting = false;
        private bool _firstNote = true;
        
        private void Start()
{
    // Находим компоненты
    gameManager = FindAnyObjectByType<GameManager>();
    uiManager = FindAnyObjectByType<UIManager>();
    
    if (gameManager == null) Debug.LogError("GameManager not found!");
    if (uiManager == null) Debug.LogWarning("UIManager not found!");

    Debug.Log($"Levels: {levels?.Length ?? 0}");
    
    // Загружаем сохраненный прогресс
    int savedLevel = PlayerPrefs.GetInt("HighestLevel", 0);
    if (savedLevel > 0)
    {
        currentLevelIndex = Mathf.Clamp(savedLevel - 1, 0, levels.Length - 1);
        Debug.Log($"Loaded progress: Level {savedLevel}");
    }
    
    currentLevelIndex = Mathf.Clamp(savedLevel - 1, 0, levels.Length - 1);

    Debug.Log("LevelManager ready, waiting for GameManager...");
    Debug.Log($"Saved level: {PlayerPrefs.GetInt("HighestLevel", 0)}, CurrentIndex: {currentLevelIndex}");
}
        
public void StartCurrentLevel()
{
    ResetFirstNoteFlag();
    _isLevelCompleting = false;
    
    if (CurrentLevel == null) return;
    
    Debug.Log($"=== LEVEL {CurrentLevelNumber} ===");
    Debug.Log($"UI Manager: {uiManager != null}");
    Debug.Log($"SmartNoteGenerator = {_smartNoteGenerator != null}");
    
    // ПОКАЗЫВАЕМ инфо с анимацией
    if (uiManager != null)
    {
        string levelName = CurrentLevel.levelName;
        uiManager.UpdateLevelInfo(levelName, CurrentLevel.description);
        uiManager.ShowLevelInfo(true, 1f); // плавное появление
    }
    
    // НАСТРАИВАЕМ генератор
    if (_smartNoteGenerator != null)
    {
        _smartNoteGenerator.SetLevel(CurrentLevel);
    }
    
    // Генерируем первую ноту
    if (_smartNoteGenerator != null)
    {
        _smartNoteGenerator.GenerateNote();
    }
}

public void HideLevelInfo()
{
    if (uiManager != null)
    {
        uiManager.ShowLevelInfo(false, 0.5f); // плавное исчезание
    }
}


public bool IsFirstNoteInLevel()
{
    if (_firstNote)
    {
        _firstNote = false;
        return true;
    }
    return false;
}

public void ResetFirstNoteFlag()
{
    _firstNote = true;
}

        
        public void AddScore(int points)
{
    if (CurrentLevel == null) return;
    
    
    if (_isLevelCompleting) return;
    
    currentLevelScore += points;
    Debug.Log($"AddScore: +{points} = {currentLevelScore}/{CurrentLevel.requiredScore}");
    
    if (currentLevelScore >= CurrentLevel.requiredScore)
    {
        LevelComplete();
    }
}
        
        
        private void LevelComplete()
{
    if (_isLevelCompleting) return;
    _isLevelCompleting = true;
    
    Debug.Log($"LEVEL {CurrentLevelNumber} COMPLETE");

    // Проверяем какое значение было до сохранения
    int oldHighest = PlayerPrefs.GetInt("HighestLevel", 0);
    Debug.Log($"Old highest level: {oldHighest}");
    
    if (CurrentLevelNumber > oldHighest)
    {
        PlayerPrefs.SetInt("HighestLevel", CurrentLevelNumber);
        PlayerPrefs.Save();
        Debug.Log($"Progress saved: Level {CurrentLevelNumber}");
    }
    
    // Показываем UI завершения уровня
    //if (uiManager != null)
    //{
    //   uiManager.ShowLevelComplete(true);
    //}
    
    //_isLevelCompleting = true;

    if (CurrentLevelNumber > PlayerPrefs.GetInt("HighestLevel", 0))
    {
        PlayerPrefs.SetInt("HighestLevel", CurrentLevelNumber);
        PlayerPrefs.Save();
        Debug.Log($"Progress saved: Level {CurrentLevelNumber}");
    }

    int nextLevel = CurrentLevelNumber + 1;
if (nextLevel > PlayerPrefs.GetInt("HighestLevel", 0))
{
    PlayerPrefs.SetInt("HighestLevel", nextLevel);
}
  
    
    // Автопереход через 3 секунды
    StartCoroutine(AutoNextLevel(3f));
}
        
        private IEnumerator AutoNextLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            // Закрываем панель
            if (uiManager != null && uiManager.levelCompletePanel != null)
            {
                uiManager.levelCompletePanel.SetActive(false);
            }
            
            // Переходим на следующий уровень
            int nextLevelIndex = currentLevelIndex + 1;
            
            if (nextLevelIndex < levels.Length)
            {
                currentLevelIndex = nextLevelIndex;
                currentLevelScore = 0;
                StartCurrentLevel();
            }
            else
            {
                Debug.Log("=== ALL LEVELS COMPLETE ==="); 
            }
        }

        public void GoToNextLevel()
        {
            int nextLevelIndex = currentLevelIndex + 1;
            
            if (nextLevelIndex < levels.Length)
            {
                currentLevelIndex = nextLevelIndex;
                currentLevelScore = 0;
                StartCurrentLevel();
            }
            else
            {
                Debug.Log("=== ALL LEVELS COMPLETE ==="); 
                
              
            }
        }
        
        
        /// Перейти на конкретный уровень
        public void GoToLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= levels.Length)
            {
                Debug.LogError($"Invalid level index: {levelIndex}");
                return;
            }
            
            currentLevelIndex = levelIndex;
            currentLevelScore = 0;
            StartCurrentLevel();
        }
        
        
        /// Сбросить прогресс
        public void ResetProgress()
        {
            currentLevelIndex = 0;
            currentLevelScore = 0;
            PlayerPrefs.DeleteKey("CurrentLevel");
            PlayerPrefs.Save();
            StartCurrentLevel();
            
            Debug.Log("Progress reset"); 
        }
        
        
        /// Загрузить сохраненный прогресс
        private void LoadProgress()
        {
            int savedLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            currentLevelIndex = Mathf.Clamp(savedLevel, 0, levels.Length - 1);
        }
        
        
        /// Сохранить текущий прогресс
        private void SaveProgress()
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevelIndex);
            PlayerPrefs.Save();
        }
        
        
        /// Получить информацию о прогрессе
        public string GetProgressInfo()
        {
            return $"Уровень {CurrentLevelNumber}/{TotalLevels}: {currentLevelScore}/{CurrentLevel.requiredScore}";
        }

        public bool IsLevelCompleted()
        {
            if (CurrentLevel == null) return false;
            return currentLevelScore >= CurrentLevel.requiredScore;
        }

        void OnApplicationQuit()
{
    Debug.Log($"OnApplicationQuit: HighestLevel = {PlayerPrefs.GetInt("HighestLevel", 0)}");
}

         

public void SetSmartNoteGenerator(SmartNoteGenerator generator)
{
    _smartNoteGenerator = generator;
    Debug.Log("SmartNoteGenerator установлен в LevelManager");
}

    }

}