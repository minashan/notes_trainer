using NotesTrainer;
using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private UIManager uiManager;
    
    [Header("Game Components")]
    [SerializeField] private PianoInputHandler pianoInputHandler;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private NoteController noteContainer;
    [SerializeField] private LevelManager levelManager;
    
    [Header("Settings")]
    [SerializeField] private float noteDisplayDelay = 0.5f;
    
    private SmartNoteGenerator _smartGenerator;
    private string _currentNote;
    private bool _isWaitingForNextNote;
    private int _wrongAttempts;
    private bool _hintGiven;
    
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    
    private void Start()
    {

        ClefType selectedClef = SceneNavigator.Instance.LoadSelectedClef();
        NoteData.Instance.SetCurrentClef(selectedClef);
        Debug.Log($"GameManager: clef set to {selectedClef}");

        FindAndInitializeNoteGenerator();
        ValidateComponents();
        
        if (levelManager != null)
        {
            levelManager.StartCurrentLevel();
        }
    }
    
    private IEnumerator WaitForLevelManager()
    {
        yield return new WaitForSeconds(0.2f);
        
        if (levelManager == null)
        {
            GenerateFirstNote();
        }
        else
        {
            if (string.IsNullOrEmpty(_currentNote) && _smartGenerator != null)
            {
                var method = _smartGenerator.GetType().GetMethod("GetLastGeneratedNote");
                string lastNote = method?.Invoke(_smartGenerator, null) as string;
                
                if (!string.IsNullOrEmpty(lastNote))
                {
                    SetCurrentNote(lastNote);
                }
            }
        }
    }
    
    private void FindAndInitializeNoteGenerator()
    {
        _smartGenerator = FindAnyObjectByType<SmartNoteGenerator>();
        
        if (_smartGenerator == null)
        {
            GameObject go = new GameObject("SmartNoteGenerator");
            _smartGenerator = go.AddComponent<SmartNoteGenerator>();
        }
        
        InitializeNoteGenerator();
    }
    
    private void InitializeNoteGenerator()
    {
        if (_smartGenerator != null)
        {
            _smartGenerator.OnNoteGenerated += HandleNewNoteGenerated;
            _smartGenerator.OnProgressUpdated += HandleProgressUpdated;
            
            if (levelManager != null)
            {
                levelManager.SetSmartNoteGenerator(_smartGenerator);
            }
        }
    }
    
    private void ValidateComponents()
    {
        if (NoteData.Instance == null) return;
        if (_smartGenerator == null) return;
    }
    
    private void GenerateFirstNote()
    {
        _smartGenerator?.GenerateNote();
    }
    
    #region Note Handling
    
    private void HandleNewNoteGenerated(string newNote)
    {
        SetCurrentNote(newNote);
    }
    
    private void HandleProgressUpdated(int guessed, int total)
    {
        uiManager?.UpdateProgress(guessed, total);
    }
    
    public void SetCurrentNote(string note)
{
    _currentNote = note;
    Debug.Log($"GameManager: Setting note {note} for clef {NoteData.Instance.GetCurrentClef()}");
    
    if (noteContainer != null)
    {
        ApplySavedPosition(note);
    }
}
    
    private void ApplySavedPosition(string noteName)
    {
        if (noteContainer == null) return;
        
        NotePosition pos = NoteData.Instance.GetNotePosition(noteName);
        if (pos == null) return;
        
        noteContainer.transform.localPosition = new Vector3(0, pos.containerY, 0);
        
        if (noteContainer.noteSprite != null)
        {
            noteContainer.noteSprite.transform.localPosition = 
                new Vector3(pos.noteSpriteX, pos.noteSpriteY, 0);
        }
        
        ApplyLedgerLines(pos);
        ApplyAccidental(pos);
        ApplyStemDirection(noteName);
    }
    
    private void ApplyLedgerLines(NotePosition pos)
    {
        if (noteContainer.ledgerLines == null || noteContainer.ledgerLines.Length < 3) return;
        
        for (int i = 0; i < 3; i++)
        {
            if (noteContainer.ledgerLines[i] == null) continue;
            
            bool shouldShow = pos.ledgerLinesY[i] != 0f;
            noteContainer.ledgerLines[i].gameObject.SetActive(shouldShow);
            
            if (shouldShow)
            {
                noteContainer.ledgerLines[i].transform.localPosition = 
                    new Vector3(pos.ledgerLinesX[i], pos.ledgerLinesY[i], 0);
            }
        }
    }
    
    private void ApplyAccidental(NotePosition pos)
    {
        if (noteContainer.accidentalSprite == null) return;
        
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
    
    private void ApplyStemDirection(string noteName)
    {
        if (noteContainer.noteSprite == null) return;
        if (noteContainer.normalUpSprite == null || noteContainer.normalDownSprite == null) return;
        
        bool stemUp = ShouldStemUp(noteName);
        noteContainer.noteSprite.sprite = stemUp ? 
            noteContainer.normalUpSprite : noteContainer.normalDownSprite;
    }
    
    private bool ShouldStemUp(string noteName)
    {
        string[] upStemNotes = 
        { 
            "F1", "F1sharp", "G1flat", "G1", "G1sharp", "A1flat", "A1", "A1sharp", 
            "B1flat", "B1", "B1sharp", "Cflat", "C", "Csharp", "Dflat", "D", "Dsharp", 
            "Eflat", "E", "Esharp", "Fflat", "F", "Fsharp", "Gflat", "G", "Gsharp", 
            "Aflat", "A", "Asharp", "A_", "A_sharp", "B_flat", "B_", "B_sharp", "C0flat",
            "C0", "C0sharp", "D0flat", "D0", "D0sharp", "E0flat", "E0", "E0sharp", "F0flat", 
            "F0", "F0sharp", "G0flat", "G0", "G0sharp", "A0flat", "A0", "A0sharp", "B0flat", 
            "B0", "B0sharp", "C1flat", "C1", "C1sharp",
        };
        return Array.Exists(upStemNotes, note => note == noteName);
    }
    
    #endregion
    
    #region Public Methods
    
    public bool CheckNote(string pressedNote)
    {
        if (string.IsNullOrEmpty(_currentNote)) return false;
        
        bool directMatch = pressedNote == _currentNote;
        bool enharmonicMatch = NoteData.Instance.AreNotesEnharmonic(pressedNote, _currentNote);
        
        return directMatch || enharmonicMatch;
    }
    
    private void TryHideLevelInfoOnFirstNote()
    {
        if (levelManager != null && levelManager.IsFirstNoteInLevel())
        {
            levelManager.HideLevelInfo();
        }
    }
    
    public void OnCorrectAnswer()
{
    _wrongAttempts = 0;
    _hintGiven = false;
    
    HintButton hintButton = FindFirstObjectByType<HintButton>();
    hintButton?.StopPulsing();
    
    if (_isWaitingForNextNote) return;
    
    _isWaitingForNextNote = true;
    
    if (uiManager != null)
    {
        string russianNote = NoteData.Instance.GetTranslatedNoteName(_currentNote);
        uiManager.ShowFeedback(russianNote, true);
    }
    
    if (levelManager != null)
    {
        levelManager.AddScore(10);
        _smartGenerator?.RegisterCorrectGuess(_currentNote);
        
        // Сброс цвета с задержкой
        Invoke(nameof(ResetKeyColorAfterDelay), 1f);
        
        if (levelManager.IsLevelCompleted())
        {
            _isWaitingForNextNote = false;
            return;
        }
    }
    
    Invoke(nameof(GenerateNextNote), noteDisplayDelay);
}

private void ResetKeyColorAfterDelay()
{
    if (pianoInputHandler != null)
    {
        GameObject correctKey = pianoInputHandler.FindKeyByNote(_currentNote);
        if (correctKey != null)
        {
            pianoInputHandler.ResetKeyColor(correctKey);
        }
    }
}


    public void OnIncorrectAnswer(string pressedNote)
{
    _wrongAttempts++;
    
   if (_wrongAttempts >= 3 && !_hintGiven)
{
    if (pianoInputHandler != null)
    {
        pianoInputHandler.HighlightHintKey(_currentNote); // тусклый
    }
    
    HintButton hintButton = FindFirstObjectByType<HintButton>();
    hintButton?.StartPulsing();
    
    _hintGiven = true;
}
    
    if (uiManager != null)
    {
        string russianNote = NoteData.Instance.GetTranslatedNoteName(pressedNote);
        uiManager.ShowFeedback(russianNote, false);
    }
}

    
    public string GetCurrentNoteName()
    {
        return _currentNote;
    }
    
    #endregion
    
    #region Helper Methods
    
    private void GenerateNextNote()
    {
        _isWaitingForNextNote = false;
        _smartGenerator?.GenerateNote();
    }
    
    #endregion
}