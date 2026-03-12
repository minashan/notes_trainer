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
    
    _highestLevel = PlayerPrefs.GetInt(HIGHEST_LEVEL_KEY, 1);
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
        _highestLevel = PlayerPrefs.GetInt(HIGHEST_LEVEL_KEY, 1);
        
        for (int i = 0; i < levelButtons.Count && i < TOTAL_LEVELS; i++)
        {
            int levelIndex = i + 1;
            LevelButton levelButton = levelButtons[i];
            
            if (levelButton == null) continue;
            
            SetButtonTexts(levelButton, levelIndex, i);
            
            bool isUnlocked = levelIndex == 1 || levelIndex <= _highestLevel;
            
            if (isUnlocked)
            {
                SetupAvailableButton(levelButton, levelIndex);
            }
            else
            {
                SetupLockedButton(levelButton);
            }
        }
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
    
    private void SetupAvailableButton(LevelButton levelButton, int levelIndex)
    {
        levelButton.button.interactable = true;
        
        if (levelButton.lockIcon != null)
        {
            levelButton.lockIcon.gameObject.SetActive(false);
        }
        
        levelButton.button.onClick.RemoveAllListeners();
        levelButton.button.onClick.AddListener(() => LoadLevel(levelIndex));
    }
    
    private void LoadLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, levelIndex);
        PlayerPrefs.Save();
        SceneNavigator.Instance?.LoadGameWithLevel(levelIndex);
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
        PlayerPrefs.DeleteKey(HIGHEST_LEVEL_KEY);
        PlayerPrefs.DeleteKey(CURRENT_LEVEL_KEY);
        
        for (int i = 1; i <= TOTAL_LEVELS; i++)
        {
            PlayerPrefs.DeleteKey(string.Format(LEVEL_PROGRESS_KEY, i));
        }
        
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