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

    private AudioSource _currentPlayingAudioSource;
    
    private const float COLOR_RESET_DELAY = 1f;
    
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
}