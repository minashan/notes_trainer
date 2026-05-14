using UnityEngine;
using UnityEngine.EventSystems;

public class PianoKey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private string noteName;
    
    private PianoInputHandler _inputHandler;
    private GameManager _gameManager;
    private AudioSource audioSource;
    
    private void Start()
    {
        FindManagers();
        audioSource = GetComponent<AudioSource>();
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
    
    public void OnPointerUp(PointerEventData eventData)
    {
        ReleaseKey();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        ReleaseKey();
    }
    
    public void PressKey()
{
    if (_inputHandler == null) return;
    
    if (_gameManager != null)
    {
        // Игровой режим
        _inputHandler.ProcessKeyPress(noteName, gameObject);
        bool isCorrect = _gameManager.CheckNote(noteName);
        _inputHandler.SetKeyFinalColor(gameObject, isCorrect);
        
        if (isCorrect) _gameManager.OnCorrectAnswer();
        else _gameManager.OnIncorrectAnswer(noteName);
    }
    else
    {
        // Режим пианино
        if (audioSource != null && audioSource.clip != null)
        {
            // Останавливаем предыдущий звук этой же ноты
            _inputHandler.NoteOff(audioSource.clip);
            
            _inputHandler.NoteOn(audioSource.clip);
            _inputHandler.SetKeyPressedColor(gameObject);
        }
    }
}
    
    private void ReleaseKey()
    {
        if (_gameManager != null) return;
        
        if (audioSource != null && audioSource.clip != null)
        {
            _inputHandler.NoteOff(audioSource.clip);
            _inputHandler.ResetKeyColor(gameObject);
        }
    }
    
    public string GetNoteName() => noteName;
}