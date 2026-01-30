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
        
        private NoteGenerator noteGenerator;
        private GameManager gameManager;
        private UIManager uiManager;
        
        public LevelData CurrentLevel => (levels != null && currentLevelIndex < levels.Length) ? levels[currentLevelIndex] : null;
        public int CurrentLevelNumber => currentLevelIndex + 1;
        public int TotalLevels => levels.Length;

        private bool _isLevelCompleting = false;
        
        private void Start()
        {
            // Находим компоненты
            noteGenerator = FindAnyObjectByType<NoteGenerator>();
            gameManager = FindAnyObjectByType<GameManager>();
            uiManager = FindAnyObjectByType<UIManager>();
            
            if (noteGenerator == null) Debug.LogError("NoteGenerator not found!");
            if (gameManager == null) Debug.LogError("GameManager not found!");
            if (uiManager == null) Debug.LogWarning("UIManager not found!");

            Debug.Log($"Levels: {levels?.Length ?? 0}"); // ← УПРОЩЕНО
            
            // Загружаем сохраненный прогресс
            int savedLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            currentLevelIndex = Mathf.Clamp(savedLevel, 0, levels.Length - 1);
            
            // Начинаем уровень
            StartCurrentLevel();
        }
        
        private void StartCurrentLevel()
        {
            _isLevelCompleting = false;
            if (CurrentLevel == null) return;
            
            Debug.Log($"=== LEVEL {CurrentLevelNumber} ==="); // ← УПРОЩЕНО
            
            // Устанавливаем ноты для генератора
            if (noteGenerator != null)
            {
                noteGenerator.SetLevelNotes(new List<string>(CurrentLevel.includedNotes), CurrentLevel.allowEnharmonic);
            }
            
            // Обновляем UI
            if (uiManager != null)
            {
                uiManager.UpdateLevelDisplay(CurrentLevelNumber, CurrentLevel.levelName, CurrentLevel.description);
                uiManager.UpdateProgress(0);
            }
            
            // Генерируем первую ноту
            if (noteGenerator != null)
            {
                noteGenerator.GenerateRandomNote();
            }
        }
        
        public void AddScore(int points)
{
    if (CurrentLevel == null) return;
    
    // ОСТАВЬ ЭТУ ПРОВЕРКУ - ОНА ВАЖНА!
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
    
    // ⭐⭐⭐ ВРЕМЕННО УБЕРИ UI КОД ⭐⭐⭐
    /*
    // Сохраняем прогресс
    PlayerPrefs.SetInt("LastUnlockedLevel", Mathf.Max(currentLevelIndex + 1, 
        PlayerPrefs.GetInt("LastUnlockedLevel", 0)));
    PlayerPrefs.Save();
    
    // Показываем окно завершения
    if (uiManager != null)
    {
        uiManager.ShowLevelComplete(CurrentLevelNumber, CurrentLevel.levelName);
    }
    */
    
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
                Debug.Log("=== ALL LEVELS COMPLETE ==="); // ← УПРОЩЕНО
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
                Debug.Log("=== ALL LEVELS COMPLETE ==="); // ← УПРОЩЕНО
                
                if (uiManager != null)
                {
                    // Или использовать ваш метод ShowLevelComplete с другим текстом
                    //uiManager.ShowLevelComplete(CurrentLevelNumber, "🎉 Все уровни пройдены!");
                }
            }
        }
        
        /// <summary>
        /// Перейти на конкретный уровень
        /// </summary>
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
        
        /// <summary>
        /// Сбросить прогресс
        /// </summary>
        public void ResetProgress()
        {
            currentLevelIndex = 0;
            currentLevelScore = 0;
            PlayerPrefs.DeleteKey("CurrentLevel");
            PlayerPrefs.Save();
            StartCurrentLevel();
            
            Debug.Log("Progress reset"); // ← УПРОЩЕНО
        }
        
        /// <summary>
        /// Загрузить сохраненный прогресс
        /// </summary>
        private void LoadProgress()
        {
            int savedLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            currentLevelIndex = Mathf.Clamp(savedLevel, 0, levels.Length - 1);
        }
        
        /// <summary>
        /// Сохранить текущий прогресс
        /// </summary>
        private void SaveProgress()
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevelIndex);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Получить информацию о прогрессе
        /// </summary>
        public string GetProgressInfo()
        {
            return $"Уровень {CurrentLevelNumber}/{TotalLevels}: {currentLevelScore}/{CurrentLevel.requiredScore}";
        }

        public bool IsLevelCompleted()
        {
            if (CurrentLevel == null) return false;
            return currentLevelScore >= CurrentLevel.requiredScore;
        }

    }

}