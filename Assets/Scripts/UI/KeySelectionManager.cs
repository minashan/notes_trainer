using UnityEngine;
using UnityEngine.UI;

public class KeySelectionManager : MonoBehaviour
{
    [Header("Exit Button")]
    [SerializeField] private Button exitButton;

    [SerializeField] private Button trebleClefButton;
    [SerializeField] private Button bassClefButton;
    [SerializeField] private GameObject bassLockedPanel;
    
    
    void Start()
    {
        if (trebleClefButton != null)
        {
            trebleClefButton.onClick.AddListener(OnTrebleClefClicked);
        }
        
        if (bassClefButton != null)
        {
            bassClefButton.interactable = false;
            bassClefButton.onClick.AddListener(OnBassClefClicked);
        }
        
        if (bassLockedPanel != null)
        {
            bassLockedPanel.SetActive(true);
        }
        
        if (exitButton != null)
{
    exitButton.onClick.RemoveAllListeners();
    exitButton.onClick.AddListener(OnExitButtonClicked);
}
else
{
    Debug.LogError("ExitButton не назначен в KeySelectionManager!");
}
}
    
    void OnTrebleClefClicked()
    {
        Debug.Log("Скрипичный ключ выбран");
        SceneNavigator.Instance.LoadLevelSelection();
    }
    
    void OnBassClefClicked()
    {
        Debug.Log("Басовый ключ заблокирован");
    }



    private void OnExitButtonClicked()
{
    Debug.Log("Выход из приложения");
    Application.Quit();
    
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #endif
}
}