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

    
    private Coroutine _currentPulseCoroutine;
    private AudioSource _currentPlayingAudioSource;
    
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
    
    GameManager gm = FindFirstObjectByType<GameManager>();
    
    if (gm != null)
    {
        // Игровой режим (скрипичный/басовый ключ)
        bool isCorrect = gm.CheckNote(pressedNote);
        SetKeyFinalColor(pressedKey, isCorrect);
        
        if (isCorrect)
            gm.OnCorrectAnswer();
        else
            gm.OnIncorrectAnswer(pressedNote);
    }
    else
    {
        // Режим пианино — серая подсветка
        SetKeyPressedColor(pressedKey);
    }
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
    
    public void ResetKeyColor(GameObject keyObject)
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

    public void SetKeyPressedColor(GameObject keyObject)
{
    Image keyImage = keyObject.GetComponent<Image>();
    if (keyImage == null) return;
    
    // Временно меняем цвет на серый
    keyImage.color = new Color(0.7f, 0.7f, 0.7f);
    
    // Через 0.2 секунды возвращаем исходный цвет
    StartCoroutine(ResetKeyAfterDelay(keyObject, 0.2f));
}


    
    private string GetNoteNameFromKey(GameObject keyObject)
    {
        PianoKey pianoKey = keyObject.GetComponent<PianoKey>();
        return pianoKey != null ? pianoKey.GetNoteName() : "";
    }
    

    


private float _fadeStartTime;
private float _fadeDuration = 0.3f;
private bool _isFading = false;

private void Update()
{
    if (_isFading && _currentPlayingAudioSource != null)
    {
        float elapsed = Time.time - _fadeStartTime;
        float t = Mathf.Clamp01(elapsed / _fadeDuration);
        _currentPlayingAudioSource.volume = Mathf.Lerp(1f, 0f, t);
        
        if (t >= 1f)
        {
            _currentPlayingAudioSource.Stop();
            _currentPlayingAudioSource.volume = 1f;
            _isFading = false;
        }
    }
}




private void PlayKeySound(GameObject pressedKey)
{
    if (!enableSound) return;
    if (AudioManager.Instance == null || AudioManager.Instance.IsMuted) return;
    
    AudioSource audioSource = pressedKey.GetComponent<AudioSource>();
    if (audioSource != null && audioSource.clip != null)
    {
        if (_currentPlayingAudioSource != null && _currentPlayingAudioSource.isPlaying)
        {
            _currentPlayingAudioSource.Stop();
        }
        
        audioSource.Play();
        _currentPlayingAudioSource = audioSource;
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



    private Dictionary<AudioSource, bool> _activeNotes = new Dictionary<AudioSource, bool>();

public void NoteOn(AudioClip clip)
{
    if (AudioManager.Instance == null || AudioManager.Instance.IsMuted) return;
    
    // Если эта нота уже играет — останавливаем и удаляем
    AudioSource existingSource = null;
    foreach (var kvp in _activeNotes)
    {
        if (kvp.Key.clip == clip)
        {
            existingSource = kvp.Key;
            break;
        }
    }
    
    if (existingSource != null)
    {
        existingSource.Stop();
        _activeNotes.Remove(existingSource);
        Destroy(existingSource.gameObject);
    }
    
    AudioSource newSource = GetFreeAudioSource();
    newSource.clip = clip;
    newSource.Play();
    _activeNotes[newSource] = true;
}

public void NoteOff(AudioClip clip)
{
    foreach (var kvp in _activeNotes)
    {
        if (kvp.Key.clip == clip)
        {
            StartCoroutine(FadeOutAndStop(kvp.Key, 0.5f));
            break;
        }
    }
}

private System.Collections.IEnumerator FadeOutAndStop(AudioSource source, float duration)
{
    float startVolume = source.volume;
    float elapsed = 0f;
    
    while (elapsed < duration && source != null)
    {
        elapsed += Time.deltaTime;
        if (source != null)
            source.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
        yield return null;
    }
    
    if (source != null)
    {
        source.Stop();
        _activeNotes.Remove(source);
        Destroy(source.gameObject);
    }
}


private AudioSource GetFreeAudioSource()
{
    GameObject go = new GameObject("NoteSource");
    go.transform.SetParent(AudioManager.Instance.transform);
    AudioSource source = go.AddComponent<AudioSource>();
    source.volume = 1f;
    source.spatialBlend = 0f;
    return source;
}


}