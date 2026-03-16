using UnityEngine;
using System.Collections.Generic;
using NotesTrainer;

public class SmartNoteGenerator : MonoBehaviour
{
    private LevelData _currentLevel;
    private List<string> _noteBag = new();
    private string _lastGeneratedNote = "";
    private Dictionary<string, int> _noteGuesses = new();
    private ClefType _currentClef;
    
    private const int NOTES_PER_LEVEL_MULTIPLIER = 2;
    private const int MAX_SELECTION_ATTEMPTS = 10;
    
    public int TotalNotes => _currentLevel?.includedNotes.Length ?? 0;
    public int NotesGuessed { get; private set; }
    
    public event System.Action<string> OnNoteGenerated;
    public event System.Action<int, int> OnProgressUpdated;
    
    private void Start() => NotesGuessed = 0;
    
    public void SetLevel(LevelData level)
{
    _currentLevel = level;
    _currentClef = SceneNavigator.Instance.LoadSelectedClef();
    
    // 🔥 Добавить лог
    Debug.Log($"SetLevel: loading level '{level.name}' with {level.includedNotes.Length} notes");
    foreach (var note in level.includedNotes)
    {
        Debug.Log($" - note: {note}");
    }
    
    ResetProgress();
    FillNoteBag();
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
    if (_currentLevel?.includedNotes == null) return;
    
    Debug.Log($"Filling note bag with {_currentLevel.includedNotes.Length} notes:");
    foreach (string note in _currentLevel.includedNotes)
    {
        Debug.Log($"Adding note: {note}");
        _noteBag.Add(note);
        _noteBag.Add(note);
        _noteGuesses[note] = 0;
    }
    
    ShuffleBag();
    OnProgressUpdated?.Invoke(NotesGuessed, TotalNotes * 2);
}
    
    private void ShuffleBag()
    {
        System.Random rng = new();
        int n = _noteBag.Count;
        
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (_noteBag[k], _noteBag[n]) = (_noteBag[n], _noteBag[k]);
        }
    }
    
    public string GenerateNote()
{
    if (_noteBag.Count == 0) return null;
    
    string selectedNote;
    
    if (_noteBag.Count == 1 && _noteBag[0] == _lastGeneratedNote)
    {
        selectedNote = _noteBag[0];
        _noteBag.RemoveAt(0);
    }
    else
    {
        int attempts = 0;
        int randomIndex;
        
        do
        {
            randomIndex = Random.Range(0, _noteBag.Count);
            attempts++;
        } 
        while (_noteBag[randomIndex] == _lastGeneratedNote && 
               _noteBag.Count > 1 && 
               attempts < MAX_SELECTION_ATTEMPTS);
        
        selectedNote = _noteBag[randomIndex];
        _noteBag.RemoveAt(randomIndex);
    }
    
    // 🔥 Добавляем лог для отладки
    Debug.Log($"Generating note: {selectedNote} for clef {_currentClef}");
    
    _lastGeneratedNote = selectedNote;
    OnNoteGenerated?.Invoke(selectedNote);
    return selectedNote;
}
    
    public void RegisterCorrectGuess(string note)
    {
        if (!_noteGuesses.ContainsKey(note)) return;
        
        _noteGuesses[note]++;
        NotesGuessed++;
        OnProgressUpdated?.Invoke(NotesGuessed, TotalNotes * NOTES_PER_LEVEL_MULTIPLIER);
    }
    
    public string GetProgressText() => $"{NotesGuessed}/{TotalNotes * NOTES_PER_LEVEL_MULTIPLIER}";
}