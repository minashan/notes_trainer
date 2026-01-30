using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoteGenerator : MonoBehaviour
{
    private List<string> allNotes = new List<string>();
    private List<string> notesForCurrentLevel = new List<string>();
    private bool allowEnharmonic = true;
    private string lastGeneratedNote; // ← ДОБАВЬТЕ ЭТО
    
    public event Action<string> OnNoteGenerated;
    
    private void Awake()
    {
        InitializeAllNotes();
    }
    
    private void InitializeAllNotes()
    {
        allNotes = new List<string>();
        // УДАЛЕНО: Debug.Log($"Note: {allNotes}");
    }
    
    public string GenerateRandomNote()
    {
        string selectedNote;
        
        if (notesForCurrentLevel != null && notesForCurrentLevel.Count > 0)
        {
            int randomIndex = Random.Range(0, notesForCurrentLevel.Count);
            selectedNote = notesForCurrentLevel[randomIndex];
            Debug.Log($"Note: {selectedNote}"); // ← УПРОЩЕНО
        }
        else
        {
            int randomIndex = Random.Range(0, allNotes.Count);
            selectedNote = allNotes[randomIndex];
            Debug.Log($"Note: {selectedNote}"); // ← УПРОЩЕНО
        }
        
        lastGeneratedNote = selectedNote; // ← СОХРАНЯЕМ
        OnNoteGenerated?.Invoke(selectedNote);
        
        return selectedNote;
    }
    
    public void SetLevelNotes(List<string> notes, bool allowEnharmonicNotes)
    {
        notesForCurrentLevel = new List<string>(notes);
        allowEnharmonic = allowEnharmonicNotes;
        Debug.Log($"Level notes: {notes.Count}"); // ← УПРОЩЕНО
    }
    
    // ДОБАВЬТЕ ЭТОТ МЕТОД:
    public string GetLastGeneratedNote()
    {
        return lastGeneratedNote;
    }
}