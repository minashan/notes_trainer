using System;
using System.Collections.Generic;
using UnityEngine;
using NotesTrainer;

public class NoteData : MonoBehaviour
{
    public static NoteData Instance { get; private set; }
    public Dictionary<string, NotePosition> NoteSettings { get; private set; }
    public Dictionary<string, string[]> EnharmonicEquivalents { get; private set; }
    public Dictionary<string, string> NoteTranslations { get; private set; }
    public Dictionary<string, NotePosition> BassNoteSettings { get; private set; }

    private ClefType _currentClef = ClefType.Treble;



    public void SetCurrentClef(ClefType clef)
{
    _currentClef = clef;
}
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeData()
    {
        InitializeNoteSettings();
        InitializeBassNoteSettings();
        InitializeEnharmonicEquivalents();
        InitializeTranslations();
    }
    
    private void InitializeTranslations()
    {
        NoteTranslations = new Dictionary<string, string>
        {
            // 🔥 НОВЫЕ: Контр-октава
        { "A_", "Ля" },
        { "A_sharp", "Ля диез" },
        { "B_flat", "Си бемоль" },
        { "B_", "Си" },
        { "B_sharp", "Си диез" },
        
        // Большая октава
        { "C0flat", "До бемоль" },
        { "C0", "До" },
        { "C0sharp", "До диез" },
        { "D0flat", "Ре бемоль" },
        { "D0", "Ре" },
        { "D0sharp", "Ре диез" },
        { "E0flat", "Ми бемоль" },
        { "E0", "Ми" },
        { "E0sharp", "Ми диез" },
        { "F0flat", "Фа бемоль" },
        { "F0", "Фа" },
        { "F0sharp", "Фа диез" },
        { "G0flat", "Соль бемоль" },
        { "G0", "Соль" },
        { "G0sharp", "Соль диез" },
        { "A0flat", "Ля бемоль" },
        { "A0", "Ля" },
        { "A0sharp", "Ля диез" },
        { "B0flat", "Си бемоль" },
        { "B0", "Си" },
        { "B0sharp", "Си диез" },
        
        // Малая октава
        { "C1flat", "До бемоль" },
        { "C1", "До" },
        { "C1sharp", "До диез" },
        { "D1flat", "Ре бемоль" },
        { "D1", "Ре" },
        { "D1sharp", "Ре диез" },
        { "E1flat", "Ми бемоль" },
        { "E1", "Ми" },
        { "E1sharp", "Ми диез" },
        { "F1flat", "Фа бемоль" },
        
        
        

            // Lower octave
            { "F1", "Фа" },
            { "F1sharp", "Фа диез" },
            { "G1flat", "Соль бемоль" },
            { "G1", "Соль" },
            { "G1sharp", "Соль диез" },
            { "A1flat", "Ля бемоль" },
            { "A1", "Ля" },
            { "A1sharp", "Ля диез" },
            { "B1flat", "Си бемоль" },
            { "B1", "Си" },
            { "B1sharp", "Си диез"},
            
            // First octave
            { "C", "До" },
            { "Cflat", "До бемоль"},
            { "Csharp", "До диез" },
            { "D", "Ре" },
            { "Dflat", "Ре бемоль" },
            { "Dsharp", "Ре диез" },
            { "Eflat", "Ми бемоль" },
            { "E", "Ми" },
            { "Esharp", "Ми диез"},
            { "Fflat", "Фа бемоль"},
            { "F", "Фа" },
            { "Fsharp", "Фа диез" },
            { "Gflat", "Соль бемоль" },
            { "G", "Соль" },
            { "Gsharp", "Соль диез" },
            { "Aflat", "Ля бемоль" },
            { "A", "Ля" },
            { "Asharp", "Ля диез" },
            { "Bflat", "Си бемоль" },
            { "B", "Си" },
            { "Bsharp", "Си диез"},
            
            // Second octave
            { "C2flat", "До бемоль"},
            { "C2", "До" },
            { "C2sharp", "До диез" },
            { "D2flat", "Ре бемоль" },
            { "D2", "Ре" },
            { "D2sharp", "Ре диез" },
            { "E2flat", "Ми бемоль" },
            { "E2", "Ми" },
            { "E2sharp", "Ми диез"},
            { "F2flat", "Фа бемоль"},
            { "F2", "Фа" },
            { "F2sharp", "Фа диез" },
            { "G2flat", "Соль бемоль" },
            { "G2", "Соль" },
            { "G2sharp", "Соль диез" },
            { "A2flat", "Ля бемоль" },
            { "A2", "Ля" },
            { "A2sharp", "Ля диез" },
            { "B2flat", "Си бемоль" },
            { "B2", "Си" },
            { "B2sharp", "Си диез"},
            
            // Third octave
            { "C3flat", "До бемоль"},
            { "C3", "До" },
            { "C3sharp", "До диез" },
            { "D3flat", "Ре бемоль" },
            { "D3", "Ре" },
            { "D3sharp", "Ре диез" },
            { "E3flat", "Ми бемоль" },
            { "E3", "Ми" }
        };
    }
    
