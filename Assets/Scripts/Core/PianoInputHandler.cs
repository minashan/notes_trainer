using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PianoInputHandler : MonoBehaviour
{
    [Header("Настройки звука")]
    public bool enableSound = true;
    public float volume = 1.0f;
    
    [Header("Цвета клавиш")]
    public Color correctKeyColor = Color.green;
    public Color incorrectKeyColor = Color.red;
    
    public void ProcessKeyPress(string pressedNote, GameObject pressedKey)
    {
        Debug.Log($"[InputHandler] Key: {pressedNote}");
        PlayKeySound(pressedKey); // только звук
    }
    
    public void SetKeyFinalColor(GameObject keyObject, bool isCorrect)
    {
        // Сразу устанавливаем цвет результата
        Image keyImage = keyObject.GetComponent<Image>();
        if (keyImage != null)
        {
            keyImage.color = isCorrect ? correctKeyColor : incorrectKeyColor;
        }
        
        // Сбрасываем через секунду
        StartCoroutine(ResetKeyAfterDelay(keyObject, 1f));
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
        
        // Определяем исходный цвет
        bool isBlackKey = keyObject.name.Contains("#") || 
                          keyObject.name.Contains("sharp") || 
                          keyObject.name.Contains("flat") ||
                          keyObject.name.Contains("b");
        
        keyImage.color = isBlackKey ? Color.black : Color.white;
    }
    
    private void PlayKeySound(GameObject pressedKey)
    {
        if (!enableSound) return;
        
        AudioSource audioSource = pressedKey.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.volume = volume;
            audioSource.Play();
        }
    }
}