using UnityEngine;
using UnityEngine.UI;

public class PianoKey : MonoBehaviour
{
    public string noteName; // Название ноты (C, D, E...)
    private GameManager gameManager;
    private AudioSource audioSource;
    private Image keyImage;

    void Start()
    {
        // Находим GameManager на сцене
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        keyImage = GetComponent<Image>();
        
        // Настраиваем кнопку
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnKeyPressed);
    }

    public void OnKeyPressed()
    {
        // Проигрываем звук
        if (audioSource != null)
            audioSource.Play();
            
        // Сообщаем GameManager о нажатии
        if (gameManager != null)
            gameManager.OnPianoKeyPressed(noteName, gameObject);
    }
}
