using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    private static SceneNavigator _instance;
    public static SceneNavigator Instance => _instance;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        Debug.Log("SceneNavigator initialized");
    }
    
    // Методы для загрузки конкретных сцен
    public void LoadLoadingScene() => LoadScene("0_Loading");
    public void LoadKeySelection() => LoadScene("1_KeySelection");
    public void LoadLevelSelection() => LoadScene("2_LevelSelection");
    public void LoadGameScene() => LoadScene("3_Game");
    
    // Для загрузки игры с конкретным уровнем
    public void LoadGameWithLevel(int levelIndex)
    {
        if (levelIndex < 1 || levelIndex > 8)
        {
            Debug.LogError($"Неверный индекс уровня: {levelIndex}");
            return;
        }
        
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        Debug.Log($"Загружаем уровень {levelIndex}");
        LoadGameScene();
    }
    
    // Общий метод загрузки
    private void LoadScene(string sceneName)
    {
        Debug.Log($"Загружаем сцену: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    
    // Для кнопки "Начать заново" в игровой сцене
    public void RestartCurrentLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        LoadGameWithLevel(currentLevel);
    }
    
    // Для выхода в меню уровней из игры
    public void ReturnToLevelSelection()
    {
        LoadLevelSelection();
    }
}