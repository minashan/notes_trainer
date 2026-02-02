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
        
        private void Start()
        {
            // Находим компоненты
            gameManager = FindAnyObjectByType<GameManager>();
            uiManager = FindAnyObjectByType<UIManager>();
            
            if (gameManager == null) Debug.LogError("GameManager not found!");
            if (uiManager == null) Debug.LogWarning("UIManager not found!");

            Debug.Log($"Levels: {levels?.Length ?? 0}"); 
            
            // Загружаем сохраненный прогресс
            int savedLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            currentLevelIndex = Mathf.Clamp(savedLevel, 0, levels.Length - 1);
            
            
            Debug.Log("LevelManager ready, waiting for GameManager...");
        }
        
        public void StartCurrentLevel()
        {

        Debug.Log($"StartCurrentLevel: CurrentLevel = {CurrentLevel != null}");
        Debug.Log($"SmartNoteGenerator = {_smartNoteGenerator != null}");

            _isLevelCompleting = false;
            if (CurrentLevel == null) return;
            
            Debug.Log($"=== LEVEL {CurrentLevelNumber} ==="); 
           

            if (_smartNoteGenerator != null)
            {
                _smartNoteGenerator.SetLevel(CurrentLevel);
            }
            
            // Обновляем UI
            if (uiManager != null)
            {
                uiManager.UpdateLevelDisplay(CurrentLevelNumber, CurrentLevel.levelName, CurrentLevel.description);
                uiManager.UpdateProgress(0);
            }

            // Генерируем первую ноту из SmartGenerator
            if (_smartNoteGenerator != null)
            {
                _smartNoteGenerator.GenerateNote();
            }
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

         

public void SetSmartNoteGenerator(SmartNoteGenerator generator)
{
    _smartNoteGenerator = generator;
    Debug.Log("SmartNoteGenerator установлен в LevelManager");
}

    }

}