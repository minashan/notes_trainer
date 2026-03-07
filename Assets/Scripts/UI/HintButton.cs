using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HintButton : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private float pulseSpeed = 0.7f;
    [SerializeField] private float pulseScale = 1.2f;
    
    [Header("Hint Settings")]
    [SerializeField] private GameObject hintBubble;
    [SerializeField] private TextMeshProUGUI hintText;
    
    private Button _button;
    private RectTransform _rectTransform;
    private Vector3 _originalScale;
    private bool _isPulsing;
    private Coroutine _pulseCoroutine;
    
    private const float HINT_DISPLAY_TIME = 2f;
    private const float PULSE_PAUSE = 0.2f;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
        _originalScale = _rectTransform.localScale;
        
        _button.onClick.AddListener(OnHintButtonClick);
        
        if (hintBubble != null)
        {
            hintBubble.SetActive(false);
        }
    }
    
    public void StartPulsing()
    {
        if (_isPulsing) return;
        
        _isPulsing = true;
        _pulseCoroutine = StartCoroutine(PulseAnimation());
    }
    
    public void StopPulsing()
    {
        if (!_isPulsing) return;
        
        _isPulsing = false;
        
        if (_pulseCoroutine != null)
        {
            StopCoroutine(_pulseCoroutine);
        }
        
        _rectTransform.localScale = _originalScale;
    }
    
    private IEnumerator PulseAnimation()
    {
        while (_isPulsing)
        {
            // Scale up
            while (_rectTransform.localScale.x < pulseScale)
            {
                _rectTransform.localScale += Vector3.one * (Time.deltaTime * pulseSpeed);
                yield return null;
            }
            
            // Scale down
            while (_rectTransform.localScale.x > _originalScale.x)
            {
                _rectTransform.localScale -= Vector3.one * (Time.deltaTime * pulseSpeed);
                yield return null;
            }
            
            yield return new WaitForSeconds(PULSE_PAUSE);
        }
    }
    
    private void OnHintButtonClick()
    {
        StopPulsing();
        
        string correctNoteName = GetCurrentCorrectNoteName();
        ShowHintBubble(correctNoteName);
    }
    
    private string GetCurrentCorrectNoteName()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        
        if (gameManager == null) return "До";
        
        string englishNote = gameManager.GetCurrentNoteName();
        return NoteData.Instance.GetTranslatedNoteName(englishNote);
    }
    
    private void ShowHintBubble(string noteName)
    {
        if (hintBubble == null || hintText == null) return;
        
        hintText.text = noteName;
        hintBubble.SetActive(true);
        
        StartCoroutine(HideHintAfterDelay(HINT_DISPLAY_TIME));
    }
    
    private IEnumerator HideHintAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hintBubble.SetActive(false);
    }
    
    public void ActivateHint()
    {
        StartPulsing();
    }
}