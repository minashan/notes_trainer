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
}

public class GameManager : MonoBehaviour
{
    public string[] notes = { 
        "F1", "G1", "A1", "B1",   
        "C", "D", "E", "F", "G", "A", "B",
        "C2", "D2", "E2", "F2", "G2", "A2", "B2",
        "C3", "D3", "E3"
    };
    
    public TextMeshProUGUI feedbackText;
    public NoteController noteContainer;
    
    // ФИНАЛЬНЫЕ НАСТРОЙКИ ВСЕХ НОТ
    public Dictionary<string, NotePosition> noteSettings = new Dictionary<string, NotePosition>()
    {
        // МАЛАЯ ОКТАВА (штиль вверх)
        { "F1", new NotePosition() { containerY = -2f, noteSpriteY = 116f, noteSpriteX = 8.4f, ledgerLinesY = new float[] { 153f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "G1", new NotePosition() { containerY = -2f, noteSpriteY = 130.8f, noteSpriteX = 8.4f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "A1", new NotePosition() { containerY = -2f, noteSpriteY = 147.6f, noteSpriteX = 8.4f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "B1", new NotePosition() { containerY = -2f, noteSpriteY = 161.8f, noteSpriteX = 8.4f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        
        // ПЕРВАЯ ОКТАВА (до A - штиль вверх)
        { "C", new NotePosition() { containerY = -2f, noteSpriteY = 178f, noteSpriteX = 8.4f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "D", new NotePosition() { containerY = -2f, noteSpriteY = 190.14f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "E", new NotePosition() { containerY = -2f, noteSpriteY = 207.2f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F", new NotePosition() { containerY = -2f, noteSpriteY = 224.9f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "G", new NotePosition() { containerY = -2f, noteSpriteY = 242.4f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "A", new NotePosition() { containerY = -2f, noteSpriteY = 259.2f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        
        // ОБНОВЛЕННЫЕ НОТЫ СО ШТИЛЕМ ВНИЗ:
        { "B", new NotePosition() { containerY = -52f, noteSpriteY = 279.3f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "C2", new NotePosition() { containerY = -50.8f, noteSpriteY = 295f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "D2", new NotePosition() { containerY = -51.6f, noteSpriteY = 313.7f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "E2", new NotePosition() { containerY = -49.6f, noteSpriteY = 329.7f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F2", new NotePosition() { containerY = -48.5f, noteSpriteY = 348.6f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "G2", new NotePosition() { containerY = -51.9f, noteSpriteY = 364.9f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "A2", new NotePosition() { containerY = 264.1f, noteSpriteY = 66.7f, noteSpriteX = 1.1f, ledgerLinesY = new float[] { 0f, 0f, 89.1f }, ledgerLinesX = new float[] { 0f, 0f, 9f } } },
        { "B2", new NotePosition() { containerY = 265.3f, noteSpriteY = 81.9f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 86.8f }, ledgerLinesX = new float[] { 0f, 0f, 4f } } },
        { "C3", new NotePosition() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = -10f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "D3", new NotePosition() { containerY = 264.1f, noteSpriteY = 115.4f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 7.3f, 7.3f } } },
        { "E3", new NotePosition() { containerY = 264.1f, noteSpriteY = 125.6f, noteSpriteX = 0f, ledgerLinesY = new float[] { 146.9f, 116.9f, 86.6f }, ledgerLinesX = new float[] { 8.7f, 8.7f, 8.7f } } }
    };
    
    private string currentNote;
    private string previousNote = "";

    void Start()
    {
        GenerateRandomNote();
    }

    public void GenerateRandomNote()
    {
        string newNote;
        int attempts = 0;
        
        do {
            newNote = notes[Random.Range(0, notes.Length)];
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
            
            bool stemUp = ShouldStemUp(noteName);
            noteContainer.noteSprite.sprite = stemUp ? noteContainer.normalUpSprite : noteContainer.normalDownSprite;
        }
    }

    private bool ShouldStemUp(string noteName)
    {
        string[] upStemNotes = { "F1", "G1", "A1", "B1", "C", "D", "E", "F", "G", "A" };
        return upStemNotes.Contains(noteName);
    }

    public void OnPianoKeyPressed(string keyNote, GameObject pressedKey)
    {
        AudioSource audioSource = pressedKey.GetComponent<AudioSource>();
        if (audioSource != null) audioSource.Play();

        feedbackText.text = keyNote;
        Image keyImage = pressedKey.GetComponent<Image>();

        if (keyNote == currentNote)
        {
            feedbackText.color = Color.green;
            keyImage.color = Color.green;
            GenerateRandomNote();
        }
        else
        {
            feedbackText.color = Color.red;
            keyImage.color = Color.red;
        }

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