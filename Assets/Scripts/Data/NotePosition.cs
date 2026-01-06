using UnityEngine;

[System.Serializable]
public class NotePosition
{
    public float containerY;
    public float noteSpriteY;
    public float noteSpriteX;
    public float[] ledgerLinesY;
    public float[] ledgerLinesX;
    public float accidentalX;
    public float accidentalY;
    public bool showAccidental;
    public bool isSharp;
}