using System;
using UnityEngine;
using System.Collections;

namespace NotesTrainer
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Levels")]
        [SerializeField] private LevelData[] levels;
        
        [Header("Current State")]
        [SerializeField] private int currentLevelIndex;
        [SerializeField] private int currentLevelScore;
        
        private GameManager _gameManager;
        private UIManager _uiManager;
        private SmartNoteGenerator _smartNoteGenerator;
        
        private bool _isLevelCompleting;
        private bool _firstNote = true;
        
        private const string HIGHEST_LEVEL_KEY = "HighestLevel";
        private const string CURRENT_LEVEL_KEY = "CurrentLevel";
        
        public LevelData CurrentLevel => 
            (levels != null && currentLevelIndex < levels.Length) ? levels[currentLevelIndex] : null;
        
        public int CurrentLevelNumber => currentLevelIndex + 1;
        public int TotalLevels => levels.Length;
        
        private void Start()
        {
            _gameManager = FindFirstObjectByType<GameManager>();
            _uiManager = FindFirstObjectByType<UIManager>();
            
            int savedHighest = PlayerPrefs.GetInt(HIGHEST_LEVEL_KEY, 1);
            int selectedLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 1);
            
            if (selectedLevel > savedHighest)
            {
                selectedLevel = savedHighest;
            }
            
            currentLevelIndex = Mathf.Clamp(selectedLevel - 1, 0, levels.Length - 1);
        }
        
        public void StartCurrentLevel()
        {
            ResetFirstNoteFlag();
            _isLevelCompleting = false;
            
            if (CurrentLevel == null) return;
            
            if (_uiManager != null)
            {
                _uiManager.UpdateLevelInfo(CurrentLevel.levelName, CurrentLevel.description);
                _uiManager.ShowLevelInfo(true, 1f);
            }
            
            if (_smartNoteGenerator != null)
            {
                //_smartNoteGenerator.SetLevel(CurrentLevel);
            }
            
            StartCoroutine(StartLevelAfterDelay(3f));
        }
        
        private IEnumerator StartLevelAfterDelay(float delay)
        {
            if (_uiManager != null)
            {
                _uiManager.ShowNoteSprite(false);
            }
            
            yield return new WaitForSeconds(delay);
            
            if (_uiManager != null)
            {
                _uiManager.ShowNoteSprite(true);
                _uiManager.ShowLevelInfo(false, 0.5f);
            }
            
            _smartNoteGenerator?.GenerateNote();
        }
        
        public void HideLevelInfo()
        {
            _uiManager?.ShowLevelInfo(false, 0.5f);
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
            if (CurrentLevel == null || _isLevelCompleting) return;
            
            currentLevelScore += points;
            
            if (currentLevelScore >= CurrentLevel.requiredScore)
            {
                LevelComplete();
            }
        }
        
        private void LevelComplete()
        {
            if (_isLevelCompleting) return;
            _isLevelCompleting = true;
            
            int oldHighest = PlayerPrefs.GetInt(HIGHEST_LEVEL_KEY, 0);
            
            if (CurrentLevelNumber > oldHighest)
            {
                PlayerPrefs.SetInt(HIGHEST_LEVEL_KEY, CurrentLevelNumber);
            }
            
            int nextLevel = CurrentLevelNumber + 1;
            if (nextLevel <= TotalLevels)
            {
                PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, nextLevel);
            }
            
            if (nextLevel > PlayerPrefs.GetInt(HIGHEST_LEVEL_KEY, 0))
            {
                PlayerPrefs.SetInt(HIGHEST_LEVEL_KEY, nextLevel);
            }
            
            PlayerPrefs.Save();
            
            StartCoroutine(AutoNextLevel(3f));
        }
        
        private IEnumerator AutoNextLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            int nextLevelIndex = currentLevelIndex + 1;
            
            if (nextLevelIndex < levels.Length)
            {
                currentLevelIndex = nextLevelIndex;
                currentLevelScore = 0;
                StartCurrentLevel();
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
        }
        
        public void GoToLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= levels.Length) return;
            
            currentLevelIndex = levelIndex;
            currentLevelScore = 0;
            StartCurrentLevel();
        }
        
        public void ResetProgress()
        {
            currentLevelIndex = 0;
            currentLevelScore = 0;
            PlayerPrefs.DeleteKey(CURRENT_LEVEL_KEY);
            PlayerPrefs.Save();
            StartCurrentLevel();
        }
        
        private void LoadProgress()
        {
            int savedLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 0);
            currentLevelIndex = Mathf.Clamp(savedLevel, 0, levels.Length - 1);
        }
        
        private void SaveProgress()
        {
            PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, currentLevelIndex);
            PlayerPrefs.Save();
        }
        
        public string GetProgressInfo()
        {
            if (CurrentLevel == null) return string.Empty;
            return $"Уровень {CurrentLevelNumber}/{TotalLevels}: {currentLevelScore}/{CurrentLevel.requiredScore}";
        }
        
        public bool IsLevelCompleted()
        {
            return CurrentLevel != null && currentLevelScore >= CurrentLevel.requiredScore;
        }
        
        public void SetSmartNoteGenerator(SmartNoteGenerator generator)
        {
            _smartNoteGenerator = generator;
        }
    }
}