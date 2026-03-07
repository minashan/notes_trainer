using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button exitToMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backToLevelsButton;

    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSound;
    
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    
    private void Start()
    {
        InitializeButtons();
    }
    
    private void InitializeButtons()
    {
        if (backToLevelsButton != null)
        {
            backToLevelsButton.onClick.RemoveAllListeners();
            backToLevelsButton.onClick.AddListener(BackToLevels);
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartLevel);
        }
        
        if (exitToMenuButton != null)
        {
            exitToMenuButton.onClick.RemoveAllListeners();
            exitToMenuButton.onClick.AddListener(ExitToMenu);
        }
    }
    
    private void ExitToMenu()
    {
        SceneNavigator.Instance?.LoadKeySelection();
    }
    
    private void RestartLevel()
    {
        int currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 1);
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, currentLevel);
        PlayerPrefs.Save();
        
        SceneNavigator.Instance?.RestartCurrentLevel();
    }
    
    private void BackToLevels()
    {
        SceneNavigator.Instance?.LoadLevelSelection();
    }
}