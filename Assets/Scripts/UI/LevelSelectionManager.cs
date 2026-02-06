using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
    
    [Header("Кнопки уровней")]
    [SerializeField] private List<LevelButton> levelButtons = new List<LevelButton>();
    
    [Header("Кнопка назад")]
    [SerializeField] private Button backButton;

    [Header("Кнопка сброса")]
    [SerializeField] private Button resetProgressButton;
    [SerializeField] private GameObject resetConfirmationPanel;
    [SerializeField] private Button confirmResetButton;
    [SerializeField] private Button cancelResetButton;
    
    [Header("Цвета")]
    [SerializeField] private Color lockedColor = new Color(0.619f, 0.619f, 0.619f); // #9E9E9E
    
    [Header("Описания уровней")]
    private string[] levelDescriptions = new string[]
    {
        "Первая октава",
        "Вторая октава", 
        "Две октавы",
        "Малая + третья октавы",
        "Все ноты без альтерации",
        "Диезы (#)",
        "Бемоли (b)",
        "Все ноты"
    };
    
    private int highestLevel;
    
   void Start()
{
    // SceneNavigator код...
    
    highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);
    
    // ОТЛАДКА - можно оставить
    Debug.Log($"HighestLevel: {highestLevel}");
    
    InitializeButtons();
    
    // 1. Кнопка назад
    if (backButton != null)
    {
        backButton.onClick.AddListener(() => {
            SceneNavigator.Instance.LoadKeySelection();
        });
    }
    
    // 2. Кнопка сброса прогресса (ОСНОВНАЯ)
    if (resetProgressButton != null)
    {
        Debug.Log($"Инициализируем кнопку сброса: {resetProgressButton.name}");
        resetProgressButton.onClick.RemoveAllListeners();
        resetProgressButton.onClick.AddListener(() => {
            Debug.Log("Кнопка сброса нажата!");
            ShowResetConfirmation(true);
        });
    }
    else
    {
        Debug.LogError("ResetProgressButton не назначен!");
    }
    
    // 3. Кнопки диалога
    if (confirmResetButton != null)
    {
        Debug.Log($"Инициализируем кнопку ДА: {confirmResetButton.name}");
        confirmResetButton.onClick.RemoveAllListeners();
        confirmResetButton.onClick.AddListener(() => {
            Debug.Log("Кнопка ДА нажата в менеджере!");
            ResetAllProgress();
        });
    }
    
    if (cancelResetButton != null)
    {
        Debug.Log($"Инициализируем кнопку НЕТ: {cancelResetButton.name}");
        cancelResetButton.onClick.RemoveAllListeners();
        cancelResetButton.onClick.AddListener(() => {
            Debug.Log("Кнопка НЕТ нажата в менеджере!");
            ShowResetConfirmation(false);
        });
    }
    
    // 4. Скрываем панель
    if (resetConfirmationPanel != null)
    {
        resetConfirmationPanel.SetActive(false);
    }
}


void ShowResetConfirmation(bool show)
{
    if (resetConfirmationPanel != null)
    {
        resetConfirmationPanel.SetActive(show);
        
        // Если показываем - делаем её поверх всего
        if (show)
        {
            resetConfirmationPanel.transform.SetAsLastSibling();
        }
    }
}

void ResetAllProgress()
{
    // Сбрасываем прогресс
    PlayerPrefs.DeleteKey("HighestLevel");
    PlayerPrefs.DeleteKey("CurrentLevel");
    
    for (int i = 1; i <= 8; i++)
    {
        PlayerPrefs.DeleteKey($"Level{i}_Progress");
    }
    
    PlayerPrefs.Save();
    Debug.Log("Весь прогресс сброшен!");
    
    // Скрываем панель
    ShowResetConfirmation(false);
    
    // Перезагружаем сцену
    SceneNavigator.Instance.LoadLevelSelection();
}
    
    void InitializeButtons()
{
    // ОБНОВЛЯЕМ прогресс перед отрисовкой
    highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);
    Debug.Log($"InitializeButtons: highestLevel = {highestLevel}");
    
    for (int i = 0; i < levelButtons.Count && i < 8; i++)
    {
        int levelIndex = i + 1;
        var levelButton = levelButtons[i];
        
        if (levelButton == null) continue;
        
        // Устанавливаем тексты
        if (levelButton.numberText != null)
            levelButton.numberText.text = $"Уровень {levelIndex}";
        
        if (levelButton.descriptionText != null && i < levelDescriptions.Length)
            levelButton.descriptionText.text = levelDescriptions[i];
        
        // Проверяем статус уровня
        bool isLevel1 = levelIndex == 1;
        bool isUnlocked = levelIndex <= highestLevel;
        
        if (isLevel1 || isUnlocked)
        {
            // Уровень доступен
            SetupAvailableButton(levelButton, levelIndex);
        }
        else
        {
            // Уровень заблокирован
            SetupLockedButton(levelButton);
        }
    }
}
    
    void SetupAvailableButton(LevelButton levelButton, int levelIndex)
{
    Debug.Log($"Настраиваем доступную кнопку уровня {levelIndex}");
    
    levelButton.button.interactable = true;
    
    // Скрываем иконку замка
    if (levelButton.lockIcon != null)
    {
        Debug.Log($"Скрываем замок для уровня {levelIndex}");
        levelButton.lockIcon.gameObject.SetActive(false);
    }
    else
    {
        Debug.LogWarning($"LockIcon не назначен для уровня {levelIndex}");
    }
    
    // Назначаем действие
    levelButton.button.onClick.RemoveAllListeners();
    levelButton.button.onClick.AddListener(() => {
        SceneNavigator.Instance.LoadGameWithLevel(levelIndex);
    });
}

    
    void SetupLockedButton(LevelButton levelButton)
{
    Debug.Log($"Настраиваем заблокированную кнопку");
    
    levelButton.button.interactable = false;
    
    // Серый цвет
    var colors = levelButton.button.colors;
    colors.normalColor = lockedColor;
    levelButton.button.colors = colors;
    
    // Показываем замок
    if (levelButton.lockIcon != null)
    {
        Debug.Log($"Показываем замок");
        levelButton.lockIcon.gameObject.SetActive(true);
    }
}


public void RefreshLevelButtons()
{
    Debug.Log("Обновляем кнопки уровней...");
    
    // Обновляем прогресс
    highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);
    
    // Переинициализируем все кнопки
    InitializeButtons();
    
    Debug.Log($"HighestLevel после обновления: {highestLevel}");
}
}