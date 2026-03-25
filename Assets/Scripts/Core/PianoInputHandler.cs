using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PianoInputHandler : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private bool enableSound = true;
    
    [Header("Key Colors")]
    [SerializeField] private Color correctKeyColor = Color.green;
    [SerializeField] private Color incorrectKeyColor = Color.red;

    [Header("Hint Settings")]
    [SerializeField] private Color hintKeyColor = new Color(0.62f, 0.94f, 0.62f, 1f); // тускло-зелёный
    [SerializeField] private float pulseDuration = 1.5f;
    [SerializeField] private float pulseSpeed = 1.5f;

   

    private AudioSource _currentPlayingAudioSource;
    private Coroutine _currentPulseCoroutine;
    
    private const float COLOR_RESET_DELAY = 1f;
    
    public void ProcessKeyPress(string pressedNote, GameObject pressedKey)
    {
        PlayKeySound(pressedKey);
    }
    
    public void SetKeyFinalColor(GameObject keyObject, bool isCorrect)
{
    if (isCorrect)
    {
        HighlightCorrectKey(GetNoteNameFromKey(keyObject));
    }
    else
    {
        // для неправильного ответа
        Image keyImage = keyObject.GetComponent<Image>();
        if (keyImage != null)
        {
            keyImage.color = incorrectKeyColor;
        }
        StartCoroutine(ResetKeyAfterDelay(keyObject, COLOR_RESET_DELAY));
    }
}

private string GetNoteNameFromKey(GameObject keyObject)
{
    PianoKey pianoKey = keyObject.GetComponent<PianoKey>();
    return pianoKey != null ? pianoKey.GetNoteName() : "";
}
    
    private IEnumerator ResetKeyAfterDelay(GameObject keyObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetKeyColor(keyObject);
    }
    
    public void ResetKeyColor(GameObject keyObject)
    {
        Debug.Log($"ResetKeyColor called for {keyObject.name}");
        Image keyImage = keyObject.GetComponent<Image>();
        if (keyImage == null) return;
        
        bool isBlackKey = keyObject.name.Contains("#") || 
                          keyObject.name.Contains("sharp") || 
                          keyObject.name.Contains("flat") ||
                          keyObject.name.Contains("b");
        
        keyImage.color = isBlackKey ? Color.black : Color.white;
    }
    
    private void PlayKeySound(GameObject pressedKey)
{
    if (!enableSound) return;
    
    if (AudioManager.Instance != null && !AudioManager.Instance.IsMuted)
    {
        AudioSource audioSource = pressedKey.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            // Останавливаем предыдущий звук, если он есть
            if (_currentPlayingAudioSource != null && _currentPlayingAudioSource.isPlaying)
            {
                _currentPlayingAudioSource.Stop();
            }
            
            // Запускаем новый звук
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
        keyImage.color = new Color(0.2f, 1f, 0.2f, 1f); // ярко-зелёный для чёрных
    }
    else
    {
        keyImage.color = hintKeyColor; // твой цвет #9DEF9D
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
    foreach (var key in allKeys)
    {
        if (key.GetNoteName() == note)
            return key.gameObject;
    }
    return null;
}


private IEnumerator PulseKey(GameObject key)
{
    Image keyImage = key.GetComponent<Image>();
    if (keyImage == null) yield break;
    
    // Запоминаем исходный цвет (белый или чёрный)
    Color originalColor = keyImage.color;
    float elapsed = 0f;
    
    while (elapsed < pulseDuration)
    {
        float t = Mathf.PingPong(elapsed * pulseSpeed, 1f);
        keyImage.color = Color.Lerp(originalColor, hintKeyColor, t);
        elapsed += Time.deltaTime;
        yield return null;
    }
    
    // После пульсации оставляем тусклый зелёный
    keyImage.color = hintKeyColor;
    _currentPulseCoroutine = null;
}
}