using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Button exitToMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backToLevelsButton;
    
    void Start()
    {
        if (exitToMenuButton != null)
        {
            exitToMenuButton.onClick.AddListener(ExitToMenu);
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartLevel);
        }
    }
    
    void ExitToMenu()
    {
        Debug.Log("Выход в меню выбора ключа");
        SceneNavigator.Instance.LoadKeySelection();
    }
    
    void RestartLevel()
    {
        Debug.Log("Рестарт уровня");
        SceneNavigator.Instance.RestartCurrentLevel();
    }

     void BackToLevels()
    {
        Debug.Log("Назад к выбору уровней");
        SceneNavigator.Instance.LoadLevelSelection();
    }
}