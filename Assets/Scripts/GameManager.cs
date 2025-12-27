using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class NotePosition
{
    public float containerY;
    public float noteSpriteY;
    public float noteSpriteX;
    public float[] ledgerLinesY;
    public float[] ledgerLinesX;

    // НОВЫЕ ПОЛЯ ДЛЯ ЗНАКОВ АЛЬТЕРАЦИИ
    public float accidentalX;
    public float accidentalY;
    public bool showAccidental;
    public bool isSharp;
}

public class GameManager : MonoBehaviour
{

public string[] notes;
    
    
    public TextMeshProUGUI feedbackText;
    public NoteController noteContainer;


    private Dictionary<string, string[]> enharmonicEquivalents = new Dictionary<string, string[]>
    {
        { "F1sharp", new string[] { "G1flat" } },
{ "G1sharp", new string[] { "A1flat" } },
{ "A1sharp", new string[] { "B1flat" } },

{ "Csharp",  new string[] { "Dflat" } },
{ "Dsharp",  new string[] { "Eflat" } },
{ "Fsharp",  new string[] { "Gflat" } },
{ "Gsharp",  new string[] { "Aflat" } },
{ "Asharp",  new string[] { "Bflat" } },

// ВТОРАЯ ОКТАВА
{ "C2sharp", new string[] { "D2flat" } },
{ "D2sharp", new string[] { "E2flat" } },
{ "F2sharp", new string[] { "G2flat" } },
{ "G2sharp", new string[] { "A2flat" } },
{ "A2sharp", new string[] { "B2flat" } },

// ТРЕТЬЯ ОКТАВА (если есть диезы)
{ "C3sharp", new string[] { "D3flat" } },
{ "D3sharp", new string[] { "E3flat" } },

// ОБРАТНЫЕ СВЯЗИ (чтобы работало в обе стороны)
// Можно добавить для полноты, хотя ваш код проверяет оба направления
{ "G1flat", new string[] { "F1sharp" } },
{ "A1flat", new string[] { "G1sharp" } },
{ "B1flat", new string[] { "A1sharp" } },

{ "Dflat",  new string[] { "Csharp" } },
{ "Eflat",  new string[] { "Dsharp" } },
{ "Gflat",  new string[] { "Fsharp" } },
{ "Aflat",  new string[] { "Gsharp" } },
{ "Bflat",  new string[] { "Asharp" } }
};



    
    // ФИНАЛЬНЫЕ НАСТРОЙКИ ВСЕХ НОТ
    public Dictionary<string, NotePosition> noteSettings = new Dictionary<string, NotePosition>()
    {
        // МАЛАЯ ОКТАВА (штиль вверх)
        { "F1", new NotePosition() { containerY = -2f, noteSpriteY = 130f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F1sharp", new NotePosition() { containerY = -2f, noteSpriteY = 130f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f },accidentalX = -67f, accidentalY = 90f,showAccidental = true,isSharp = true } },
        { "G1flat", new NotePosition() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 123f, showAccidental = true, isSharp = false } },
        { "G1", new NotePosition() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "G1sharp", new NotePosition() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 103f,showAccidental = true,isSharp = true } },
        { "A1flat", new NotePosition() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 141f, showAccidental = true, isSharp = false } },
        { "A1", new NotePosition() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "A1sharp", new NotePosition() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 123f,showAccidental = true,isSharp = true } },
        { "B1flat", new NotePosition() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 154f, showAccidental = true, isSharp = false } },
        { "B1", new NotePosition() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },

        
        // ПЕРВАЯ ОКТАВА (до A - штиль вверх)
        { "C", new NotePosition() { containerY = -2f, noteSpriteY = 192f, noteSpriteX = 5f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Csharp", new NotePosition() { containerY = -2f, noteSpriteY = 190f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f },accidentalX = -62f,accidentalY = 151f,showAccidental = true,isSharp = true } },
        { "D", new NotePosition() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Dflat", new NotePosition() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 181f, showAccidental = true, isSharp = false } },
        { "Dsharp", new NotePosition() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -54f,accidentalY = 165f,showAccidental = true,isSharp = true } },
        { "Eflat", new NotePosition() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 203f, showAccidental = true, isSharp = false } },
        { "E", new NotePosition() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F", new NotePosition() { containerY = -2f, noteSpriteY = 238f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Fsharp", new NotePosition() { containerY = -2f, noteSpriteY = 238f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -54f,accidentalY = 200f,showAccidental = true,isSharp = true } },
        { "Gflat", new NotePosition() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 237f, showAccidental = true, isSharp = false } },
        { "G", new NotePosition() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Gsharp", new NotePosition() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -54f,accidentalY = 217f,showAccidental = true,isSharp = true } },
        { "Aflat", new NotePosition() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 256f, showAccidental = true, isSharp = false } },
        { "A", new NotePosition() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Asharp", new NotePosition() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -54f,accidentalY = 234f,showAccidental = true,isSharp = true } },
        
        // ОБНОВЛЕННЫЕ НОТЫ СО ШТИЛЕМ ВНИЗ:
        { "Bflat", new NotePosition() { containerY = -52f, noteSpriteY = 265f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 320f, showAccidental = true, isSharp = false } },
        { "B", new NotePosition() { containerY = -52f, noteSpriteY = 265f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "C2", new NotePosition() { containerY = -50.8f, noteSpriteY = 281f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "C2sharp", new NotePosition() { containerY = -50.8f, noteSpriteY = 281f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -50f,accidentalY = 318f,showAccidental = true,isSharp = true } },
        { "D2flat", new NotePosition() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 357f, showAccidental = true, isSharp = false } },
        { "D2", new NotePosition() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "D2sharp", new NotePosition() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -50f,accidentalY = 335f,showAccidental = true,isSharp = true } },
        { "E2flat", new NotePosition() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 372f, showAccidental = true, isSharp = false } },
        { "E2", new NotePosition() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F2", new NotePosition() { containerY = -48.5f, noteSpriteY = 334f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F2sharp", new NotePosition() { containerY = -48.5f, noteSpriteY = 334f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -50f,accidentalY = 367f,showAccidental = true,isSharp = true } },
        { "G2flat", new NotePosition() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -70f, accidentalY = 411f, showAccidental = true, isSharp = false } },
        { "G2", new NotePosition() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "G2sharp", new NotePosition() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -50f,accidentalY = 390f,showAccidental = true,isSharp = true } },
        { "A2flat", new NotePosition() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 89.1f }, ledgerLinesX = new float[] { 0f, 0f, 9f }, accidentalX = -70f, accidentalY = 110f, showAccidental = true, isSharp = false } },
        { "A2", new NotePosition() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 89.1f }, ledgerLinesX = new float[] { 0f, 0f, 9f } } },
        { "A2sharp", new NotePosition() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 1.1f, ledgerLinesY = new float[] { 0f, 0f, 89.1f }, ledgerLinesX = new float[] { 0f, 0f, 9f }, accidentalX = -65f,accidentalY = 87f,showAccidental = true,isSharp = true } },
        { "B2flat", new NotePosition() { containerY = 265.3f, noteSpriteY = 67f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 86.8f }, ledgerLinesX = new float[] { 0f, 0f, 4f }, accidentalX = -70f, accidentalY = 123f, showAccidental = true, isSharp = false } },
        { "B2", new NotePosition() { containerY = 265.3f, noteSpriteY = 67f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 86.8f }, ledgerLinesX = new float[] { 0f, 0f, 4f } } },
        { "C3", new NotePosition() { containerY = 264.1f, noteSpriteY = 87f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "C3sharp", new NotePosition() { containerY = 264.1f, noteSpriteY = 87f, noteSpriteX = -10f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -65f,accidentalY = 120f,showAccidental = true,isSharp = true } },
        { "D3flat", new NotePosition() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 7.3f, 7.3f }, accidentalX = -70f, accidentalY = 158f, showAccidental = true, isSharp = false } },
        { "D3", new NotePosition() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 7.3f, 7.3f } } },
        { "D3sharp", new NotePosition() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 7.3f, 7.3f }, accidentalX = -60f,accidentalY = 138f,showAccidental = true,isSharp = true } },
        { "E3flat", new NotePosition() { containerY = 264.1f, noteSpriteY = 111f, noteSpriteX = 0f, ledgerLinesY = new float[] { 146.9f, 116.9f, 86.6f }, ledgerLinesX = new float[] { 8.7f, 8.7f, 8.7f }, accidentalX = -70f, accidentalY = 168f, showAccidental = true, isSharp = false } },
        { "E3", new NotePosition() { containerY = 264.1f, noteSpriteY = 111f, noteSpriteX = 0f, ledgerLinesY = new float[] { 146.9f, 116.9f, 86.6f }, ledgerLinesX = new float[] { 8.7f, 8.7f, 8.7f } } } };
    


    private string currentNote;
    private string previousNote = "";




    void Start()
{
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
    if (noteSettings.ContainsKey(noteName) && noteContainer != null)
    {
        NotePosition pos = noteSettings[noteName];
        
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
    // Энгармоническое совпадение (диез → бемоль)
    else if (enharmonicEquivalents.ContainsKey(keyNote) 
             && enharmonicEquivalents[keyNote].Contains(currentNote))
    {
        isCorrect = true;
        
    }
    // Энгармоническое совпадение (бемоль → диез)
    else if (enharmonicEquivalents.ContainsKey(currentNote)
             && enharmonicEquivalents[currentNote].Contains(keyNote))
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