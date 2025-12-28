using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
    public NoteController noteContainer;
    
    private NoteGenerator noteGenerator;
    private string currentNote;
    private Image keyImage;
    
    private float correctFeedbackDuration = 1.5f;
    private float incorrectFeedbackDuration = 1.5f;
    
    private Color darkGreenColor = new Color(0f, 0.5f, 0f, 1f);
    private Color redColor = Color.red;
    private Color whiteColor = Color.white;

    void Start()
    {
        if (NoteData.Instance == null)
        {
            return;
        }
        
        noteGenerator = GetComponent<NoteGenerator>();
        if (noteGenerator == null)
        {
            noteGenerator = FindAnyObjectByType<NoteGenerator>();
            if (noteGenerator == null)
            {
                noteGenerator = gameObject.AddComponent<NoteGenerator>();
            }
        }
        
        if (noteGenerator != null)
        {
            noteGenerator.OnNoteGenerated += HandleNewNoteGenerated;
        }
        else
        {
            return;
        }
        
        string[] allNotes = new string[] { 
            "F1", "F1sharp", "G1", "G1sharp", "A1", "A1sharp", "B1",
            "C", "Csharp", "D", "Dsharp", "E", "F", "Fsharp", "G", "Gsharp", "A", "Asharp", "B",
            "C2", "C2sharp", "D2", "D2sharp", "E2", "F2", "F2sharp", "G2", "G2sharp", "A2", "A2sharp", "B2",
            "C3", "C3sharp", "D3", "D3sharp", "E3"
        };
        
        noteGenerator.Initialize(allNotes);
        noteGenerator.GenerateRandomNote();
        
        if (noteGenerator.GetCurrentNote() != null)
        {
            ApplySavedPosition(noteGenerator.GetCurrentNote());
        }
    }

    private void HandleNewNoteGenerated(string newNote)
    {
        currentNote = newNote;
        ApplySavedPosition(newNote);
    }

    private void ApplySavedPosition(string noteName)
    {
        if (noteContainer == null)
        {
            return;
        }
        
        if (NoteData.Instance.GetNotePosition(noteName) != null)
        {
            NotePosition pos = NoteData.Instance.GetNotePosition(noteName);
            
            noteContainer.transform.localPosition = new Vector3(0, pos.containerY, 0);
            
            if (noteContainer.noteSprite != null)
            {
                noteContainer.noteSprite.transform.localPosition = new Vector3(pos.noteSpriteX, pos.noteSpriteY, 0);
            }
            
            if (noteContainer.ledgerLines != null && noteContainer.ledgerLines.Length >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    bool shouldShow = pos.ledgerLinesY[i] != 0f;
                    if (noteContainer.ledgerLines[i] != null)
                    {
                        noteContainer.ledgerLines[i].gameObject.SetActive(shouldShow);
                        if (shouldShow)
                        {
                            noteContainer.ledgerLines[i].transform.localPosition = new Vector3(pos.ledgerLinesX[i], pos.ledgerLinesY[i], 0);
                        }
                    }
                }
            }
            
            if (noteContainer.accidentalSprite != null)
            {
                if (pos.showAccidental)
                {
                    noteContainer.accidentalSprite.gameObject.SetActive(true);
                    noteContainer.accidentalSprite.transform.localPosition = 
                        new Vector3(pos.accidentalX, pos.accidentalY, 0);
                    
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
                    noteContainer.accidentalSprite.gameObject.SetActive(false);
                }
            }
            
            if (noteContainer.noteSprite != null)
            {
                bool stemUp = ShouldStemUp(noteName);
                if (noteContainer.normalUpSprite != null && noteContainer.normalDownSprite != null)
                {
                    noteContainer.noteSprite.sprite = stemUp ? noteContainer.normalUpSprite : noteContainer.normalDownSprite;
                }
            }
        }
    }

    private bool ShouldStemUp(string noteName)
    {
        string[] upStemNotes = { 
            "F1", "F1sharp", "G1", "G1sharp", "A1", "A1sharp", "B1",
            "C", "Csharp", "D", "Dsharp", "E", "F", "Fsharp", 
            "G", "Gsharp", "A", "Asharp" 
        };
        return upStemNotes.Contains(noteName);
    }

    void ClearFeedbackText()
    {
        if (feedbackText != null)
            feedbackText.text = "";
    }

    void ResetKeyColorWithoutText()
    {
        GameObject[] keys = GameObject.FindGameObjectsWithTag("PianoKey");
        foreach (GameObject key in keys)
        {
            Image keyImage = key.GetComponent<Image>();
            if (keyImage != null) keyImage.color = whiteColor;
        }
    }

    void ResetKeyColor()
    {
        GameObject[] keys = GameObject.FindGameObjectsWithTag("PianoKey");
        foreach (GameObject key in keys)
        {
            Image keyImage = key.GetComponent<Image>();
            if (keyImage != null) keyImage.color = whiteColor;
        }
        
        if (feedbackText != null)
            feedbackText.text = "";
    }

    public void OnPianoKeyPressed(string keyNote, GameObject pressedKey)
    {
        if (noteGenerator == null)
        {
            return;
        }
        
        if (pressedKey != null)
        {
            Image pressedKeyImage = pressedKey.GetComponent<Image>();
            if (pressedKeyImage != null)
            {
                keyImage = pressedKeyImage;
            }
        }
        
        AudioSource audioSource = pressedKey?.GetComponent<AudioSource>();
        if (audioSource != null) audioSource.Play();

        string currentEnglishNote = noteGenerator.GetCurrentNote();
        if (string.IsNullOrEmpty(currentEnglishNote))
        {
            return;
        }
        
        bool isCorrect = false;
        
        if (keyNote == currentEnglishNote)
        {
            isCorrect = true;
        }
        else if (NoteData.Instance.AreNotesEnharmonic(keyNote, currentEnglishNote))
        {
            isCorrect = true;
        }

        if (isCorrect)
        {
            string russianNote = NoteData.Instance.GetTranslatedNoteName(currentEnglishNote);
            if (feedbackText != null)
            {
                feedbackText.text = russianNote;
                feedbackText.color = darkGreenColor;
            }
            
            if (keyImage != null) 
                keyImage.color = Color.green;
            
            Invoke("ClearFeedbackText", correctFeedbackDuration);
            Invoke("ResetKeyColorWithoutText", correctFeedbackDuration);
            
            noteGenerator.GenerateRandomNote();
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = $"Неправильно";
                feedbackText.color = redColor;
            }
            
            if (keyImage != null) 
                keyImage.color = redColor;
                
            Invoke("ResetKeyColor", incorrectFeedbackDuration);
        }
    }
}