    public string GetTranslatedNoteName(string englishName)
    {
        return NoteTranslations.TryGetValue(englishName, out string translation) 
            ? translation 
            : englishName;
    }
    
    private void InitializeNoteSettings()
    {
        NoteSettings = new Dictionary<string, NotePosition>
        {
            { "F1", new() { containerY = -2f, noteSpriteY = 130f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "F1sharp", new() { containerY = -2f, noteSpriteY = 130f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f },
                accidentalX = -67f, accidentalY = 90f, showAccidental = true, isSharp = true } },
            { "G1flat", new() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 123f, showAccidental = true, isSharp = false } },
            { "G1", new() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "G1sharp", new() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 103f, showAccidental = true, isSharp = true } },
            { "A1flat", new() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 141f, showAccidental = true, isSharp = false } },
            { "A1", new() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "A1sharp", new() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 123f, showAccidental = true, isSharp = true } },
            { "B1flat", new() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 154f, showAccidental = true, isSharp = false } },
            { "B1", new() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "B1sharp", new() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 5f, 
                ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 137f, showAccidental = true, isSharp = true } },
            
            // First octave (до A - штиль вверх)
            { "Cflat", new() { containerY = -2f, noteSpriteY = 192f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 173f, showAccidental = true, isSharp = false } },
            { "C", new() { containerY = -2f, noteSpriteY = 192f, noteSpriteX = 5f, 
                ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "Csharp", new() { containerY = -2f, noteSpriteY = 190f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 151f, showAccidental = true, isSharp = true } },
            { "D", new() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "Dflat", new() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 181f, showAccidental = true, isSharp = false } },
            { "Dsharp", new() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 165f, showAccidental = true, isSharp = true } },
            { "Eflat", new() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 203f, showAccidental = true, isSharp = false } },
            { "E", new() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "Esharp", new() { containerY = -2f, noteSpriteY = 222f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 184f, showAccidental = true, isSharp = true } },
            { "Fflat", new() { containerY = -2f, noteSpriteY = 239f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 222f, showAccidental = true, isSharp = false } },
            { "F", new() { containerY = -2f, noteSpriteY = 238f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "Fsharp", new() { containerY = -2f, noteSpriteY = 238f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 200f, showAccidental = true, isSharp = true } },
            { "Gflat", new() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 237f, showAccidental = true, isSharp = false } },
            { "G", new() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "Gsharp", new() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 217f, showAccidental = true, isSharp = true } },
            { "Aflat", new() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 256f, showAccidental = true, isSharp = false } },
            { "A", new() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "Asharp", new() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 234f, showAccidental = true, isSharp = true } },
            
            // Updated notes with stem down
            { "Bflat", new() { containerY = -52f, noteSpriteY = 265f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 320f, showAccidental = true, isSharp = false } },
            { "B", new() { containerY = -52f, noteSpriteY = 265f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "Bsharp", new() { containerY = -50.8f, noteSpriteY = 265f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 301f, showAccidental = true, isSharp = true } },
            { "C2flat", new() { containerY = -52f, noteSpriteY = 282f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 342f, showAccidental = true, isSharp = false } },
            { "C2", new() { containerY = -50.8f, noteSpriteY = 282f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "C2sharp", new() { containerY = -50.8f, noteSpriteY = 281f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 318f, showAccidental = true, isSharp = true } },
            { "D2flat", new() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 357f, showAccidental = true, isSharp = false } },
            { "D2", new() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "D2sharp", new() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 335f, showAccidental = true, isSharp = true } },
            { "E2flat", new() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 372f, showAccidental = true, isSharp = false } },
            { "E2", new() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "E2sharp", new() { containerY = -48.5f, noteSpriteY = 314f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 351f, showAccidental = true, isSharp = true } },
            { "F2flat", new() { containerY = -49.6f, noteSpriteY = 333f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 389f, showAccidental = true, isSharp = false } },
            { "F2", new() { containerY = -48.5f, noteSpriteY = 334f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "F2sharp", new() { containerY = -48.5f, noteSpriteY = 334f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 367f, showAccidental = true, isSharp = true } },
            { "G2flat", new() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 411f, showAccidental = true, isSharp = false } },
            { "G2", new() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "G2sharp", new() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 390f, showAccidental = true, isSharp = true } },
            { "A2flat", new() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 89.1f }, ledgerLinesX = new[] { 0f, 0f, 9f }, 
                accidentalX = -67f, accidentalY = 110f, showAccidental = true, isSharp = false } },
            { "A2", new() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 89.1f }, ledgerLinesX = new[] { 0f, 0f, 9f } } },
            { "A2sharp", new() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 1.1f, 
                ledgerLinesY = new[] { 0f, 0f, 89.1f }, ledgerLinesX = new[] { 0f, 0f, 9f }, 
                accidentalX = -67f, accidentalY = 87f, showAccidental = true, isSharp = true } },
            { "B2flat", new() { containerY = 265.3f, noteSpriteY = 67f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 86.8f }, ledgerLinesX = new[] { 0f, 0f, 4f }, 
                accidentalX = -67f, accidentalY = 123f, showAccidental = true, isSharp = false } },
            { "B2", new() { containerY = 265.3f, noteSpriteY = 67f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 86.8f }, ledgerLinesX = new[] { 0f, 0f, 4f } } },
            { "B2sharp", new() { containerY = 264.1f, noteSpriteY = 75f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 0f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 105f, showAccidental = true, isSharp = true } },
            { "C3flat", new() { containerY = 265.3f, noteSpriteY = 87f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 4f }, 
                accidentalX = -67f, accidentalY = 143f, showAccidental = true, isSharp = false } },
            { "C3", new() { containerY = 264.1f, noteSpriteY = 87f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            { "C3sharp", new() { containerY = 264.1f, noteSpriteY = 87f, noteSpriteX = -10f, 
                ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 120f, showAccidental = true, isSharp = true } },
            { "D3flat", new() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 7.3f, 7.3f }, 
                accidentalX = -67f, accidentalY = 158f, showAccidental = true, isSharp = false } },
            { "D3", new() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 7.3f, 7.3f } } },
            { "D3sharp", new() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 7.3f, 7.3f }, 
                accidentalX = -67f, accidentalY = 138f, showAccidental = true, isSharp = true } },
            { "E3flat", new() { containerY = 264.1f, noteSpriteY = 111f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 146.9f, 116.9f, 86.6f }, ledgerLinesX = new[] { 8.7f, 8.7f, 8.7f }, 
                accidentalX = -67f, accidentalY = 168f, showAccidental = true, isSharp = false } },
            { "E3", new() { containerY = 264.1f, noteSpriteY = 111f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 146.9f, 116.9f, 86.6f }, ledgerLinesX = new[] { 8.7f, 8.7f, 8.7f } } }
        };
    }
    
    private void InitializeEnharmonicEquivalents()
    {
        EnharmonicEquivalents = new Dictionary<string, string[]>
        {
              // Контр-октава
        { "A_sharp", new[] { "B_flat" } },
        { "B_flat", new[] { "A_sharp" } },
        { "B_sharp", new[] { "C0" } },
        
        // Большая октава
        { "C0sharp", new[] { "D0flat" } },
        { "D0flat", new[] { "C0sharp" } },
        { "D0sharp", new[] { "E0flat" } },
        { "E0flat", new[] { "D0sharp" } },
        { "E0sharp", new[] { "F0" } },
        { "F0flat", new[] { "E0" } },
        { "F0sharp", new[] { "G0flat" } },
        { "G0flat", new[] { "F0sharp" } },
        { "G0sharp", new[] { "A0flat" } },
        { "A0flat", new[] { "G0sharp" } },
        { "A0sharp", new[] { "B0flat" } },
        { "B0flat", new[] { "A0sharp" } },
        { "B0sharp", new[] { "C1" } },
        
        // Малая октава
        { "C1sharp", new[] { "D1flat" } },
        { "D1flat", new[] { "C1sharp" } },
        { "D1sharp", new[] { "E1flat" } },
        { "E1flat", new[] { "D1sharp" } },
        { "E1sharp", new[] { "F1" } },
        

            // Малая октава
            { "F1sharp", new[] { "G1flat" } },
            { "G1flat",  new[] { "F1sharp" } },
            { "G1sharp", new[] { "A1flat" } },
            { "A1flat",  new[] { "G1sharp" } },
            { "A1sharp", new[] { "B1flat" } },
            { "B1flat",  new[] { "A1sharp" } },
            { "B1sharp", new[] { "C" } },
            { "Cflat",   new[] { "B1" } },
            
            // 1 октава
            { "Csharp",  new[] { "Dflat" } },
            { "Dflat",   new[] { "Csharp" } },
            { "Dsharp",  new[] { "Eflat" } },
            { "Eflat",   new[] { "Dsharp" } },
            { "Esharp",  new[] { "F" } },
            { "Fflat",   new[] { "E" } },
            { "Fsharp",  new[] { "Gflat" } },
            { "Gflat",   new[] { "Fsharp" } },
            { "Gsharp",  new[] { "Aflat" } },
            { "Aflat",   new[] { "Gsharp" } },
            { "Asharp",  new[] { "Bflat" } },
            { "Bflat",   new[] { "Asharp" } },
            { "Bsharp",  new[] { "C2" } },
            { "C2flat",  new[] { "B" } },
            
            // 2 октава
            { "C2sharp", new[] { "D2flat" } },
            { "D2flat",  new[] { "C2sharp" } },
            { "D2sharp", new[] { "E2flat" } },
            { "E2flat",  new[] { "D2sharp" } },
            { "E2sharp", new[] { "F2" } },
            { "F2flat",  new[] { "E2" } },
            { "F2sharp", new[] { "G2flat" } },
            { "G2flat",  new[] { "F2sharp" } },
            { "G2sharp", new[] { "A2flat" } },
            { "A2flat",  new[] { "G2sharp" } },
            { "A2sharp", new[] { "B2flat" } },
            { "B2flat",  new[] { "A2sharp" } },
            { "B2sharp", new[] { "C3" } },
            { "C3flat",  new[] { "B2" } },
            
            // 3 октава
            { "C3sharp", new[] { "D3flat" } },
            { "D3flat",  new[] { "C3sharp" } },
            { "D3sharp", new[] { "E3flat" } },
            { "E3flat",  new[] { "D3sharp" } },
            
            // Обратные связи для белых клавиш
            { "F",       new[] { "Esharp" } },
            { "E",       new[] { "Fflat" } },
            { "C2",      new[] { "Bsharp" } },
            { "B",       new[] { "C2flat" } },
            { "F2",      new[] { "E2sharp" } },
            { "E2",      new[] { "F2flat" } },
            { "C3",      new[] { "B2sharp" } },
            { "B2",      new[] { "C3flat" } },
            { "B1",      new[] { "Cflat" } },

        { "C0", new[] { "B_sharp" } },
        { "E0", new[] { "F0flat" } },
        { "F0", new[] { "E0sharp" } },
        { "C1", new[] { "B0sharp" } },
        { "E1", new[] { "F1flat" } },
        
        };
    }


        private void InitializeBassNoteSettings()
{
    BassNoteSettings = new Dictionary<string, NotePosition>
    {
        // Контр-октава
        { "A_", new() { containerY = -2f, noteSpriteY = 130f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
            
        { "A_sharp", new() { containerY = -2f, noteSpriteY = 130f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f },
                accidentalX = -67f, accidentalY = 90f, showAccidental = true, isSharp = true } },
        
        { "B_flat", new() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 123f, showAccidental = true, isSharp = false } },
        
        { "B_", new() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },
        
        { "B_sharp", new() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, 
                ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f }, 
                accidentalX = -67f, accidentalY = 103f, showAccidental = true, isSharp = true } },


            // Большая октава
{ "C0flat", new() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 141f, showAccidental = true, isSharp = false } },

