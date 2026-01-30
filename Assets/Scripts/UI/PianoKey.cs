using UnityEngine;
using UnityEngine.EventSystems;

public class PianoKey : MonoBehaviour, IPointerDownHandler
{
    public string noteName;
    
    // Используем полные имена компонентов
    private MonoBehaviour _inputHandler;
    private MonoBehaviour _gameManager;
    
    void Start()
    {
        // Ищем по имени класса без namespace
        _inputHandler = FindAnyObjectByType(typeof(PianoInputHandler)) as PianoInputHandler;
        _gameManager = FindAnyObjectByType(typeof(GameManager)) as GameManager;
        
        if (_inputHandler == null)
            Debug.LogError("PianoInputHandler not found! Check namespace.");
        if (_gameManager == null)
            Debug.LogError("GameManager not found! Check namespace.");
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        PressKey();
    }
    
    public void PressKey()
    {
        if (_inputHandler == null || _gameManager == null) 
        {
            Debug.LogError("Managers not found!");
            return;
        }
        
        // Приводим типы
        PianoInputHandler inputHandler = _inputHandler as PianoInputHandler;
        GameManager gameManager = _gameManager as GameManager;
        
        if (inputHandler == null || gameManager == null)
        {
            Debug.LogError("Failed to cast managers!");
            return;
        }
        
     
        
        // 1. Обрабатываем звук и временную подсветку
        inputHandler.ProcessKeyPress(noteName, gameObject);
        
        // 2. Проверяем правильность
        bool isCorrect = gameManager.CheckNote(noteName);
        
        // 3. Устанавливаем финальный цвет
        inputHandler.SetKeyFinalColor(gameObject, isCorrect);
        
        // 4. Уведомляем GameManager
        if (isCorrect)
        {
            gameManager.OnCorrectAnswer();
        }
        else
        {
            gameManager.OnIncorrectAnswer(noteName);
        }
    }
}