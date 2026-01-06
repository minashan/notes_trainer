using UnityEngine;
using UnityEngine.UI;
using System;

public class PianoInputHandler : MonoBehaviour
{
    [Header("Настройки звука")]
    public bool enableSound = true;
    public float volume = 1.0f;
    
    [Header("Цвета клавиш")]
    public Color correctKeyColor = Color.green;     // Из GameManager строка 175
    public Color incorrectKeyColor = Color.red;     // Из GameManager строка 195
    
    // Метод обработки нажатия (ЗВУК + ЦВЕТ)
    public (bool shouldProceed, string currentNote) ProcessKeyPress(
        string pressedNote, 
        GameObject pressedKey, 
        NoteGenerator noteGenerator)
    {
        // 1. ЗВУК (из GameManager строки 152-155)
        PlayKeySound(pressedKey);
        
        // 2. Получаем текущую ноту (для GameManager)
        string currentNote = noteGenerator.GetCurrentNote();
        if (string.IsNullOrEmpty(currentNote))
            return (false, null);
        
        // 3. Временный цвет клавиши (окончательный цвет будет после проверки в GameManager)
        SetKeyTemporaryColor(pressedKey, Color.yellow); // или любой промежуточный
        
        return (true, currentNote);
    }
    
    // 4. Установка финального цвета (после проверки в GameManager)
    public void SetKeyFinalColor(GameObject keyObject, bool isCorrect)
    {
        Image keyImage = keyObject?.GetComponent<Image>();
        if (keyImage != null)
        {
            keyImage.color = isCorrect ? correctKeyColor : incorrectKeyColor;
        }
    }
    
    // 5. Сброс цвета всех клавиш
    public void ResetAllKeyColors(Color resetColor)
    {
        GameObject[] keys = GameObject.FindGameObjectsWithTag("PianoKey");
        foreach (GameObject key in keys)
        {
            Image keyImage = key.GetComponent<Image>();
            if (keyImage != null)
            {
                keyImage.color = resetColor;
            }
        }
    }
    
    private void PlayKeySound(GameObject pressedKey)
    {
        if (!enableSound) return;
        
        AudioSource audioSource = pressedKey?.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.volume = volume;
            audioSource.Play();
        }
    }
    
    private void SetKeyTemporaryColor(GameObject keyObject, Color tempColor)
    {
        Image keyImage = keyObject?.GetComponent<Image>();
        if (keyImage != null)
        {
            keyImage.color = tempColor;
        }
    }
}