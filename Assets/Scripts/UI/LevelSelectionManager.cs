using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using NotesTrainer;

public class LevelSelectionManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        public Button button;
        public TextMeshProUGUI numberText;
        public TextMeshProUGUI descriptionText;
        public Image lockIcon;
    }
    

    [Header("Level Buttons")]
    [SerializeField] private List<LevelButton> levelButtons = new();
    
    [Header("Navigation")]
    [SerializeField] private Button backButton;
    
    [Header("Reset Progress")]
    [SerializeField] private Button resetProgressButton;
    [SerializeField] private GameObject resetConfirmationPanel;
    [SerializeField] private Button confirmResetButton;
    [SerializeField] private Button cancelResetButton;
    
    [Header("Colors")]
    [SerializeField] private Color lockedColor = new(0.619f, 0.619f, 0.619f);

    [Header("Level Lists")]
    [SerializeField] private LevelData[] trebleLevels;
    [SerializeField] private LevelData[] bassLevels;
    
    private const int TOTAL_LEVELS = 8;
    private const string HIGHEST_LEVEL_KEY = "HighestLevel";
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    private const string LEVEL_PROGRESS_KEY = "Level{0}_Progress";

    private const string TREBLE_HIGHEST_KEY = "TrebleHighestLevel";
    private const string BASS_HIGHEST_KEY = "BassHighestLevel";
    private const string TREBLE_CURRENT_KEY = "TrebleCurrentLevel"; // 🔥 добавить
    private const string BASS_CURRENT_KEY = "BassCurrentLevel";     // 🔥 добавить
    

    
    private readonly string[] _levelDescriptions = 
    {
        "First Octave",
        "Second Octave",
        "Two Octaves",
        "Lower + Third Octaves",
        "All Notes (No Accidentals)",
        "Sharps (#)",
        "Flats (b)",
        "All Notes"
    };
    
    private int _highestLevel;
    
    private LevelData[] _currentLevels;
    private ClefType _currentClef;

    private void Start()
{
    _currentClef = SceneNavigator.Instance.LoadSelectedClef();
    _currentLevels = _currentClef == ClefType.Treble ? trebleLevels : bassLevels;
    
    string highestKey = _currentClef == ClefType.Treble ? "TrebleHighestLevel" : "BassHighestLevel";
    _highestLevel = PlayerPrefs.GetInt(highestKey, 1);
    
    InitializeUI();
}


    
    private void InitializeUI()
    {
        InitializeButtons();
        SetupNavigationButtons();
        SetupResetButtons();
        
        if (resetConfirmationPanel != null)
        {
            resetConfirmationPanel.SetActive(false);
        }
    }
    
    private void InitializeButtons()
{
    // Определяем текущий ключ
    ClefType currentClef = SceneNavigator.Instance.LoadSelectedClef();
    
    // Выбираем нужный массив уровней
    LevelData[] levelsToUse = currentClef == ClefType.Treble ? trebleLevels : bassLevels;
    
    // Сначала отключаем все кнопки
    foreach (var btn in levelButtons)
    {
        if (btn?.button != null)
            btn.button.gameObject.SetActive(false);
    }
    
    // Включаем только те кнопки, для которых есть уровни
    int levelsCount = Mathf.Min(levelButtons.Count, levelsToUse.Length);
    for (int i = 0; i < levelsCount; i++)
    {
        int levelIndex = i + 1;
        LevelButton levelButton = levelButtons[i];
        LevelData levelData = levelsToUse[i];
        
        if (levelButton == null || levelData == null) continue;
        
        // Активируем кнопку
        levelButton.button.gameObject.SetActive(true);
        
        // Устанавливаем текст из уровня
        if (levelButton.numberText != null)
            levelButton.numberText.text = levelData.levelName;
        
        if (levelButton.descriptionText != null)
            levelButton.descriptionText.text = levelData.description;
        
        // Проверка доступности
        bool isUnlocked = IsLevelUnlocked(currentClef, levelIndex);
        
        if (isUnlocked)
            SetupAvailableButton(levelButton, levelData, levelIndex);
        else
            SetupLockedButton(levelButton);
    }
}

