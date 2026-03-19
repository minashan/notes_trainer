using System;
using UnityEngine;
using System.Collections;

namespace NotesTrainer
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Level Lists")]
        [SerializeField] private LevelData[] trebleLevels;
        [SerializeField] private LevelData[] bassLevels;

        [Header("Current State")]
        [SerializeField] private int currentLevelIndex;
        [SerializeField] private int currentLevelScore;
        
        private GameManager _gameManager;
        private UIManager _uiManager;
        private SmartNoteGenerator _smartNoteGenerator;
        
        private bool _isLevelCompleting;
        private bool _firstNote = true;
        
        private LevelData[] _currentLevels;
        private ClefType _currentClef;
        
        private const string TREBLE_HIGHEST_KEY = "TrebleHighestLevel";
        private const string BASS_HIGHEST_KEY = "BassHighestLevel";
        private const string TREBLE_CURRENT_KEY = "TrebleCurrentLevel";
        private const string BASS_CURRENT_KEY = "BassCurrentLevel";
        
        public LevelData CurrentLevel => 
            _currentLevels != null && currentLevelIndex < _currentLevels.Length ? 
            _currentLevels[currentLevelIndex] : null;
        
        public int CurrentLevelNumber => currentLevelIndex + 1;
        public int TotalLevels => _currentLevels?.Length ?? 0;
        
        private void Start()
        {
            _gameManager = FindFirstObjectByType<GameManager>();
            _uiManager = FindFirstObjectByType<UIManager>();
            
            _currentClef = SceneNavigator.Instance.LoadSelectedClef();
            _currentLevels = _currentClef == ClefType.Treble ? trebleLevels : bassLevels;
            
            int savedHighest = GetHighestLevel();
            int selectedLevel = GetCurrentLevel();
            
            if (selectedLevel > savedHighest) selectedLevel = savedHighest;
            
            currentLevelIndex = Mathf.Clamp(selectedLevel - 1, 0, _currentLevels.Length - 1);
        }
        
        private int GetHighestLevel()
        {
            string key = _currentClef == ClefType.Treble ? TREBLE_HIGHEST_KEY : BASS_HIGHEST_KEY;
            return PlayerPrefs.GetInt(key, 1);
        }
        
        private int GetCurrentLevel()
        {
            string key = _currentClef == ClefType.Treble ? TREBLE_CURRENT_KEY : BASS_CURRENT_KEY;
            return PlayerPrefs.GetInt(key, 1);
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
                _smartNoteGenerator.SetLevel(CurrentLevel);
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
    
    string highestKey = _currentClef == ClefType.Treble ? "TrebleHighestLevel" : "BassHighestLevel";
    string currentKey = _currentClef == ClefType.Treble ? "TrebleCurrentLevel" : "BassCurrentLevel";
    
    int oldHighest = PlayerPrefs.GetInt(highestKey, 0);
    Debug.Log($"LevelComplete: CurrentLevel={CurrentLevelNumber}, oldHighest={oldHighest}");
    
    if (CurrentLevelNumber > oldHighest)
    {
        PlayerPrefs.SetInt(highestKey, CurrentLevelNumber);
        Debug.Log($"Saving {highestKey} = {CurrentLevelNumber}");
    }
    
    int nextLevel = CurrentLevelNumber + 1;
    if (nextLevel <= TotalLevels)
    {
        PlayerPrefs.SetInt(currentKey, nextLevel);
        Debug.Log($"Saving {currentKey} = {nextLevel}");
        
        // 🔥 ВАЖНО: также сохраняем следующий уровень в highestKey
        if (nextLevel > oldHighest)
        {
            PlayerPrefs.SetInt(highestKey, nextLevel);
            Debug.Log($"Also saving {highestKey} = {nextLevel} (next level)");
        }
    }
    
    PlayerPrefs.Save();
    
    StartCoroutine(AutoNextLevel(3f));
}

        
        private IEnumerator AutoNextLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            int nextLevelIndex = currentLevelIndex + 1;
            
            if (nextLevelIndex < _currentLevels.Length)
            {
                currentLevelIndex = nextLevelIndex;
                currentLevelScore = 0;
                StartCurrentLevel();
            }
        }
        
        public void GoToNextLevel()
        {
            int nextLevelIndex = currentLevelIndex + 1;
            
            if (nextLevelIndex < _currentLevels.Length)
            {
                currentLevelIndex = nextLevelIndex;
                currentLevelScore = 0;
                StartCurrentLevel();
            }
        }
        
        public void GoToLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= _currentLevels.Length) return;
            
            currentLevelIndex = levelIndex;
            currentLevelScore = 0;
            StartCurrentLevel();
        }
        
        public void ResetProgress()
        {
            string currentKey = _currentClef == ClefType.Treble ? TREBLE_CURRENT_KEY : BASS_CURRENT_KEY;
            string highestKey = _currentClef == ClefType.Treble ? TREBLE_HIGHEST_KEY : BASS_HIGHEST_KEY;
            
            currentLevelIndex = 0;
            currentLevelScore = 0;
            
            PlayerPrefs.DeleteKey(currentKey);
            PlayerPrefs.SetInt(highestKey, 1); // Сбрасываем на 1 уровень
            PlayerPrefs.Save();
            
            StartCurrentLevel();
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