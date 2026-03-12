using UnityEngine;
using UnityEngine.UI;
using NotesTrainer; // 🔥 Добавляем

public class KeySelectionManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button trebleClefButton;
    [SerializeField] private Button bassClefButton;
    
    [Header("Bass Clef Lock")]
    [SerializeField] private GameObject bassLockedPanel; // Можно будет удалить позже
    
    private void Start()
    {
        InitializeButtons();
    }
    
    private void InitializeButtons()
    {
        if (trebleClefButton != null)
        {
            trebleClefButton.onClick.AddListener(OnTrebleClefClicked);
        }
        
        if (bassClefButton != null)
        {
            // 🔥 Разблокируем кнопку басового ключа
            bassClefButton.interactable = true;
            bassClefButton.onClick.AddListener(OnBassClefClicked);
        }
        
        // 🔥 Прячем панель блокировки (если она есть)
        if (bassLockedPanel != null)
        {
            bassLockedPanel.SetActive(false);
        }
        
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }
    
    private void OnTrebleClefClicked()
    {
        // 🔥 Сохраняем выбор скрипичного ключа
        SceneNavigator.Instance.SaveSelectedClef(ClefType.Treble);
        SceneNavigator.Instance?.LoadLevelSelection();
    }
    
    private void OnBassClefClicked()
    {
        // 🔥 Сохраняем выбор басового ключа
        SceneNavigator.Instance.SaveSelectedClef(ClefType.Bass);
        SceneNavigator.Instance?.LoadLevelSelection();
    }
    
    private void OnExitButtonClicked()
    {
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}