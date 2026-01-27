using UnityEngine;
using System.Collections.Generic;

namespace NotesTrainer
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Уровни")]
        [SerializeField] private LevelData[] levels;
        
        [Header("Текущее состояние")]
        [SerializeField] private int currentLevelIndex = 0;
        [SerializeField] private int currentLevelScore = 0;
        [SerializeField] private bool isLevelCompleted = false;
        
        // Компоненты
        private NoteGenerator noteGenerator;
        private GameManager gameManager;
        private UIManager uiManager;
        
        // Свойства
        public LevelData CurrentLevel => levels[currentLevelIndex];
        public int CurrentLevelNumber => currentLevelIndex + 1;
        public int TotalLevels => levels.Length;
        public (int current, int required) ScoreInfo => (currentLevelScore, CurrentLevel.requiredScore);
        public float Progress => Mathf.Clamp01((float)currentLevelScore / CurrentLevel.requiredScore);
        
        /// <summary>
        /// Инициализация менеджера уровней
        /// </summary>
        public void Initialize(NoteGenerator generator, GameManager gameMgr, UIManager uiMgr)
        {
            noteGenerator = generator;
            gameManager = gameMgr;
            uiManager = uiMgr;
            
            // Загружаем сохраненный прогресс
            LoadProgress();
            
            // Начинаем с текущего уровня
            StartCurrentLevel();
            
            Debug.Log($"[LevelManager] Initialized. Current level: {CurrentLevel.levelName}");
        }
        
        /// <summary>
        /// Начать текущий уровень
        /// </summary>
        private void StartCurrentLevel()
        {
            isLevelCompleted = false;
            
            // Устанавливаем ноты для генератора
            noteGenerator.SetAvailableNotes(CurrentLevel.includedNotes);
            
            // Генерируем первую ноту
            noteGenerator.GenerateRandomNote();
            
            // Обновляем UI
            uiManager?.UpdateLevelDisplay(CurrentLevelNumber, CurrentLevel.levelName, CurrentLevel.description);
            uiManager?.UpdateProgress(Progress);
            
            Debug.Log($"[LevelManager] Started level {CurrentLevelNumber}: {CurrentLevel.levelName}");
            Debug.Log($"[LevelManager] Notes in level: {string.Join(", ", CurrentLevel.includedNotes)}");
        }
        
        /// <summary>
        /// Добавить очки за правильный ответ
        /// </summary>
        public void AddScore(int points)
        {
            if (isLevelCompleted) return;
            
            currentLevelScore += points;
            SaveProgress();
            
            // Обновляем прогресс в UI
            uiManager?.UpdateProgress(Progress);
            
            // Проверяем завершение уровня
            if (currentLevelScore >= CurrentLevel.requiredScore)
            {
                CompleteLevel();
            }
            
            Debug.Log($"[LevelManager] +{points} points. Total: {currentLevelScore}/{CurrentLevel.requiredScore}");
        }
        
        /// <summary>
        /// Завершить текущий уровень
        /// </summary>
        private void CompleteLevel()
        {
            isLevelCompleted = true;
            
            Debug.Log($"[LevelManager] Level {CurrentLevelNumber} completed!");
            
            // Сохраняем прогресс
            PlayerPrefs.SetInt("CurrentLevel", currentLevelIndex);
            PlayerPrefs.Save();
            
            // Уведомляем UI
            uiManager?.ShowLevelComplete(CurrentLevelNumber, CurrentLevel.levelName);
            
            // Если есть следующий уровень - разблокируем
            if (currentLevelIndex < levels.Length - 1)
            {
                // Автоматически переходим или ждем нажатия кнопки
                // Пока просто разблокируем следующий
            }
        }
        
        /// <summary>
        /// Перейти на следующий уровень
        /// </summary>
        public void GoToNextLevel()
        {
            if (currentLevelIndex >= levels.Length - 1)
            {
                Debug.Log("[LevelManager] Это последний уровень!");
                return;
            }
            
            currentLevelIndex++;
            currentLevelScore = 0;
            StartCurrentLevel();
            
            Debug.Log($"[LevelManager] Moved to level {CurrentLevelNumber}");
        }
        
        /// <summary>
        /// Перейти на конкретный уровень
        /// </summary>
        public void GoToLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= levels.Length)
            {
                Debug.LogError($"[LevelManager] Invalid level index: {levelIndex}");
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
            
            Debug.Log("[LevelManager] Progress reset");
        }
        
        /// <summary>
        /// Загрузить сохраненный прогресс
        /// </summary>
        private void LoadProgress()
        {
            int savedLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            currentLevelIndex = Mathf.Clamp(savedLevel, 0, levels.Length - 1);
            
            Debug.Log($"[LevelManager] Loaded progress: level {currentLevelIndex + 1}");
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

        void Start()
{
    Debug.Log($"[LevelManager] === INITIALIZATION ===");
    Debug.Log($"[LevelManager] Total levels: {levels?.Length ?? 0}");
    
    for (int i = 0; i < levels.Length; i++)
    {
        if (levels[i] != null)
        {
            Debug.Log($"[LevelManager] Level {i+1}: '{levels[i].levelName}' - {levels[i].includedNotes?.Length ?? 0} notes");
        }
        else
        {
            Debug.LogError($"[LevelManager] Level {i+1} is NULL!");
        }
    }
}
    }
}
