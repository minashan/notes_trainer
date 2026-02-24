using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Текстовые элементы")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI streakText;
    public TextMeshProUGUI progressText;
    
    [Header("Настройки анимаций")]
    [SerializeField] private float feedbackFadeDuration = 0.5f;
    [SerializeField] private float feedbackDisplayDuration = 1.5f;
    
    [Header("Цвета")]
    [SerializeField] private Color correctColor = new Color(0f, 0.5f, 0f, 1f); // темно-зеленый
    [SerializeField] private Color incorrectColor = Color.red;
    [SerializeField] private Color defaultTextColor = Color.white;

    [Header("Элементы уровней")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelDescriptionText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] public GameObject levelCompletePanel;
    [SerializeField] private TextMeshProUGUI levelCompleteText;

    [Header("Анимации")]
    [SerializeField] private CanvasGroup levelInfoCanvasGroup;

    [Header("Note Display")]
    [SerializeField] private GameObject noteSprite;
    [SerializeField] private GameObject accidentalSprite;

    [Header("Ledger Lines")]
    [SerializeField] private GameObject ledgerLine1;
    [SerializeField] private GameObject ledgerLine2;
    [SerializeField] private GameObject ledgerLine3;

    
    private Coroutine _currentFeedbackCoroutine;


    public void UpdateLevelInfo(string levelName, string description)
{
    if (levelText != null)
    {
        levelText.text = levelName;
        Debug.Log($"LevelText set: {levelName}");
    }
    
    if (levelDescriptionText != null)
    {
        levelDescriptionText.text = description;
        Debug.Log($"Description set: {description}");
    }
}

public void ShowNoteSprite(bool show)
{
    if (noteSprite != null)
        noteSprite.gameObject.SetActive(show);
    
    if (accidentalSprite != null)
        accidentalSprite.gameObject.SetActive(show);
    
    if (ledgerLine1 != null)
        ledgerLine1.gameObject.SetActive(show);
    
    if (ledgerLine2 != null)
        ledgerLine2.gameObject.SetActive(show);
    
    if (ledgerLine3 != null)
        ledgerLine3.gameObject.SetActive(show);
}

    
    /// <summary>
    /// Показать обратную связь (правильно/неправильно)
    /// </summary>
    public void ShowFeedback(string message, bool isCorrect)
    {
        
        
        if (_currentFeedbackCoroutine != null)
        {
            StopCoroutine(_currentFeedbackCoroutine);
        }
        
        _currentFeedbackCoroutine = StartCoroutine(ShowFeedbackCoroutine(message, isCorrect));
    }
    
    /// <summary>
    /// Показать название ноты (без оценки правильности)
    /// </summary>
    public void ShowNoteName(string noteName, Color color)
    {
        if (feedbackText == null) return;
        
        feedbackText.text = noteName;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Очистить обратную связь
    /// </summary>
    public void ClearFeedback()
    {
        if (feedbackText == null) return;
        
        feedbackText.text = "";
        feedbackText.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Обновить счет
    /// </summary>
    public void UpdateScore(int score, int streak)
    {
        if (scoreText != null)
            scoreText.text = $"Счет: {score}";
        
        if (streakText != null)
            streakText.text = $"Серия: {streak}";
    }
    
    /// <summary>
    /// Корутина для показа обратной связи с анимацией
    /// </summary>
    private IEnumerator ShowFeedbackCoroutine(string message, bool isCorrect)
    {
        if (feedbackText == null) yield break;
        
        // Устанавливаем текст и цвет
        feedbackText.text = message;
        feedbackText.color = isCorrect ? correctColor : incorrectColor;
        feedbackText.gameObject.SetActive(true);
        
        // Ждем указанное время
        yield return new WaitForSeconds(feedbackDisplayDuration);
        
        // Плавно исчезаем
        float elapsedTime = 0f;
        Color startColor = feedbackText.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        while (elapsedTime < feedbackFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / feedbackFadeDuration;
            feedbackText.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        
        // Очищаем
        ClearFeedback();
        _currentFeedbackCoroutine = null;
    }
    
    /// <summary>
    /// Инициализация (можно вызывать из GameManager)
    /// </summary>
    public void Initialize(TextMeshProUGUI feedback, TextMeshProUGUI score, TextMeshProUGUI streak)
    {
        feedbackText = feedback;
        scoreText = score;
        streakText = streak;
        
        ClearFeedback(); // Начинаем с чистого состояния
        
        Debug.Log("[UIManager] Initialized");
    }

    /// <summary>
    /// Обновить отображение уровня
    /// </summary>
    public void UpdateLevelDisplay(int levelNumber, string levelName, string description)
    {
        if (levelText != null)
            levelText.text = levelName;
        
        if (levelDescriptionText != null)
            levelDescriptionText.text = description;
    }

    public void ShowLevelInfo(bool show, float fadeTime = 0.5f)
{
    // Если есть CanvasGroup - используем анимацию
    if (levelInfoCanvasGroup != null)
    {
        StartCoroutine(FadeLevelInfo(show, fadeTime));
    }
    else // Иначе старый способ
    {
        if (levelText != null)
            levelText.gameObject.SetActive(show);
        
        if (levelDescriptionText != null)
            levelDescriptionText.gameObject.SetActive(show);
    }
}

private IEnumerator FadeLevelInfo(bool show, float fadeTime)
{
    if (levelInfoCanvasGroup == null) yield break;
    
    float targetAlpha = show ? 1f : 0f;
    float startAlpha = levelInfoCanvasGroup.alpha;
    float elapsed = 0f;
    
    while (elapsed < fadeTime)
    {
        levelInfoCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeTime);
        elapsed += Time.deltaTime;
        yield return null;
    }
    
    levelInfoCanvasGroup.alpha = targetAlpha;
}
    
  

    public void UpdateProgress(int guessed, int total)
{
    if (progressSlider != null)
    {
        progressSlider.maxValue = total; // ← динамически
        progressSlider.value = guessed;
    }
    
    if (progressText != null)
    {
        progressText.text = $"{guessed}/{total}";
    }
}
    
    /// <summary>
    /// Показать сообщение о завершении уровня
    /// </summary>
    public void ShowLevelComplete(int levelNumber, string levelName)
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            
            if (levelCompleteText != null)
                levelCompleteText.text = $"Уровень {levelNumber} завершён!\n{levelName}";
        }
        
        // Автоматически скрыть через 3 секунды
        Invoke(nameof(HideLevelComplete), 3f);
    }
    
    /// <summary>
    /// Скрыть сообщение о завершении уровня
    /// </summary>
    private void HideLevelComplete()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }
    }
}