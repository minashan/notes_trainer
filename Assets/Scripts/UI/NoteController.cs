using UnityEngine;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
    public Image noteSprite;
    public Sprite normalUpSprite;
    public Sprite normalDownSprite;
    public Image[] ledgerLines;

    // ДОБАВЛЯЕМ ЭТИ ПОЛЯ:
    public Image accidentalSprite;   // Image для знака альтерации
    public Sprite sharpSprite;       // Спрайт диеза
    public Sprite flatSprite;        // Спрайт бемоля
}