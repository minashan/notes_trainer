using UnityEngine;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
    [Header("Note Sprite")]
    public Image noteSprite;
    public Sprite normalUpSprite;
    public Sprite normalDownSprite;
    
    [Header("Ledger Lines")]
    public Image[] ledgerLines;
    
    [Header("Accidentals")]
    public Image accidentalSprite;
    public Sprite sharpSprite;
    public Sprite flatSprite;
}