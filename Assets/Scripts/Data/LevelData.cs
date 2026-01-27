using UnityEngine;

namespace NotesTrainer
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Notes Trainer/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Основные настройки")]
        public string levelName = "Новый уровень";
        [TextArea] public string description = "Описание уровня";
        public int requiredScore = 50; // очков для перехода
        
        [Header("Ноты в уровне")]
        public string[] includedNotes;
        
        [Header("Дополнительные настройки")]
        public bool allowEnharmonic = false; // учитывать энгармонизм?
        public int timeLimit = 0; // 0 = без ограничения времени
        
        [Header("Визуальные настройки")]
        public Color levelColor = Color.white;
        public Sprite levelIcon;
        
        [Header("Подсказки")]
        public string hintText = "";
    }
}