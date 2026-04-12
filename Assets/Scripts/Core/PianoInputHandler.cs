using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // 🔥 Добавлено

public class PianoInputHandler : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private bool enableSound = true;
    
    [Header("Key Colors")]
    [SerializeField] private Color correctKeyColor = Color.green;
    [SerializeField] private Color incorrectKeyColor = Color.red;

    [Header("Hint Settings")]
    [SerializeField] private Color hintKeyColor = new Color(0.62f, 0.94f, 0.62f, 1f);
    [SerializeField] private float pulseDuration = 1.5f;
    [SerializeField] private float pulseSpeed = 1.5f;

    private AudioSource _currentPlayingAudioSource;
    private Coroutine _currentPulseCoroutine;
    
    private const float COLOR_RESET_DELAY = 1f;
    
    private Dictionary<GameObject, Color> _originalColors = new Dictionary<GameObject, Color>();
    
    void Start()
    {
        // Сохраняем исходные цвета всех клавиш
        foreach (var key in FindObjectsByType<PianoKey>(FindObjectsSortMode.None))
        {
            Image img = key.GetComponent<Image>();
            if (img != null)
            {
                _originalColors[key.gameObject] = img.color;
            }
        }
    }
    
    public void ProcessKeyPress(string pressedNote, GameObject pressedKey)
    {
        PlayKeySound(pressedKey);
    }
    
    public void SetKeyFinalColor(GameObject keyObject, bool isCorrect)
    {
        Image keyImage = keyObject.GetComponent<Image>();
        if (keyImage == null) return;
        
        keyImage.color = isCorrect ? correctKeyColor : incorrectKeyColor;
        StartCoroutine(ResetKeyAfterDelay(keyObject, COLOR_RESET_DELAY));
    }
    
    private IEnumerator ResetKeyAfterDelay(GameObject keyObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetKeyColor(keyObject);
    }
    
    private void ResetKeyColor(GameObject keyObject)
    {
        Image keyImage = keyObject.GetComponent<Image>();
        if (keyImage == null) return;
        
        // Возвращаем исходный цвет
        if (_originalColors.ContainsKey(keyObject))
        {
            keyImage.color = _originalColors[keyObject];
        }
        else
        {
            // fallback
            bool isBlackKey = keyObject.name.Contains("#") || 
                              keyObject.name.Contains("sharp") || 
                              keyObject.name.Contains("flat") ||
                              keyObject.name.Contains("b");
            keyImage.color = isBlackKey ? Color.black : Color.white;
        }
    }
    
    private string GetNoteNameFromKey(GameObject keyObject)
    {
        PianoKey pianoKey = keyObject.GetComponent<PianoKey>();
        return pianoKey != null ? pianoKey.GetNoteName() : "";
    }
    
    private void PlayKeySound(GameObject pressedKey)
    {
        if (!enableSound) return;
        
        if (AudioManager.Instance != null && !AudioManager.Instance.IsMuted)
        {
            AudioSource audioSource = pressedKey.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                if (_currentPlayingAudioSource != null && _currentPlayingAudioSource.isPlaying)
                {
                    _currentPlayingAudioSource.Stop();
                }
                
                audioSource.Play();
                _currentPlayingAudioSource = audioSource;
            }
        }
    }
    
    public void StopPulsing()
    {
        if (_currentPulseCoroutine != null)
        {
            StopCoroutine(_currentPulseCoroutine);
            _currentPulseCoroutine = null;
        }
    }
    
    public void HighlightHintKey(string correctNote)
    {
        GameObject correctKey = FindKeyByNote(correctNote);
        if (correctKey == null) return;
        
        Image keyImage = correctKey.GetComponent<Image>();
        if (keyImage == null) return;
        
        bool isBlackKey = correctKey.name.Contains("#") || 
                          correctKey.name.Contains("sharp") || 
                          correctKey.name.Contains("flat") ||
                          correctKey.name.Contains("b");
        
        if (isBlackKey)
        {
            keyImage.color = new Color(0.2f, 1f, 0.2f, 1f);
        }
        else
        {
            keyImage.color = hintKeyColor;
        }
    }
    
    public void HighlightCorrectKey(string correctNote)
    {
        Debug.Log($"HighlightCorrectKey: {correctNote}");
        GameObject correctKey = FindKeyByNote(correctNote);
        Debug.Log($"Found: {correctKey != null}");
        if (correctKey == null) return;
        
        Image keyImage = correctKey.GetComponent<Image>();
        if (keyImage != null)
        {
            keyImage.color = correctKeyColor;
        }
    }
    
   public GameObject FindKeyByNote(string note)
{
    PianoKey[] allKeys = FindObjectsByType<PianoKey>(FindObjectsSortMode.None);
    
    // Прямой поиск
    foreach (var key in allKeys)
    {
        if (key.GetNoteName() == note)
            return key.gameObject;
    }
    
    // Поиск по энгармонизму (для бемолей и диезов)
    foreach (var key in allKeys)
    {
        string keyNote = key.GetNoteName();
        if (NoteData.Instance.AreNotesEnharmonic(note, keyNote))
            return key.gameObject;
    }
    
    return null;
}
    
    private IEnumerator PulseKey(GameObject key)
    {
        Image keyImage = key.GetComponent<Image>();
        if (keyImage == null) yield break;
        
        Color originalColor = keyImage.color;
        float elapsed = 0f;
        
        while (elapsed < pulseDuration)
        {
            float t = Mathf.PingPong(elapsed * pulseSpeed, 1f);
            keyImage.color = Color.Lerp(originalColor, hintKeyColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        keyImage.color = hintKeyColor;
        _currentPulseCoroutine = null;
    }
}