{ "C0", new() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "C0sharp", new() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 153f, 123f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 123f, showAccidental = true, isSharp = true } },

{ "D0flat", new() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 154f, showAccidental = true, isSharp = false } },

{ "D0", new() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "D0sharp", new() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 5f,
    ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 137f, showAccidental = true, isSharp = true } },

{ "E0flat", new() { containerY = -2f, noteSpriteY = 192f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 173f, showAccidental = true, isSharp = false } },

{ "E0", new() { containerY = -2f, noteSpriteY = 192f, noteSpriteX = 5f,
    ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "E0sharp", new() { containerY = -2f, noteSpriteY = 190f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 153f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 151f, showAccidental = true, isSharp = true } },

{ "F0flat", new() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 181f, showAccidental = true, isSharp = false } },

{ "F0", new() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "F0sharp", new() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 165f, showAccidental = true, isSharp = true } },

{ "G0flat", new() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 203f, showAccidental = true, isSharp = false } },

{ "G0", new() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "G0sharp", new() { containerY = -2f, noteSpriteY = 222f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 184f, showAccidental = true, isSharp = true } },

{ "A0flat", new() { containerY = -2f, noteSpriteY = 239f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 222f, showAccidental = true, isSharp = false } },

{ "A0", new() { containerY = -2f, noteSpriteY = 238f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "A0sharp", new() { containerY = -2f, noteSpriteY = 238f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 200f, showAccidental = true, isSharp = true } },

{ "B0flat", new() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 237f, showAccidental = true, isSharp = false } },

