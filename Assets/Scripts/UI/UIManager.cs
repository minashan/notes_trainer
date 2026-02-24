using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Текстовые элементы")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    public TextMeshProUGUI progressText;
    
    [Header("Настройки анимаций")]
    [SerializeField] private float feedbackFadeDuration = 0.5f;
    [SerializeField] private float feedbackDisplayDuration = 1.5f;
    
    [Header("Цвета")]
    [SerializeField] private Color correctColor = new Color(0f, 0.5f, 0f, 1f);
    [SerializeField] private Color incorrectColor = Color.red;

    [Header("Элементы уровней")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelDescriptionText;

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

    public void ShowFeedback(string message, bool isCorrect)
    {
        if (_currentFeedbackCoroutine != null)
        {
            StopCoroutine(_currentFeedbackCoroutine);
        }
        
        _currentFeedbackCoroutine = StartCoroutine(ShowFeedbackCoroutine(message, isCorrect));
    }

    public void ShowNoteName(string noteName, Color color)
    {
        if (feedbackText == null) return;
        
        feedbackText.text = noteName;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);
    }

    public void ClearFeedback()
    {
        if (feedbackText == null) return;
        
        feedbackText.text = "";
        feedbackText.gameObject.SetActive(false);
    }

    private IEnumerator ShowFeedbackCoroutine(string message, bool isCorrect)
    {
        if (feedbackText == null) yield break;
        
        feedbackText.text = message;
        feedbackText.color = isCorrect ? correctColor : incorrectColor;
        feedbackText.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(feedbackDisplayDuration);
        
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
        
        ClearFeedback();
        _currentFeedbackCoroutine = null;
    }

    public void Initialize(TextMeshProUGUI feedback)
    {
        feedbackText = feedback;
        ClearFeedback();
        Debug.Log("[UIManager] Initialized");
    }

    public void ShowLevelInfo(bool show, float fadeTime = 0.5f)
    {
        if (levelInfoCanvasGroup != null)
        {
            StartCoroutine(FadeLevelInfo(show, fadeTime));
        }
        else
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
        if (progressText != null)
        {
            progressText.text = $"{guessed}/{total}";
        }
    }
}