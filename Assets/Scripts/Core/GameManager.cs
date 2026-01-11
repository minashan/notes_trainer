using UnityEngine;
using TMPro;
using System;



    public class GameManager : MonoBehaviour
    {

        [Header("UI Components")]
        [SerializeField] private UIManager uiManager;  // ← Добавляем эту строку

        [Header("Основные компоненты")]
        [SerializeField] private PianoInputHandler pianoInputHandler;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private NoteController noteContainer;
        
        // NoteGenerator будем искать по имени GameObject
        private NoteGenerator _noteGenerator;
        
        [Header("Настройки времени")]
        [SerializeField] private float noteDisplayDelay = 0.5f;
        
        
        
        // Состояние игры
        private string _currentNote;
        private bool _isWaitingForNextNote = false;
        
        #region Инициализация
        
        void Start()
        {
            
            // 1. Находим NoteGenerator
            FindAndInitializeNoteGenerator();
            
            // 2. Проверяем остальные компоненты
            ValidateComponents();
            
            // 3. Генерируем первую ноту
            GenerateFirstNote();
        }
        
        private void FindAndInitializeNoteGenerator()
        {
            // Способ 1: Ищем по имени GameObject
            GameObject generatorObj = GameObject.Find("NoteGenerator");
            if (generatorObj != null)
            {
                _noteGenerator = generatorObj.GetComponent<NoteGenerator>();
                if (_noteGenerator != null)
                {
                    Debug.Log("NoteGenerator found by GameObject name");
                }
            }
            
            // Способ 2: Ищем в сцене любой NoteGenerator
            if (_noteGenerator == null)
            {
                _noteGenerator = FindAnyObjectByType<NoteGenerator>();
                if (_noteGenerator != null)
                {
                    Debug.Log("NoteGenerator found in scene");
                }
            }
            
            // Способ 3: Создаем новый если не нашли
            if (_noteGenerator == null)
            {
                Debug.LogWarning("NoteGenerator not found. Creating new one...");
                GameObject newGenerator = new GameObject("NoteGenerator");
                _noteGenerator = newGenerator.AddComponent<NoteGenerator>();
            }
            
            // Инициализируем NoteGenerator
            InitializeNoteGenerator();
        }
        
        private void InitializeNoteGenerator()
        {
            if (_noteGenerator == null)
            {
                Debug.LogError("NoteGenerator is null after search!");
                return;
            }
            
            string[] allNotes = new string[] { 
    // Малая октава (F1 - B1)
    "F1", "F1sharp", "G1flat", "G1", "G1sharp", "A1flat", "A1", "A1sharp", "B1flat", "B1",
    
    // Первая октава (C - B)
    "C", "Csharp", "Dflat", "D", "Dsharp", "Eflat", "E", "F", "Fsharp", "Gflat", "G", "Gsharp", "Aflat", "A", "Asharp", "Bflat", "B",
    
    // Вторая октава (C2 - B2)
    "C2", "C2sharp", "D2flat", "D2", "D2sharp", "E2flat", "E2", "F2", "F2sharp", "G2flat", "G2", "G2sharp", "A2flat", "A2", "A2sharp", "B2flat", "B2",
    
    // Третья октава (C3 - E3)
    "C3", "C3sharp", "D3flat", "D3", "D3sharp", "E3flat", "E3"
};
            
            _noteGenerator.Initialize(allNotes);
            _noteGenerator.OnNoteGenerated += HandleNewNoteGenerated;
            
            Debug.Log("NoteGenerator initialized successfully");
        }
        
        private void ValidateComponents()
        {
            if (NoteData.Instance == null)
            {
                Debug.LogError("NoteData.Instance is null!");
                return;
            }
            
            if (_noteGenerator == null)
            {
                Debug.LogError("NoteGenerator is still null!");
                return;
            }
            
            // Предупреждения для остальных компонентов
            if (pianoInputHandler == null)
                Debug.LogWarning("PianoInputHandler not assigned. Sound and colors may not work.");
            
            if (noteContainer == null)
                Debug.LogWarning("NoteContainer (NoteController) not assigned!");
            
            if (feedbackText == null)
                Debug.LogWarning("FeedbackText not assigned!");

            if (uiManager == null)
                Debug.LogWarning("UIManager not assigned. UI features may not work.");
        }

        
        
        private void GenerateFirstNote()
        {
            if (_noteGenerator != null)
            {
                _noteGenerator.GenerateRandomNote();
                Debug.Log("First note generated");
            }
        }
        
        #endregion
        
        #region Обработка нот
        
        private void HandleNewNoteGenerated(string newNote)
        {
            _currentNote = newNote;
            Debug.Log($"New note generated: {newNote}");
            ApplySavedPosition(newNote);
        }
        
        private void ApplySavedPosition(string noteName)
        {
            if (noteContainer == null) 
            {
                Debug.LogWarning("NoteContainer is null, cannot apply position");
                return;
            }
            
            NotePosition pos = NoteData.Instance.GetNotePosition(noteName);
            if (pos == null) 
            {
                Debug.LogWarning($"No NotePosition found for {noteName}");
                return;
            }
            
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
            
            Debug.Log($"Applied position for note: {noteName}");
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
                "F1", "F1sharp", "G1flat", "G1", "G1sharp",  "A1flat", "A1", "A1sharp", "B1flat", "B1",
                "C", "Csharp", "Dflat", "D", "Dsharp", "Eflat", "E", "F", "Fsharp",  "Gflat",
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
            if (string.IsNullOrEmpty(_currentNote)) 
            {
                Debug.LogWarning("Current note is empty!");
                return false;
            }
            
            bool directMatch = pressedNote == _currentNote;
            bool enharmonicMatch = NoteData.Instance.AreNotesEnharmonic(pressedNote, _currentNote);
            
            Debug.Log($"CheckNote: pressed={pressedNote}, current={_currentNote}, " +
                     $"direct={directMatch}, enharmonic={enharmonicMatch}");
            
            return directMatch || enharmonicMatch;
        }
        
        /// <summary>
        /// Обработка правильного ответа
        /// </summary>
       public void OnCorrectAnswer()
{
    if (_isWaitingForNextNote) return;
    
    Debug.Log("Correct answer!");
    _isWaitingForNextNote = true;
    
    // СТАЛО (заменяем ShowFeedback на uiManager):
    if (uiManager != null)
    {
        string russianNote = NoteData.Instance.GetTranslatedNoteName(_currentNote);
        uiManager.ShowFeedback(russianNote, true); // true = правильный
    }
    
    // УДАЛЯЕМ эту строку (uiManager сам очистит):
    // Invoke(nameof(ClearFeedback), correctFeedbackDuration);
    
    // Генерируем новую ноту с задержкой
    Invoke(nameof(GenerateNextNote), noteDisplayDelay);
}
        
       public void OnIncorrectAnswer(string pressedNote)
{
    Debug.Log($"Incorrect answer: {pressedNote}");
    
    // СТАЛО:
    if (uiManager != null)
    {
        string russianNote = NoteData.Instance.GetTranslatedNoteName(pressedNote);
        uiManager.ShowFeedback(russianNote, false); // false = неправильный
    }
    
    // УДАЛЯЕМ эту строку (uiManager сам очистит):
    // Invoke(nameof(ClearFeedback), incorrectFeedbackDuration);
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