{ "B0", new() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "B0sharp", new() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 217f, showAccidental = true, isSharp = true } },


    // Малая октава
{ "C1flat", new() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 256f, showAccidental = true, isSharp = false } },

{ "C1", new() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "C1sharp", new() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 234f, showAccidental = true, isSharp = true } },

{ "D1flat", new() { containerY = -52f, noteSpriteY = 265f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 320f, showAccidental = true, isSharp = false } },

{ "D1", new() { containerY = -52f, noteSpriteY = 265f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "D1sharp", new() { containerY = -50.8f, noteSpriteY = 265f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 301f, showAccidental = true, isSharp = true } },

{ "E1flat", new() { containerY = -52f, noteSpriteY = 282f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 342f, showAccidental = true, isSharp = false } },

{ "E1", new() { containerY = -50.8f, noteSpriteY = 282f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "E1sharp", new() { containerY = -50.8f, noteSpriteY = 281f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 318f, showAccidental = true, isSharp = true } },

{ "F1flat", new() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 357f, showAccidental = true, isSharp = false } },

{ "F1", new() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "F1sharp", new() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 335f, showAccidental = true, isSharp = true } },

{ "G1flat", new() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 372f, showAccidental = true, isSharp = false } },

{ "G1", new() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "G1sharp", new() { containerY = -48.5f, noteSpriteY = 314f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 351f, showAccidental = true, isSharp = true } },

