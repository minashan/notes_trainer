using NotesTrainer;
using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private UIManager uiManager;

    [Header("Основные компоненты")]
    [SerializeField] private PianoInputHandler pianoInputHandler;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private NoteController noteContainer;
    [SerializeField] private LevelManager levelManager;
    
    
    private SmartNoteGenerator _smartGenerator;
    
    [Header("Настройки времени")]
    [SerializeField] private float noteDisplayDelay = 0.5f;
    
    private string _currentNote;
    private bool _isWaitingForNextNote = false;
    
    private void Start()
{

int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
    Debug.Log($"GameManager: CurrentLevel из PlayerPrefs = {currentLevel}");
    Debug.Log($"GameManager: HighestLevel = {PlayerPrefs.GetInt("HighestLevel", 0)}");

    Debug.Log($"GameManager: Saved level = {PlayerPrefs.GetInt("HighestLevel", 0)}");
    Debug.Log("GameManager Started");
    
    // 1. Инициализируем компоненты
    FindAndInitializeNoteGenerator();
    ValidateComponents();
    
    // 2. НЕМЕДЛЕННО запускаем уровень
    if (levelManager != null)
    {
        Debug.Log("Starting level from GameManager...");
        levelManager.StartCurrentLevel();
    }
    else
    {
        Debug.LogError("LevelManager not found!");
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
            // Если текущая нота пустая, получаем из SmartNoteGenerator
            if (string.IsNullOrEmpty(_currentNote) && _smartGenerator != null)
            {
                var noteGeneratorType = _smartGenerator.GetType();
                var method = noteGeneratorType.GetMethod("GetLastGeneratedNote");
                
                if (method != null)
                {
                    string lastNote = (string)method.Invoke(_smartGenerator, null);
                    if (!string.IsNullOrEmpty(lastNote))
                    {
                        SetCurrentNote(lastNote);
                    }
                }
            }
        }
    }
    
    private void FindAndInitializeNoteGenerator()
{
    
    // Ищем SmartNoteGenerator
    _smartGenerator = FindAnyObjectByType<SmartNoteGenerator>();
    
    if (_smartGenerator == null)
    {
        Debug.Log("Creating SmartNoteGenerator...");
        GameObject go = new GameObject("SmartNoteGenerator");
        _smartGenerator = go.AddComponent<SmartNoteGenerator>();
    }
    else
    {
        Debug.Log("Found existing SmartNoteGenerator");
    }
    
    InitializeNoteGenerator();
}


    private void InitializeNoteGenerator()
{
    Debug.Log($"GameManager: SmartGenerator = {_smartGenerator != null}");
    Debug.Log($"GameManager: LevelManager = {levelManager != null}");
    
    if (_smartGenerator != null)
    {
        _smartGenerator.OnNoteGenerated += HandleNewNoteGenerated;
        _smartGenerator.OnProgressUpdated += HandleProgressUpdated; 
        
        // Передаём в LevelManager
        if (levelManager != null)
        {
            Debug.Log($"Calling SetSmartNoteGenerator...");
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
        if (_smartGenerator != null)
{
    _smartGenerator.GenerateNote();
}

    }
    
    #region Обработка нот
    
    private void HandleNewNoteGenerated(string newNote)
    {
        SetCurrentNote(newNote);
    }


    private void HandleProgressUpdated(int guessed, int total)
{
    if (uiManager != null)
    {
        uiManager.UpdateProgress(guessed, total);
    }
}


    public void SetCurrentNote(string note)
    {
        _currentNote = note;
        
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
        
        // Применяем позицию контейнера
        noteContainer.transform.localPosition = new Vector3(0, pos.containerY, 0);
        
        // Применяем позицию спрайта ноты
        if (noteContainer.noteSprite != null)
        {
            noteContainer.noteSprite.transform.localPosition = 
                new Vector3(pos.noteSpriteX, pos.noteSpriteY, 0);
        }
        
        // Настраиваем дополнительные линии
        ApplyLedgerLines(pos);
        
        // Настраиваем знаки альтерации
        ApplyAccidental(pos);
        
        // Настраиваем направление штиля
        ApplyStemDirection(noteName);
    }
    
    private void ApplyLedgerLines(NotePosition pos)
    {
        if (noteContainer.ledgerLines == null || noteContainer.ledgerLines.Length < 3) 
            return;
        
        for (int i = 0; i < 3; i++)
        {
            if (noteContainer.ledgerLines[i] != null)
            {
                bool shouldShow = pos.ledgerLinesY[i] != 0f;
                noteContainer.ledgerLines[i].gameObject.SetActive(shouldShow);
                
                if (shouldShow)
                {
                    noteContainer.ledgerLines[i].transform.localPosition = 
                        new Vector3(pos.ledgerLinesX[i], pos.ledgerLinesY[i], 0);
                }
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
            
            // Выбираем спрайт диеза или бемоля
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
        string[] upStemNotes = { 
            "F1", "F1sharp", "G1flat", "G1", "G1sharp",  "A1flat", "A1", "A1sharp", "B1flat", "B1", "B1sharp",
            "Cflat", "C", "Csharp", "Dflat", "D", "Dsharp", "Eflat", "E", "Esharp", "Fflat", "F", "Fsharp",  "Gflat",
            "G", "Gsharp", "Aflat", "A", "Asharp" 
        };
        return Array.Exists(upStemNotes, note => note == noteName);
    }
    
    #endregion
    
    #region Публичные методы для PianoKey
    
    
    /// Проверяет правильность нажатой ноты
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
    
    
    /// Обработка правильного ответа
    public void OnCorrectAnswer()
{
    TryHideLevelInfoOnFirstNote();

    if (_isWaitingForNextNote) return;
    
    _isWaitingForNextNote = true;
    
    
    // Обратная связь через UI
    if (uiManager != null)
    {
        string russianNote = NoteData.Instance.GetTranslatedNoteName(_currentNote);
        uiManager.ShowFeedback(russianNote, true);
    }
    
    // ⭐⭐⭐ ДОБАВЛЯЕМ ОЧКИ В LEVELMANAGER ⭐⭐⭐
    if (levelManager != null)
    {
        levelManager.AddScore(10);
        
        
        // Регистрируем правильный ответ в SmartGenerator
        if (_smartGenerator != null)
        {
            _smartGenerator.RegisterCorrectGuess(_currentNote);
        }
        
        if (levelManager.IsLevelCompleted())
        {
            _isWaitingForNextNote = false;
            return;
        }
    }
    
    Invoke(nameof(GenerateNextNote), noteDisplayDelay);
}
    
    public void OnIncorrectAnswer(string pressedNote)
    {

    TryHideLevelInfoOnFirstNote();
        // Обратная связь через UI
        if (uiManager != null)
        {
            string russianNote = NoteData.Instance.GetTranslatedNoteName(pressedNote);
            uiManager.ShowFeedback(russianNote, false);
        }
    }
    
    #endregion
    
    #region Вспомогательные методы
    
    private void GenerateNextNote()
{
    _isWaitingForNextNote = false;
    
    // Пробуем новый SmartGenerator
    if (_smartGenerator != null)
    {
        _smartGenerator.GenerateNote();
    }
    
}
    
    #endregion
}