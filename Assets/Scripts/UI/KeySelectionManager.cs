using UnityEngine;
using UnityEngine.UI;
using NotesTrainer;
using TMPro;

public class KeySelectionManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button trebleClefButton;
    [SerializeField] private Button bassClefButton;
    
    [Header("Bass Clef Lock")]
    [SerializeField] private GameObject bassLockedPanel; // Можно будет удалить позже

    public TextMeshProUGUI trebleProgressText;
    public TextMeshProUGUI bassProgressText;
    
    private void Start()
    {
        InitializeButtons();
        UpdateProgressTexts();
    }
    
 private void UpdateProgressTexts()
{
    if (trebleProgressText != null)
    {
        int trebleHighest = PlayerPrefs.GetInt("TrebleHighestLevel", 1);
        trebleProgressText.text = $"Уровни: {trebleHighest - 1}/8";
    }
    
    if (bassProgressText != null)
    {
        int bassHighest = PlayerPrefs.GetInt("BassHighestLevel", 1);
        bassProgressText.text = $"Уровни: {bassHighest - 1}/8";
    }
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