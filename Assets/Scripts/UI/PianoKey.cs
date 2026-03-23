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
        if (_inputHandler == null || _gameManager == null) return;
        
        _inputHandler.ProcessKeyPress(noteName, gameObject);
        
        bool isCorrect = _gameManager.CheckNote(noteName);
        
        _inputHandler.SetKeyFinalColor(gameObject, isCorrect);
        
        if (isCorrect)
        {
            _gameManager.OnCorrectAnswer();
        }
        else
        {
            _gameManager.OnIncorrectAnswer(noteName);
        }
    }
    public string GetNoteName() => noteName;
}