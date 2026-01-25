using System;
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
            { "B1sharp", "Си диез"},
            
            // ПЕРВАЯ ОКТАВА
            { "C", "До" },
            { "Cflat","До бемоль"},
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
            
            // ВТОРАЯ ОКТАВА
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
            
            // ТРЕТЬЯ ОКТАВА
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
        { "B1sharp", new NotePosition() { containerY = -2f, noteSpriteY = 175f, noteSpriteX = 5f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f },accidentalX = -67f,accidentalY = 137f,showAccidental = true,isSharp = true } }, 

        
        // ПЕРВАЯ ОКТАВА (до A - штиль вверх)
        { "Cflat", new NotePosition() { containerY = -2f, noteSpriteY = 192f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f },accidentalX = -67f,accidentalY = 173f,showAccidental = true,isSharp = false  } }, 
        { "C", new NotePosition() { containerY = -2f, noteSpriteY = 192f, noteSpriteX = 5f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Csharp", new NotePosition() { containerY = -2f, noteSpriteY = 190f, noteSpriteX = 0f, ledgerLinesY = new float[] { 153f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f },accidentalX = -67f,accidentalY = 151f,showAccidental = true,isSharp = true } },
        { "D", new NotePosition() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Dflat", new NotePosition() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 181f, showAccidental = true, isSharp = false } },
        { "Dsharp", new NotePosition() { containerY = -2f, noteSpriteY = 206f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 165f,showAccidental = true,isSharp = true } },
        { "Eflat", new NotePosition() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 203f, showAccidental = true, isSharp = false } },
        { "E", new NotePosition() { containerY = -2f, noteSpriteY = 221f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "Esharp", new NotePosition() { containerY = -2f, noteSpriteY = 222f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 184f, showAccidental = true, isSharp = true } }, 
        { "Fflat", new NotePosition() { containerY = -2f, noteSpriteY = 239f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 222f,showAccidental = true,isSharp = false } }, 
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
        { "Bsharp", new NotePosition() { containerY = -50.8f, noteSpriteY = 265f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 301f, showAccidental = true, isSharp = true } }, 
        { "C2flat", new NotePosition() { containerY = -52f, noteSpriteY = 282f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 342f,showAccidental = true,isSharp = false  } }, 
        { "C2", new NotePosition() { containerY = -50.8f, noteSpriteY = 282f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "C2sharp", new NotePosition() { containerY = -50.8f, noteSpriteY = 281f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 318f,showAccidental = true,isSharp = true } },
        { "D2flat", new NotePosition() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 357f, showAccidental = true, isSharp = false } },
        { "D2", new NotePosition() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "D2sharp", new NotePosition() { containerY = -51.6f, noteSpriteY = 299f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 335f,showAccidental = true,isSharp = true } },
        { "E2flat", new NotePosition() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 372f, showAccidental = true, isSharp = false } },
        { "E2", new NotePosition() { containerY = -49.6f, noteSpriteY = 315f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f } } },
        { "E2sharp", new NotePosition() { containerY = -48.5f, noteSpriteY = 314f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 351f, showAccidental = true, isSharp = true } }, 
        { "F2flat", new NotePosition() { containerY = -49.6f, noteSpriteY = 333f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 0f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f,accidentalY = 389f,showAccidental = true,isSharp = false } }, 
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
        { "B2sharp", new NotePosition() { containerY = 264.1f, noteSpriteY = 75f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 0f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 0f }, accidentalX = -67f, accidentalY = 105f, showAccidental = true, isSharp = true } }, 
        { "C3flat", new NotePosition() { containerY = 265.3f, noteSpriteY = 87f, noteSpriteX = 0f, ledgerLinesY = new float[] { 0f, 123f, 92.7f }, ledgerLinesX = new float[] { 0f, 0f, 4f }, accidentalX = -67f,accidentalY = 143f,showAccidental = true,isSharp = false } }, 
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
        // МАЛАЯ ОКТАВА
        { "F1sharp", new string[] { "G1flat" } },
        { "G1flat",  new string[] { "F1sharp" } },
        
        { "G1sharp", new string[] { "A1flat" } },
        { "A1flat",  new string[] { "G1sharp" } },
        
        { "A1sharp", new string[] { "B1flat" } },
        { "B1flat",  new string[] { "A1sharp" } },
        
        { "B1sharp", new string[] { "C" } },
        { "Cflat",   new string[] { "B1" } },      // Cb = B1
        
        // ПЕРВАЯ ОКТАВА
        { "Csharp",  new string[] { "Dflat" } },
        { "Dflat",   new string[] { "Csharp" } },
        
        { "Dsharp",  new string[] { "Eflat" } },
        { "Eflat",   new string[] { "Dsharp" } },
        
        { "Esharp",  new string[] { "F" } },
        { "Fflat",   new string[] { "E" } },       // Fb = E
        
        { "Fsharp",  new string[] { "Gflat" } },
        { "Gflat",   new string[] { "Fsharp" } },
        
        { "Gsharp",  new string[] { "Aflat" } },
        { "Aflat",   new string[] { "Gsharp" } },
        
        { "Asharp",  new string[] { "Bflat" } },
        { "Bflat",   new string[] { "Asharp" } },
        
        { "Bsharp",  new string[] { "C2" } },
        { "C2flat",  new string[] { "B" } },       // C2b = B
        
        // ВТОРАЯ ОКТАВА
        { "C2sharp", new string[] { "D2flat" } },
        { "D2flat",  new string[] { "C2sharp" } },
        
        { "D2sharp", new string[] { "E2flat" } },
        { "E2flat",  new string[] { "D2sharp" } },
        
        { "E2sharp", new string[] { "F2" } },
        { "F2flat",  new string[] { "E2" } },      // F2b = E2
        
        { "F2sharp", new string[] { "G2flat" } },
        { "G2flat",  new string[] { "F2sharp" } },
        
        { "G2sharp", new string[] { "A2flat" } },
        { "A2flat",  new string[] { "G2sharp" } },
        
        { "A2sharp", new string[] { "B2flat" } },
        { "B2flat",  new string[] { "A2sharp" } },
        
        { "B2sharp", new string[] { "C3" } },
        { "C3flat",  new string[] { "B2" } },      // C3b = B2
        
        // ТРЕТЬЯ ОКТАВА
        { "C3sharp", new string[] { "D3flat" } },
        { "D3flat",  new string[] { "C3sharp" } },
        
        { "D3sharp", new string[] { "E3flat" } },
        { "E3flat",  new string[] { "D3sharp" } },
        
        // ОБРАТНЫЕ СВЯЗИ ДЛЯ БЕЛЫХ КЛАВИШ
        { "F",       new string[] { "Esharp" } },
        { "E",       new string[] { "Fflat" } },
        { "C2",      new string[] { "Bsharp" } },
        { "B",       new string[] { "C2flat" } },
        { "F2",      new string[] { "E2sharp" } },
        { "E2",      new string[] { "F2flat" } },
        { "C3",      new string[] { "B2sharp" } },
        { "B2",      new string[] { "C3flat" } },
        { "B1",      new string[] { "Cflat" } }
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
    // 1. Проверка на null и пустоту
    if (string.IsNullOrEmpty(note1) || string.IsNullOrEmpty(note2))
    {
        Debug.LogWarning($"AreNotesEnharmonic: Null or empty notes: '{note1}', '{note2}'");
        return false;
    }
    
    // 2. Прямое совпадение
    if (note1 == note2)
        return true;
    
    // 3. Проверка инициализации словаря
    if (EnharmonicEquivalents == null)
    {
        Debug.LogError("EnharmonicEquivalents dictionary is null!");
        return false;
    }
    
    // 4. Попробуем найти note1 как ключ
    if (EnharmonicEquivalents.TryGetValue(note1, out string[] equivalents))
    {
        if (equivalents != null && Array.Exists(equivalents, eq => eq == note2))
        {
            Debug.Log($"Enharmonic match: {note1} = {note2}");
            return true;
        }
    }
    
    // 5. Попробуем найти note2 как ключ (обратная связь)
    if (EnharmonicEquivalents.TryGetValue(note2, out equivalents))
    {
        if (equivalents != null && Array.Exists(equivalents, eq => eq == note1))
        {
            Debug.Log($"Enharmonic match (reverse): {note2} = {note1}");
            return true;
        }
    }
    
    // 6. Если не нашли - не энгармоничны
    Debug.Log($"No enharmonic match: {note1} ≠ {note2}");
    return false;
}
}