using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    private static SceneNavigator _instance;
    public static SceneNavigator Instance => _instance;
    
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // Scene loading methods
    public void LoadLoadingScene() => LoadScene("0_Loading");
    public void LoadKeySelection() => LoadScene("1_KeySelection");
    public void LoadLevelSelection() => LoadScene("2_LevelSelection");
    public void LoadGameScene() => LoadScene("3_Game");
    
    public void LoadGameWithLevel(int levelIndex)
    {
        if (levelIndex < 1 || levelIndex > 8) return;
        
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, levelIndex);
        PlayerPrefs.Save();
        
        LoadGameScene();
    }
    
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void RestartCurrentLevel()
    {
        int currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 1);
        LoadGameWithLevel(currentLevel);
    }
    
    public void ReturnToLevelSelection()
    {
        LoadLevelSelection();
    }
}