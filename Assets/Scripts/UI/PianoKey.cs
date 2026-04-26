using UnityEngine;
using UnityEngine.EventSystems;

public class PianoKey : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private string noteName;
    
    private PianoInputHandler _inputHandler;
    private GameManager _gameManager;
    
    
    private void Start()
    {
        FindManagers();
    }
    
    private void FindManagers()
    {
        _inputHandler = FindFirstObjectByType<PianoInputHandler>();
        _gameManager = FindFirstObjectByType<GameManager>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        PressKey();
    }
    
public void PressKey()
{
    Debug.Log($"PressKey called for {noteName}");
    
    // Если нет GameManager, всё равно пытаемся вызвать ProcessKeyPress
    if (_inputHandler == null)
    {
        Debug.LogWarning("No InputHandler!");
        return;
    }
    
    _inputHandler.ProcessKeyPress(noteName, gameObject);
    
    // Остальной код только если есть GameManager
    if (_gameManager != null)
    {
        bool isCorrect = _gameManager.CheckNote(noteName);
        _inputHandler.SetKeyFinalColor(gameObject, isCorrect);
        
        if (isCorrect) _gameManager.OnCorrectAnswer();
        else _gameManager.OnIncorrectAnswer(noteName);
    }
    else
    {
        // Режим пианино: просто подсветка зелёным
        _inputHandler.SetKeyFinalColor(gameObject, true);
    }
}

    public string GetNoteName() => noteName;
}