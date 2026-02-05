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
    // Создаём SceneNavigator если его нет
    if (SceneNavigator.Instance == null)
    {
        Debug.LogWarning("Создаём SceneNavigator...");
        GameObject navigator = new GameObject("SceneNavigator");
        navigator.AddComponent<SceneNavigator>();
    }

    // ОТЛАДКА
    Debug.Log($"=== LevelSelection Debug ===");
    Debug.Log($"HighestLevel: {highestLevel}");
    for (int i = 1; i <= 8; i++)
    {
        int progress = PlayerPrefs.GetInt($"Level{i}_Progress", 0);
        Debug.Log($"Level {i}: Progress = {progress}/14");
    }

    highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);
    InitializeButtons();
    
    if (backButton != null)
    {
        backButton.onClick.AddListener(() => {
            SceneNavigator.Instance.LoadKeySelection();
        });
    }
}
    
    void InitializeButtons()
    {
        for (int i = 0; i < levelButtons.Count && i < 8; i++)
        {
            int levelIndex = i + 1;
            var levelButton = levelButtons[i];
            
            // Устанавливаем тексты
            if (levelButton.numberText != null)
                levelButton.numberText.text = $"Уровень {levelIndex}";
            
            if (levelButton.descriptionText != null && i < levelDescriptions.Length)
                levelButton.descriptionText.text = levelDescriptions[i];
            
            SetupAvailableButton(levelButton, levelIndex); 
        }
    }
    
    void SetupAvailableButton(LevelButton levelButton, int levelIndex)
    {
        levelButton.button.interactable = true;

        // Скрываем иконку замка
        if (levelButton.lockIcon != null)
            levelButton.lockIcon.gameObject.SetActive(false);

        // Назначаем действие
        levelButton.button.onClick.RemoveAllListeners();
        levelButton.button.onClick.AddListener(() => {
            SceneNavigator.Instance.LoadGameWithLevel(levelIndex);
        });
    }
    
    void SetupLockedButton(LevelButton levelButton)
{
    levelButton.button.interactable = false;
    
    // Только для заблокированных - серый цвет
    var colors = levelButton.button.colors;
    colors.normalColor = lockedColor;
    levelButton.button.colors = colors;
    
    // Показываем замок (если добавишь позже)
    if (levelButton.lockIcon != null)
        levelButton.lockIcon.gameObject.SetActive(true);
}
}