using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HintButton : MonoBehaviour
{
    [Header("Настройки пульсации")]
    public float pulseSpeed = 0.7f;  // медленнее в 3 раза
    public float pulseScale = 1.2f;
    
    [Header("Подсказка")]
    public GameObject hintBubble;
    public TextMeshProUGUI hintText;
    
    private Button button;
    private RectTransform rectTransform;
    private Vector3 originalScale;
    private bool isPulsing = false;
    private Coroutine pulseCoroutine;
    
    void Start()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        
        button.onClick.AddListener(OnHintButtonClick);
        
        if (hintBubble != null)
            hintBubble.SetActive(false);
        
        // Кнопка всегда активна, пульсация начинается только после ошибок
    }
    
    public void StartPulsing()
    {
        if (!isPulsing)
        {
            isPulsing = true;
            pulseCoroutine = StartCoroutine(PulseAnimation());
        }
    }
    
    public void StopPulsing()
    {
        if (isPulsing)
        {
            isPulsing = false;
            if (pulseCoroutine != null)
                StopCoroutine(pulseCoroutine);
            rectTransform.localScale = originalScale;
        }
    }
    
    IEnumerator PulseAnimation()
    {
        while (isPulsing)
        {
            // Увеличиваем
            while (rectTransform.localScale.x < pulseScale)
            {
                rectTransform.localScale += Vector3.one * Time.deltaTime * pulseSpeed;
                yield return null;
            }
            // Уменьшаем
            while (rectTransform.localScale.x > originalScale.x)
            {
                rectTransform.localScale -= Vector3.one * Time.deltaTime * pulseSpeed;
                yield return null;
            }
            
            // Небольшая пауза между пульсациями
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    void OnHintButtonClick()
{
    Debug.Log($"Hint clicked");
    
    // ⭐ ОСТАНАВЛИВАЕМ ПУЛЬСАЦИЮ СРАЗУ ПРИ НАЖАТИИ
    StopPulsing();
    
    string correctNoteName = GetCurrentCorrectNoteName();
    Debug.Log($"Correct note name: {correctNoteName}");
    
    ShowHintBubble(correctNoteName);
}
    
    string GetCurrentCorrectNoteName()
{
    GameManager gameManager = FindFirstObjectByType<GameManager>();
    
    if (gameManager != null)
    {
        string englishNote = gameManager.GetCurrentNoteName();
        string russianNote = NoteData.Instance.GetTranslatedNoteName(englishNote);
        
        if (!string.IsNullOrEmpty(russianNote))
        {
            Debug.Log($"Нота: {englishNote} -> {russianNote}");
            return russianNote;
        }
    }
    
    Debug.LogWarning("Не удалось получить ноту, возвращаем 'До'");
    return "До";
}
    

    
    void ShowHintBubble(string noteName)
    {
        Debug.Log($"ShowHintBubble called with note: {noteName}");
        Debug.Log($"hintBubble: {hintBubble != null}, hintText: {hintText != null}");
        
        if (hintBubble == null || hintText == null)
        {
            Debug.LogError("HintBubble или HintText не назначены в инспекторе!");
            return;
        }
        
        hintText.text = noteName;
        hintBubble.SetActive(true);
        Debug.Log($"HintBubble active: {hintBubble.activeSelf}");
        
        StartCoroutine(HideHintAfterDelay(2f));
    }
    
    IEnumerator HideHintAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hintBubble.SetActive(false);
    }
    
    // Для вызова из другого скрипта (например LevelManager при ошибках)
    public void ActivateHint()
    {
        StartPulsing();
    }
}