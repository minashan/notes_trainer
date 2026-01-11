using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class NoteData : MonoBehaviour
{
    public static NoteData Instance { get; private set; }
    public Dictionary<string, NotePosition> NoteSettings { get; private set; }
    public Dictionary<string, string[]> EnharmonicEquivalents { get; private set; }

    // Словарь переводов Английский → Русский
    public Dictionary<string, string> NoteTranslations { get; private set; }
    
    void Awake()
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
        InitializeEnharmonicEquivalents();
        InitializeTranslations();
    }


     private void InitializeTranslations()
    {
        NoteTranslations = new Dictionary<string, string>()
        {
            // МАЛАЯ ОКТАВА
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
            
            // ПЕРВАЯ ОКТАВА
            { "C", "До" },
            { "Csharp", "До диез" },
            { "D", "Ре" },
            { "Dflat", "Ре бемоль" },
            { "Dsharp", "Ре диез" },
            { "Eflat", "Ми бемоль" },
            { "E", "Ми" },
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
            
            // ВТОРАЯ ОКТАВА
            { "C2", "До" },
            { "C2sharp", "До диез" },
            { "D2flat", "Ре бемоль" },
            { "D2", "Ре" },
            { "D2sharp", "Ре диез" },
            { "E2flat", "Ми бемоль" },
            { "E2", "Ми" },
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
            
            // ТРЕТЬЯ ОКТАВА
            { "C3", "До" },
            { "C3sharp", "До диез" },
            { "D3flat", "Ре бемоль" },
            { "D3", "Ре" },
            { "D3sharp", "Ре диез" },
            { "E3flat", "Ми бемоль" },
            { "E3", "Ми" }
        };
    }

    // Получение перевода
    public string GetTranslatedNoteName(string englishName)
    {
        if (NoteTranslations.ContainsKey(englishName))
            return NoteTranslations[englishName];
        else
        {
            Debug.LogWarning($"Перевод для ноты '{englishName}' не найден!");
            return englishName; // Возвращаем оригинал, если перевода нет
        }
    }


    private void InitializeNoteSettings()
    {
        NoteSettings = new Dictionary<string, NotePosition>()
        {
            { "F1", new NotePosition() { containerY = -2f, noteSpriteY = 130f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F1sharp", new NotePosition() { containerY = -2f, noteSpriteY = 130f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f },accidentalX = -67f, accidentalY = 90f,showAccidental = true,isSharp = true } },
        { "G1flat", new NotePosition() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 123f, showAccidental = true, isSharp = false } },
        { "G1", new NotePosition() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "G1sharp", new NotePosition() { containerY = -2f, noteSpriteY = 145f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 103f,showAccidental = true,isSharp = true } },
        { "A1flat", new NotePosition() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 141f, showAccidental = true, isSharp = false } },
        { "A1", new NotePosition() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "A1sharp", new NotePosition() { containerY = -2f, noteSpriteY = 161f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 123f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 123f,showAccidental = true,isSharp = true } },
        { "B1flat", new NotePosition() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 154f, showAccidental = true, isSharp = false } },
        { "B1", new NotePosition() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },

        
        // ПЕРВАЯ ОКТАВА (до A - штиль вверх)
        { "C", new NotePosition() { containerY = -2f, noteSpriteY = 192f, noteSpriteX = 5f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Csharp", new NotePosition() { containerY = -2f, noteSpriteY = 190f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f },accidentalX = -67f,accidentalY = 151f,showAccidental = true,isSharp = true } },
        { "D", new NotePosition() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Dflat", new NotePosition() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 181f, showAccidental = true, isSharp = false } },
        { "Dsharp", new NotePosition() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 165f,showAccidental = true,isSharp = true } },
        { "Eflat", new NotePosition() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 203f, showAccidental = true, isSharp = false } },
        { "E", new NotePosition() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F", new NotePosition() { containerY = -2f, noteSpriteY = 238f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Fsharp", new NotePosition() { containerY = -2f, noteSpriteY = 238f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 200f,showAccidental = true,isSharp = true } },
        { "Gflat", new NotePosition() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 237f, showAccidental = true, isSharp = false } },
        { "G", new NotePosition() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Gsharp", new NotePosition() { containerY = -2f, noteSpriteY = 256f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 217f,showAccidental = true,isSharp = true } },
        { "Aflat", new NotePosition() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 256f, showAccidental = true, isSharp = false } },
        { "A", new NotePosition() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Asharp", new NotePosition() { containerY = -2f, noteSpriteY = 273f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 234f,showAccidental = true,isSharp = true } },
        
        // ОБНОВЛЕННЫЕ НОТЫ СО ШТИЛЕМ ВНИЗ:
        { "Bflat", new NotePosition() { containerY = -52f, noteSpriteY = 265f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 320f, showAccidental = true, isSharp = false } },
        { "B", new NotePosition() { containerY = -52f, noteSpriteY = 265f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "C2", new NotePosition() { containerY = -50.8f, noteSpriteY = 281f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "C2sharp", new NotePosition() { containerY = -50.8f, noteSpriteY = 281f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 318f,showAccidental = true,isSharp = true } },
        { "D2flat", new NotePosition() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 357f, showAccidental = true, isSharp = false } },
        { "D2", new NotePosition() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "D2sharp", new NotePosition() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 335f,showAccidental = true,isSharp = true } },
        { "E2flat", new NotePosition() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 372f, showAccidental = true, isSharp = false } },
        { "E2", new NotePosition() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F2", new NotePosition() { containerY = -48.5f, noteSpriteY = 334f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "F2sharp", new NotePosition() { containerY = -48.5f, noteSpriteY = 334f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 367f,showAccidental = true,isSharp = true } },
        { "G2flat", new NotePosition() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 411f, showAccidental = true, isSharp = false } },
        { "G2", new NotePosition() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "G2sharp", new NotePosition() { containerY = -51.9f, noteSpriteY = 352f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 390f,showAccidental = true,isSharp = true } },
        { "A2flat", new NotePosition() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 89.1f }, ledgerLinesX = new float[] { 0f, 0f, 9f }, accidentalX = -67f, accidentalY = 110f, showAccidental = true, isSharp = false } },
        { "A2", new NotePosition() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 89.1f }, ledgerLinesX = new float[] { 0f, 0f, 9f } } },
        { "A2sharp", new NotePosition() { containerY = 264.1f, noteSpriteY = 52f, noteSpriteX = 1.1f, ledgerLinesY = new float[] { 0f, 0f, 89.1f }, ledgerLinesX = new float[] { 0f, 0f, 9f }, accidentalX = -67f,accidentalY = 87f,showAccidental = true,isSharp = true } },
        { "B2flat", new NotePosition() { containerY = 265.3f, noteSpriteY = 67f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 86.8f }, ledgerLinesX = new float[] { 0f, 0f, 4f }, accidentalX = -67f, accidentalY = 123f, showAccidental = true, isSharp = false } },
        { "B2", new NotePosition() { containerY = 265.3f, noteSpriteY = 67f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 86.8f }, ledgerLinesX = new float[] { 0f, 0f, 4f } } },
        { "C3", new NotePosition() { containerY = 264.1f, noteSpriteY = 87f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "C3sharp", new NotePosition() { containerY = 264.1f, noteSpriteY = 87f, noteSpriteX = -10f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 120f,showAccidental = true,isSharp = true } },
        { "D3flat", new NotePosition() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 7.3f, 7.3f }, accidentalX = -67f, accidentalY = 158f, showAccidental = true, isSharp = false } },
        { "D3", new NotePosition() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 7.3f, 7.3f } } },
        { "D3sharp", new NotePosition() { containerY = 264.1f, noteSpriteY = 101f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 7.3f, 7.3f }, accidentalX = -67f,accidentalY = 138f,showAccidental = true,isSharp = true } },
        { "E3flat", new NotePosition() { containerY = 264.1f, noteSpriteY = 111f, noteSpriteX = 0f, ledgerLinesY = new float[] { 146.9f, 116.9f, 86.6f }, ledgerLinesX = new float[] { 8.7f, 8.7f, 8.7f }, accidentalX = -67f, accidentalY = 168f, showAccidental = true, isSharp = false } },
        { "E3", new NotePosition() { containerY = 264.1f, noteSpriteY = 111f, noteSpriteX = 0f, ledgerLinesY = new float[] { 146.9f, 116.9f, 86.6f }, ledgerLinesX = new float[] { 8.7f, 8.7f, 8.7f } } }
        };
    }
    
    private void InitializeEnharmonicEquivalents()
    {
        EnharmonicEquivalents = new Dictionary<string, string[]>()
        {
            { "F1sharp", new string[] { "G1flat" } },
            { "G1sharp", new string[] { "A1flat" } },
            { "A1sharp", new string[] { "B1flat" } },

            { "Csharp",  new string[] { "Dflat" } },
            { "Dsharp",  new string[] { "Eflat" } },
            { "Fsharp",  new string[] { "Gflat" } },
            { "Gsharp",  new string[] { "Aflat" } },
            { "Asharp",  new string[] { "Bflat" } },

            // ВТОРАЯ ОКТАВА
            { "C2sharp", new string[] { "D2flat" } },
            { "D2sharp", new string[] { "E2flat" } },
            { "F2sharp", new string[] { "G2flat" } },
            { "G2sharp", new string[] { "A2flat" } },
            { "A2sharp", new string[] { "B2flat" } },

            // ТРЕТЬЯ ОКТАВА (если есть диезы)
            { "C3sharp", new string[] { "D3flat" } },
            { "D3sharp", new string[] { "E3flat" } },

            // ОБРАТНЫЕ СВЯЗИ (чтобы работало в обе стороны
            { "G1flat", new string[] { "F1sharp" } },
            { "A1flat", new string[] { "G1sharp" } },
            { "B1flat", new string[] { "A1sharp" } },

            { "Dflat",  new string[] { "Csharp" } },
            { "Eflat",  new string[] { "Dsharp" } },
            { "Gflat",  new string[] { "Fsharp" } },
            { "Aflat",  new string[] { "Gsharp" } },
            { "Bflat",  new string[] { "Asharp" } }
        };
    }
    
    public NotePosition GetNotePosition(string noteName)
    {
        if (NoteSettings.ContainsKey(noteName))
            return NoteSettings[noteName];
        else
        {
            Debug.LogWarning($"Note '{noteName}' not found!");
            return null;
        }
    }
    
    public bool AreNotesEnharmonic(string note1, string note2)
    {
        if (EnharmonicEquivalents.ContainsKey(note1) && 
            EnharmonicEquivalents[note1].Contains(note2))
            return true;
            
        if (EnharmonicEquivalents.ContainsKey(note2) && 
            EnharmonicEquivalents[note2].Contains(note1))
            return true;
            
        return false;
    }
}