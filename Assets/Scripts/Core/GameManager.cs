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
    
    private NoteGenerator _noteGenerator;
    
    [Header("Настройки времени")]
    [SerializeField] private float noteDisplayDelay = 0.5f;
    
    private string _currentNote;
    private bool _isWaitingForNextNote = false;
    
    private void Start()
    {
        // Инициализируем компоненты
        FindAndInitializeNoteGenerator();
        ValidateComponents();
        
        // Ждем LevelManager
        StartCoroutine(WaitForLevelManager());
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
            // Если текущая нота пустая, получаем из NoteGenerator
            if (string.IsNullOrEmpty(_currentNote) && _noteGenerator != null)
            {
                var noteGeneratorType = _noteGenerator.GetType();
                var method = noteGeneratorType.GetMethod("GetLastGeneratedNote");
                
                if (method != null)
                {
                    string lastNote = (string)method.Invoke(_noteGenerator, null);
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
        // Способ 1: Ищем по имени GameObject
        GameObject generatorObj = GameObject.Find("NoteGenerator");
        if (generatorObj != null)
        {
            _noteGenerator = generatorObj.GetComponent<NoteGenerator>();
        }
        
        // Способ 2: Ищем в сцене любой NoteGenerator
        if (_noteGenerator == null)
        {
            _noteGenerator = FindAnyObjectByType<NoteGenerator>();
        }
        
        // Способ 3: Создаем новый если не нашли
        if (_noteGenerator == null)
        {
            GameObject newGenerator = new GameObject("NoteGenerator");
            _noteGenerator = newGenerator.AddComponent<NoteGenerator>();
        }
        
        // Инициализируем NoteGenerator
        InitializeNoteGenerator();
    }

    private void InitializeNoteGenerator()
    {
        if (_noteGenerator == null) return;
        
        // ПОДПИСЫВАЕМСЯ НА СОБЫТИЕ ГЕНЕРАЦИИ НОТ
        _noteGenerator.OnNoteGenerated += HandleNewNoteGenerated;
    }
    
    private void ValidateComponents()
    {
        if (NoteData.Instance == null) return;
        if (_noteGenerator == null) return;
    } // ← ВОТ ЗДЕСЬ ДОБАВИЛ ЗАКРЫВАЮЩУЮ СКОБКУ!

    private void GenerateFirstNote()
    {
        if (_noteGenerator != null)
        {
            _noteGenerator.GenerateRandomNote();
        }
    }
    
    #region Обработка нот
    
    private void HandleNewNoteGenerated(string newNote)
    {
        SetCurrentNote(newNote);
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
    
    /// <summary>
    /// Проверяет правильность нажатой ноты
    /// </summary>
    public bool CheckNote(string pressedNote)
    {
        if (string.IsNullOrEmpty(_currentNote)) return false;
        
        bool directMatch = pressedNote == _currentNote;
        bool enharmonicMatch = NoteData.Instance.AreNotesEnharmonic(pressedNote, _currentNote);
        
        return directMatch || enharmonicMatch;
    }
    
    /// <summary>
    /// Обработка правильного ответа
    /// </summary>
    public void OnCorrectAnswer()
{
    if (_isWaitingForNextNote) return;
    
    _isWaitingForNextNote = true;
    
    // ... UI код ...
    
    if (levelManager != null)
    {
        levelManager.AddScore(10);
        
        // ПРОВЕРКА: если уровень завершился - не генерируем новую ноту
        if (levelManager.IsLevelCompleted())
        {
            _isWaitingForNextNote = false; // Важно сбросить!
            return; // ← ВЫХОДИМ!
        }
    }
    
    Invoke(nameof(GenerateNextNote), noteDisplayDelay);
}
    
    public void OnIncorrectAnswer(string pressedNote)
    {
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
        if (_noteGenerator != null)
        {
            _noteGenerator.GenerateRandomNote();
        }
    }
    
    #endregion
}