private bool IsLevelUnlocked(ClefType clef, int levelIndex)
{
    if (levelIndex == 1) return true;
    
    string key = clef == ClefType.Treble ? "TrebleHighestLevel" : "BassHighestLevel";
    int highestUnlocked = PlayerPrefs.GetInt(key, 1);
    
    bool unlocked = levelIndex <= highestUnlocked;
    Debug.Log($"Level {levelIndex} unlocked: {unlocked} (highest={highestUnlocked})");
    
    return unlocked;
}


    
    private void SetButtonTexts(LevelButton levelButton, int levelIndex, int arrayIndex)
    {
        if (levelButton.numberText != null)
        {
            levelButton.numberText.text = $"Level {levelIndex}";
        }
        
        if (levelButton.descriptionText != null && arrayIndex < _levelDescriptions.Length)
        {
            levelButton.descriptionText.text = _levelDescriptions[arrayIndex];
        }
    }
    
   private void SetupAvailableButton(LevelButton levelButton, LevelData levelData, int levelIndex)
{
    levelButton.button.interactable = true;
    
    if (levelButton.lockIcon != null)
        levelButton.lockIcon.gameObject.SetActive(false);
    
    levelButton.button.onClick.RemoveAllListeners();
    
    // 🔥 Передаём и уровень, и индекс
    levelButton.button.onClick.AddListener(() => LoadLevel(levelData, levelIndex));
}

private void LoadLevel(LevelData levelData, int levelIndex)
{
    ClefType currentClef = SceneNavigator.Instance.LoadSelectedClef();
    
    // 🔥 СОХРАНЯЕМ ИМЯ УРОВНЯ (а не только индекс)
    PlayerPrefs.SetString("SelectedLevelName", levelData.name);
    PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, levelIndex);
    PlayerPrefs.Save();
    
    // Загружаем игру
    SceneNavigator.Instance.LoadGameWithLevel(levelIndex, currentClef);
}
    
   
    
    private void SetupLockedButton(LevelButton levelButton)
    {
        levelButton.button.interactable = false;
        
        ColorBlock colors = levelButton.button.colors;
        colors.normalColor = lockedColor;
        levelButton.button.colors = colors;
        
        if (levelButton.lockIcon != null)
        {
            levelButton.lockIcon.gameObject.SetActive(true);
        }
    }
    
    private void SetupNavigationButtons()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(() => SceneNavigator.Instance?.LoadKeySelection());
        }
    }
    
    private void SetupResetButtons()
    {
        if (resetProgressButton != null)
        {
            resetProgressButton.onClick.RemoveAllListeners();
            resetProgressButton.onClick.AddListener(() => ShowResetConfirmation(true));
        }
        
        if (confirmResetButton != null)
        {
            confirmResetButton.onClick.RemoveAllListeners();
            confirmResetButton.onClick.AddListener(ResetAllProgress);
        }
        
        if (cancelResetButton != null)
        {
            cancelResetButton.onClick.RemoveAllListeners();
            cancelResetButton.onClick.AddListener(() => ShowResetConfirmation(false));
        }
    }
    
    private void ShowResetConfirmation(bool show)
    {
        if (resetConfirmationPanel == null) return;
        
        resetConfirmationPanel.SetActive(show);
        
        if (show)
        {
            resetConfirmationPanel.transform.SetAsLastSibling();
        }
    }
    
    private void ResetAllProgress()
{
    string highestKey = _currentClef == ClefType.Treble ? TREBLE_HIGHEST_KEY : BASS_HIGHEST_KEY;
    string currentKey = _currentClef == ClefType.Treble ? TREBLE_CURRENT_KEY : BASS_CURRENT_KEY;
    
    PlayerPrefs.DeleteKey(highestKey);
    PlayerPrefs.DeleteKey(currentKey);
    
    // Сброс прогресса по уровням для текущего ключа
    for (int i = 1; i <= 8; i++)
    {
        string progressKey = $"{_currentClef}_Level{i}_Progress";
        PlayerPrefs.DeleteKey(progressKey);
    }
    
    PlayerPrefs.SetInt(highestKey, 1); // 1 уровень доступен
    PlayerPrefs.Save();
    
    ShowResetConfirmation(false);
    SceneNavigator.Instance?.LoadLevelSelection();
}
    
    public void RefreshLevelButtons()
    {
        _highestLevel = PlayerPrefs.GetInt(HIGHEST_LEVEL_KEY, 1);
        InitializeButtons();
    }
}