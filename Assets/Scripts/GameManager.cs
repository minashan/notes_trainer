using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;



public class GameManager : MonoBehaviour
{

public string[] notes;
    
    
    public TextMeshProUGUI feedbackText;
    public NoteController noteContainer;

            
    private string currentNote;
    private string previousNote = "";


    void Start()
{
    // Проверка NoteData
    if (NoteData.Instance == null)
    {
        Debug.LogError("NoteData не инициализирован!");
        return;
    }

    // Инициализируем массив в Start()
    notes = new string[] { 
        "F1", "F1sharp", "G1flat", "G1", "G1sharp", "A1flat", "A1", "A1sharp", "B1flat", "B1",   
        "C", "Csharp", "D", "Dflat", "Dsharp", "Eflat", "E", "F", "Fsharp", "Gflat", "G", "Gsharp", "Aflat", "A", "Asharp", "Bflat", "B",
        "C2", "C2sharp", "D2flat", "D2", "D2sharp", "E2flat", "E2", "F2", "F2sharp", "G2flat", "G2", "G2sharp", "A2flat", "A2", "A2sharp", "B2flat", "B2",
        "C3", "C3sharp", "D3flat", "D3", "D3sharp", "E3flat", "E3"
    };
    
    GenerateRandomNote();
}


    public void GenerateRandomNote()
{
    string newNote;
    int attempts = 0;
    
    do {
        newNote = notes[Random.Range(0, notes.Length)]; // обычная случайность
        attempts++;
    } while (newNote == previousNote && attempts < 10);
    
    previousNote = newNote;
    currentNote = newNote;
    
    ApplySavedPosition(currentNote);
}


    private void ApplySavedPosition(string noteName)
{
    if (NoteData.Instance.GetNotePosition(noteName) != null && noteContainer != null)
    {
        NotePosition pos = NoteData.Instance.GetNotePosition(noteName);
        
        // Существующий код для ноты и линеек
        noteContainer.transform.localPosition = new Vector3(0, pos.containerY, 0);
        noteContainer.noteSprite.transform.localPosition = new Vector3(pos.noteSpriteX, pos.noteSpriteY, 0);
        
        for (int i = 0; i < 3; i++)
        {
            bool shouldShow = pos.ledgerLinesY[i] != 0f;
            noteContainer.ledgerLines[i].gameObject.SetActive(shouldShow);
            if (shouldShow)
            {
                noteContainer.ledgerLines[i].transform.localPosition = new Vector3(pos.ledgerLinesX[i], pos.ledgerLinesY[i], 0);
            }
        }
        
        // НОВЫЙ КОД ДЛЯ ЗНАКОВ АЛЬТЕРАЦИИ
        if (noteContainer.accidentalSprite != null)
        {
            if (pos.showAccidental)
            {
                // Показываем знак
                noteContainer.accidentalSprite.gameObject.SetActive(true);
                noteContainer.accidentalSprite.transform.localPosition = 
                    new Vector3(pos.accidentalX, pos.accidentalY, 0);
                
                // Выбираем диез или бемоль
                if (pos.isSharp && noteContainer.sharpSprite != null)
                {
                    noteContainer.accidentalSprite.sprite = noteContainer.sharpSprite;
                }
                else if (!pos.isSharp && noteContainer.flatSprite != null)
                {
                    noteContainer.accidentalSprite.sprite = noteContainer.flatSprite;
                }
            }
            else
            {
                // Скрываем знак для белых нот
                noteContainer.accidentalSprite.gameObject.SetActive(false);
            }
        }
        
        // Существующий код для штиля
        bool stemUp = ShouldStemUp(noteName);
        noteContainer.noteSprite.sprite = stemUp ? noteContainer.normalUpSprite : noteContainer.normalDownSprite;
    }
}


    private bool ShouldStemUp(string noteName)
    {
        string[] upStemNotes = { "F1", "F1sharp", "G1flat", "G1", "G1sharp", "A1flat", "A1", "A1sharp", "B1flat", "B1", "C", "Csharp", "D", "Dflat", "Dsharp", "Eflat", "E", "F", "Fsharp", "Gflat", "G", "Gsharp", "Aflat", "A", "Asharp" };
        return upStemNotes.Contains(noteName);
    }


    public void OnPianoKeyPressed(string keyNote, GameObject pressedKey)
{
    // 1. Воспроизведение звука (без изменений)
    AudioSource audioSource = pressedKey.GetComponent<AudioSource>();
    if (audioSource != null) audioSource.Play();

    // 2. Проверка правильности с учётом энгармонизма
    bool isCorrect = false;
    
    // Прямое совпадение
    if (keyNote == currentNote)
    {
        isCorrect = true;
    }
    // Энгармоническое совпадение (используем NoteData)
    else if (NoteData.Instance.AreNotesEnharmonic(keyNote, currentNote))
    {
        isCorrect = true;
    }

    // 3. Визуальная обратная связь
    feedbackText.text = keyNote;
    Image keyImage = pressedKey.GetComponent<Image>();
    
    if (isCorrect)
    {
        feedbackText.color = Color.green;
        keyImage.color = Color.green;
        GenerateRandomNote(); // Генерируем новую ноту только при правильном ответе
    }
    else
    {
        feedbackText.color = Color.red;
        keyImage.color = Color.red;
    }

    // 4. Сброс цвета (без изменений)
    Invoke("ResetKeyColor", 0.5f);
}

    void ResetKeyColor()
    {
        GameObject[] keys = GameObject.FindGameObjectsWithTag("PianoKey");
        foreach (GameObject key in keys)
        {
            Image keyImage = key.GetComponent<Image>();
            if (keyImage != null) keyImage.color = Color.white;
        }
        feedbackText.text = "";
    }
}