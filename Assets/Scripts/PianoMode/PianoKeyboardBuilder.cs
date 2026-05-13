using UnityEngine;
using UnityEngine.UI;

public class PianoKeyboardBuilder : MonoBehaviour
{
    [Header("Префабы")]
    [SerializeField] private GameObject whiteKeyPrefab;
    [SerializeField] private GameObject blackKeyPrefab;
    
    [Header("Контейнеры")]
    [SerializeField] private Transform whiteContent;
    [SerializeField] private Transform blackContent;
    
    [Header("Настройки размеров")]
    [SerializeField] private float whiteKeyWidth = 80f;
    [SerializeField] private float whiteKeyHeight = 200f;
    [SerializeField] private float blackKeyWidth = 50f;
    [SerializeField] private float blackKeyHeight = 120f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] whiteKeySounds;  // 52 звука для белых
    [SerializeField] private AudioClip[] blackKeySounds; // 36 звуков для чёрных

    [Header("Audio")]
    [SerializeField] private PianoSoundDatabase soundDatabase;

    private float currentScale = 1f;
    private float minScale = 0.5f;
    private float maxScale = 1.5f;
    private float zoomStep = 0.1f;


    public void ZoomIn()
{
    currentScale = Mathf.Clamp(currentScale + zoomStep, minScale, maxScale);
    whiteContent.localScale = Vector3.one * currentScale;
    blackContent.localScale = Vector3.one * currentScale;
}

public void ZoomOut()
{
    currentScale = Mathf.Clamp(currentScale - zoomStep, minScale, maxScale);
    whiteContent.localScale = Vector3.one * currentScale;
    blackContent.localScale = Vector3.one * currentScale;
}




   void SetNoteSound(GameObject key, string noteName)
{
    Debug.Log($"SetNoteSound for {noteName}");
    
    AudioSource audioSource = key.GetComponent<AudioSource>();
    if (audioSource == null)
    {
        Debug.LogWarning($"No AudioSource on {key.name}");
        return;
    }
    
    if (soundDatabase != null)
    {
        AudioClip clip = soundDatabase.GetClip(noteName);
        Debug.Log($"Clip for {noteName}: {clip?.name ?? "NULL"}");
        
        if (clip != null)
        {
            audioSource.clip = clip;
        }
    }
    else
    {
        Debug.LogWarning("soundDatabase is null!");
    }
}

    
    private string[] allNotes = new string[]
    {
        "A__", "A__sharp", "B__",
        "C_", "C_sharp", "D_", "D_sharp", "E_", "F_", "F_sharp", "G_", "G_sharp", "A_", "A_sharp", "B_",
        "C0", "C0sharp", "D0", "D0sharp", "E0", "F0", "F0sharp", "G0", "G0sharp", "A0", "A0sharp", "B0",
        "C1", "C1sharp", "D1", "D1sharp", "E1", "F1", "F1sharp", "G1", "G1sharp", "A1", "A1sharp", "B1",
        "C", "Csharp", "D", "Dsharp", "E", "F", "Fsharp", "G", "Gsharp", "A", "Asharp", "B",
        "C2", "C2sharp", "D2", "D2sharp", "E2", "F2", "F2sharp", "G2", "G2sharp", "A2", "A2sharp", "B2",
        "C3", "C3sharp", "D3", "D3sharp", "E3", "F3", "F3sharp", "G3", "G3sharp", "A3", "A3sharp", "B3",
        "C4", "C4sharp", "D4", "D4sharp", "E4", "F4", "F4sharp", "G4", "G4sharp", "A4", "A4sharp", "B4",
        "C5"
    };
    
    void Start()
    {
        BuildKeyboard();
    }
    
    void BuildKeyboard()
{
    int whiteIndex = 0;
    
    for (int i = 0; i < allNotes.Length; i++)
    {
        string noteName = allNotes[i];
        bool isBlack = noteName.Contains("sharp") || noteName.Contains("flat");
        
        if (isBlack)
        {
            GameObject blackKey = Instantiate(blackKeyPrefab, blackContent);
            
            float xPos = (whiteIndex - 0.5f) * whiteKeyWidth;
            RectTransform rect = blackKey.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(xPos, 0);
            rect.sizeDelta = new Vector2(blackKeyWidth, blackKeyHeight);
            
            SetNoteName(blackKey, noteName);
            SetNoteSound(blackKey, noteName);  // ← ВЫЗОВ ДЛЯ ЧЁРНОЙ
        }
        else
        {
            GameObject whiteKey = Instantiate(whiteKeyPrefab, whiteContent);
            
            RectTransform rect = whiteKey.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(whiteIndex * whiteKeyWidth, 0);
            rect.sizeDelta = new Vector2(whiteKeyWidth, whiteKeyHeight);
            
            SetNoteName(whiteKey, noteName);
            SetNoteSound(whiteKey, noteName);  // ← ВЫЗОВ ДЛЯ БЕЛОЙ
            
            whiteIndex++;
        }
    }
}
    
    void SetNoteName(GameObject key, string noteName)
    {
        PianoKey pianoKey = key.GetComponent<PianoKey>();
        if (pianoKey != null)
        {
            var field = typeof(PianoKey).GetField("noteName", 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance);
            field.SetValue(pianoKey, noteName);
        }
    }
}