using UnityEngine;
using System.Collections.Generic;
using NotesTrainer;

public class SmartNoteGenerator : MonoBehaviour
{
    private LevelData _currentLevel;
    private List<string> _noteBag = new List<string>();
    private string _lastGeneratedNote = "";
    private Dictionary<string, int> _noteGuesses = new Dictionary<string, int>();
    
    public int TotalNotes => _currentLevel?.includedNotes.Length ?? 0;
    public int NotesGuessed { get; private set; }
    
    public event System.Action<string> OnNoteGenerated;
    public event System.Action<int, int> OnProgressUpdated;
    
    void Start() => NotesGuessed = 0;
    
    public void SetLevel(LevelData level)
    {
        Debug.Log($"SetLevel called: {level?.levelName}, notes: {level?.includedNotes?.Length}");
        _currentLevel = level;
        ResetProgress();
        FillNoteBag();
        Debug.Log($"Smart generator: Level '{level.levelName}' with {TotalNotes} notes");
       
    }
    
    private void ResetProgress()
    {
        _noteBag.Clear();
        _noteGuesses.Clear();
        NotesGuessed = 0;
        _lastGeneratedNote = "";
    }
    
    private void FillNoteBag()
    {
        Debug.Log($"FillNoteBag called. CurrentLevel: {_currentLevel != null}");
         if (_currentLevel == null)
    {
        Debug.LogError("CurrentLevel is null in FillNoteBag!");
        return;
    }
    
    Debug.Log($"Notes to add: {_currentLevel.includedNotes.Length}");
        
        foreach (string note in _currentLevel.includedNotes)
        {
            _noteBag.Add(note);
            _noteBag.Add(note);
            _noteGuesses[note] = 0;
        }
        
        ShuffleBag();
        OnProgressUpdated?.Invoke(NotesGuessed, TotalNotes * 2);
        Debug.Log($"Note bag filled: {_noteBag.Count} notes total");
    }
    
    private void ShuffleBag()
    {
        System.Random rng = new System.Random();
        int n = _noteBag.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = _noteBag[k];
            _noteBag[k] = _noteBag[n];
            _noteBag[n] = value;
        }
    }
    
   public string GenerateNote()
{
    if (_noteBag.Count == 0)
    {
        Debug.LogWarning("Note bag is empty!");
        return null;
    }
    
    string selectedNote;
    
    // Если осталась 1 нота и она равна последней - это конец уровня
    if (_noteBag.Count == 1 && _noteBag[0] == _lastGeneratedNote)
    {
        selectedNote = _noteBag[0];
        _noteBag.RemoveAt(0);
    }
    else
    {
        // Обычная логика
        int attempts = 0;
        int randomIndex;
        
        do
        {
            randomIndex = Random.Range(0, _noteBag.Count);
            attempts++;
        } while (_noteBag[randomIndex] == _lastGeneratedNote && _noteBag.Count > 1 && attempts < 10);
        
        selectedNote = _noteBag[randomIndex];
        _noteBag.RemoveAt(randomIndex);
    }
    
    _lastGeneratedNote = selectedNote;
    OnNoteGenerated?.Invoke(selectedNote);
    return selectedNote;
}
    
    public void RegisterCorrectGuess(string note)
    {
        if (!_noteGuesses.ContainsKey(note)) return;
        
        _noteGuesses[note]++;
        NotesGuessed++;
        OnProgressUpdated?.Invoke(NotesGuessed, TotalNotes * 2);
    }
    
    public string GetProgressText() => $"{NotesGuessed}/{TotalNotes * 2}";
}
