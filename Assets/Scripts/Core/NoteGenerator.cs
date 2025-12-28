using System.Linq;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    // Событие для уведомления о новой ноте
    public System.Action<string> OnNoteGenerated;
    
    // Все доступные ноты (заполняется из GameManager или настройках)
    private string[] availableNotes;
    private string currentNote;
    private string previousNote = "";
    
    // Настройки генерации
    private int maxAttemptsToAvoidRepeat = 10;
    
    public void Initialize(string[] notes)
    {
        availableNotes = notes;
    }
    
    public string GenerateRandomNote()
    {
        if (availableNotes == null || availableNotes.Length == 0)
        {
            return null;
        }
        
        string newNote;
        int attempts = 0;
        
        do {
            newNote = availableNotes[Random.Range(0, availableNotes.Length)];
            attempts++;
        } while (newNote == previousNote && attempts < maxAttemptsToAvoidRepeat);
        
        previousNote = newNote;
        currentNote = newNote;
        
        
        OnNoteGenerated?.Invoke(currentNote);
        
        return currentNote;
    }
    
    public string GetCurrentNote()
    {
        return currentNote;
    }
    
    public void SetAvailableNotes(string[] notes)
    {
        availableNotes = notes;
    }
    
    public void SetAvoidRepeatAttempts(int attempts)
    {
        maxAttemptsToAvoidRepeat = Mathf.Max(1, attempts);
    }
    
    // Для будущих режимов сложности
    public void SetNoteRange(string[] notesSubset)
    {
        availableNotes = notesSubset;
    }
}