{ "A1flat", new() { containerY = -49.6f, noteSpriteY = 333f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 389f, showAccidental = true, isSharp = false } },

{ "A1", new() { containerY = -48.5f, noteSpriteY = 334f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "A1sharp", new() { containerY = -48.5f, noteSpriteY = 334f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 367f, showAccidental = true, isSharp = true } },

{ "B1flat", new() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 411f, showAccidental = true, isSharp = false } },

{ "B1", new() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "B1sharp", new() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 0f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 390f, showAccidental = true, isSharp = true } },



    // Первая октава (позиции из A2flat, A2, A2sharp, B2flat, B2, B2sharp, C3flat, C3, C3sharp)
{ "Cflat", new() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 89.1f }, ledgerLinesX = new[] { 0f, 0f, 9f },
    accidentalX = -67f, accidentalY = 110f, showAccidental = true, isSharp = false } },

{ "C", new() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 89.1f }, ledgerLinesX = new[] { 0f, 0f, 9f } } },

{ "Csharp", new() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 1.1f,
    ledgerLinesY = new[] { 0f, 0f, 89.1f }, ledgerLinesX = new[] { 0f, 0f, 9f },
    accidentalX = -67f, accidentalY = 87f, showAccidental = true, isSharp = true } },

{ "Dflat", new() { containerY = 265.3f, noteSpriteY = 67f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 86.8f }, ledgerLinesX = new[] { 0f, 0f, 4f },
    accidentalX = -67f, accidentalY = 123f, showAccidental = true, isSharp = false } },

