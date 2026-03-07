using UnityEngine;

namespace NotesTrainer
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Notes Trainer/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Basic Settings")]
        public string levelName = "New Level";
        
        [TextArea] 
        public string description = "Level description";
        
        public int requiredScore = 50;
        
        [Header("Notes in Level")]
        public string[] includedNotes;
        
        [Header("Optional Settings")]
        public bool allowEnharmonic;
        public int timeLimit; // 0 = no time limit
        
        [Header("Visual Settings")]
        public Color levelColor = Color.white;
        public Sprite levelIcon;
        
        [Header("Hints")]
        public string hintText;
    }
}