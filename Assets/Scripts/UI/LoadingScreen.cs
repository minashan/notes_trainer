using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private float loadingTime = 2f;
    [SerializeField] private bool skipButtonEnabled = false;
    
    void Start()
    {
        // Создаём SceneNavigator если его нет
        EnsureSceneNavigator();
        
        // Автопереход через указанное время
        Invoke(nameof(LoadKeySelection), loadingTime);
        
        // Если хочешь кнопку пропуска (опционально)
        if (skipButtonEnabled)
        {
            SetupSkipButton();
        }
    }
    
    void EnsureSceneNavigator()
    {
        if (SceneNavigator.Instance == null)
        {
            Debug.Log("Создаём SceneNavigator...");
            GameObject navigatorObj = new GameObject("SceneNavigator");
            navigatorObj.AddComponent<SceneNavigator>();
        }
        else
        {
            Debug.Log("SceneNavigator уже существует");
        }
    }
    
    void LoadKeySelection()
    {
        Debug.Log("Загрузка завершена, переходим к выбору ключа");
        SceneNavigator.Instance.LoadKeySelection();
    }
    
    void SetupSkipButton()
    {
        // Если в сцене есть кнопка - можно назначить её
        // Пока просто оставляем закомментированным
        // button.onClick.AddListener(LoadKeySelection);
    }
    
    // Для тестирования - переход по нажатию клавиши
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            CancelInvoke(nameof(LoadKeySelection));
            LoadKeySelection();
        }
    }
}