{ "D", new() { containerY = 265.3f, noteSpriteY = 67f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 86.8f }, ledgerLinesX = new[] { 0f, 0f, 4f } } },

{ "Dsharp", new() { containerY = 264.1f, noteSpriteY = 75f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 0f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 105f, showAccidental = true, isSharp = true } },

{ "Eflat", new() { containerY = 265.3f, noteSpriteY = 87f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 4f },
    accidentalX = -67f, accidentalY = 143f, showAccidental = true, isSharp = false } },

{ "E", new() { containerY = 264.1f, noteSpriteY = 87f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f } } },

{ "Esharp", new() { containerY = 264.1f, noteSpriteY = 87f, noteSpriteX = -10f,
    ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 0f, 0f },
    accidentalX = -67f, accidentalY = 120f, showAccidental = true, isSharp = true } },

{ "Fflat", new() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 7.3f, 7.3f },
    accidentalX = -67f, accidentalY = 158f, showAccidental = true, isSharp = false } },

{ "F", new() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 7.3f, 7.3f } } },

{ "Fsharp", new() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 0f, 123f, 92.7f }, ledgerLinesX = new[] { 0f, 7.3f, 7.3f },
    accidentalX = -67f, accidentalY = 138f, showAccidental = true, isSharp = true } },

{ "Gflat", new() { containerY = 264.1f, noteSpriteY = 111f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 146.9f, 116.9f, 86.6f }, ledgerLinesX = new[] { 8.7f, 8.7f, 8.7f },
    accidentalX = -67f, accidentalY = 168f, showAccidental = true, isSharp = false } },

{ "G", new() { containerY = 264.1f, noteSpriteY = 111f, noteSpriteX = 0f,
    ledgerLinesY = new[] { 146.9f, 116.9f, 86.6f }, ledgerLinesX = new[] { 8.7f, 8.7f, 8.7f } } },

    };
}

           public ClefType GetCurrentClef() => _currentClef;

    
    public NotePosition GetNotePosition(string noteName)
{
    var settings = _currentClef == ClefType.Treble ? NoteSettings : BassNoteSettings;
    return settings.TryGetValue(noteName, out NotePosition pos) ? pos : null;
}
    
    public bool AreNotesEnharmonic(string note1, string note2)
    {
        if (string.IsNullOrEmpty(note1) || string.IsNullOrEmpty(note2)) return false;
        if (note1 == note2) return true;
        if (EnharmonicEquivalents == null) return false;
        
        if (EnharmonicEquivalents.TryGetValue(note1, out string[] equivalents) && 
            equivalents != null && 
            Array.Exists(equivalents, eq => eq == note2))
        {
            return true;
        }
        
        if (EnharmonicEquivalents.TryGetValue(note2, out equivalents) && 
            equivalents != null && 
            Array.Exists(equivalents, eq => eq == note1))
        {
            return true;
        }
        
        return false;
    }
}