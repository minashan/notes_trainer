using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Button exitToMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backToLevelsButton;
    
   void Start()
{
    Debug.Log("=== GameUIController Start ===");
    
    // Кнопка "← УРОВНИ"
    if (backToLevelsButton != null)
    {
        Debug.Log($"Back button найден: {backToLevelsButton.name}");
        backToLevelsButton.onClick.RemoveAllListeners();
        backToLevelsButton.onClick.AddListener(() => {
            Debug.Log("Кнопка '← УРОВНИ' нажата в GameUIController!");
            BackToLevels();
        });
    }
    else
    {
        Debug.LogError("BackToLevelsButton не назначен в GameUIController!");
    }
    
    // Кнопка "ЗАНОВО"
    if (restartButton != null)
    {
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => {
            Debug.Log("Кнопка 'ЗАНОВО' нажата");
            RestartLevel();
        });
    }
    
    // Кнопка "В МЕНЮ"
    if (exitToMenuButton != null)
    {
        exitToMenuButton.onClick.RemoveAllListeners();
        exitToMenuButton.onClick.AddListener(() => {
            Debug.Log("Кнопка 'В МЕНЮ' нажата");
            ExitToMenu();
        });
    }
    
    Debug.Log("=== GameUIController инициализирован ===");
}
    
    void ExitToMenu()
    {
        Debug.Log("Выход в меню выбора ключа");
        SceneNavigator.Instance.LoadKeySelection();
    }
    
    void RestartLevel()
{
    int currentBefore = PlayerPrefs.GetInt("CurrentLevel", 1);
    Debug.Log($"Рестарт уровня. CurrentLevel до: {currentBefore}");
    
    SceneNavigator.Instance.RestartCurrentLevel();
    
    // Можно проверить после
    int currentAfter = PlayerPrefs.GetInt("CurrentLevel", 1);
    Debug.Log($"CurrentLevel после: {currentAfter}");
}

     void BackToLevels()
{
    Debug.Log("Назад к выбору уровней");
    
    // Загружаем сцену выбора уровней
    SceneNavigator.Instance.LoadLevelSelection();
    
    // НЕЛЬЗЯ вызвать RefreshLevelButtons() тут - сцена ещё не загрузилась
    // Обновление произойдёт автоматически в Start() LevelSelectionManager
}
}