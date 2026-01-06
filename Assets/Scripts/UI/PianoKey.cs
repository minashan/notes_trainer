using UnityEngine;
using UnityEngine.EventSystems;

public class PianoKey : MonoBehaviour, IPointerDownHandler
{
    public string noteName;
    
    private PianoInputHandler _inputHandler;
    private GameManager _gameManager;
    
    void Start()
    {
        // Используем новые методы вместо устаревших
        _inputHandler = FindAnyObjectByType<PianoInputHandler>();
        _gameManager = FindAnyObjectByType<GameManager>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        PressKey();
    }
    
    public void PressKey()
    {
        if (_gameManager != null)
        {
            _gameManager.OnPianoKeyPressed(noteName, gameObject);
        }
    }
}