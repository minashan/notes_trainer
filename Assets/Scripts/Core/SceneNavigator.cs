using UnityEngine;
using UnityEngine.SceneManagement;
using NotesTrainer; // 🔥 Добавляем namespace для ClefType

public class SceneNavigator : MonoBehaviour
{
    private static SceneNavigator _instance;
    public static SceneNavigator Instance => _instance;
    
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    private const string SELECTED_CLEF_KEY = "SelectedClef"; // 🔥 Новый ключ
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // Scene loading methods
    public void LoadLoadingScene() => LoadScene("0_Loading");
    public void LoadKeySelection() => LoadScene("1_KeySelection");
    public void LoadLevelSelection() => LoadScene("2_LevelSelection");
    public void LoadGameScene() => LoadScene("3_Game");
    public void LoadPianoScene() => LoadScene("4_PianoMode");


    public void LoadPianoModeScene()
{
    SceneManager.LoadScene("4_PianoMode");
}


    
   public void LoadGameWithLevel(int levelIndex, ClefType clef)
{
    if (levelIndex < 1 || levelIndex > 8) return;
    
    // Сохраняем выбранный уровень
    string currentLevelKey = clef == ClefType.Treble ? "TrebleCurrentLevel" : "BassCurrentLevel";
    PlayerPrefs.SetInt(currentLevelKey, levelIndex);
    
    // Сохраняем высший доступный уровень для этого ключа
    string highestLevelKey = clef == ClefType.Treble ? "TrebleHighestLevel" : "BassHighestLevel";
    int currentHighest = PlayerPrefs.GetInt(highestLevelKey, 1);
    if (levelIndex > currentHighest)
    {
        PlayerPrefs.SetInt(highestLevelKey, levelIndex);
    }
    
    SaveSelectedClef(clef);
    PlayerPrefs.Save();
    
    LoadGameScene();
}
    
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void RestartCurrentLevel()
{
    int currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 1);
    ClefType currentClef = LoadSelectedClef(); // 🔥 получаем текущий ключ
    LoadGameWithLevel(currentLevel, currentClef); // 🔥 передаём оба параметра
}
    
    public void ReturnToLevelSelection()
    {
        LoadLevelSelection();
    }
    
    // 🔥 Новые методы для работы с ключом
    public void SaveSelectedClef(ClefType clef)
    {
        PlayerPrefs.SetString(SELECTED_CLEF_KEY, clef.ToString());
        PlayerPrefs.Save();
    }
    
    public ClefType LoadSelectedClef()
    {
        string savedClef = PlayerPrefs.GetString(SELECTED_CLEF_KEY, ClefType.Treble.ToString());
        
        if (System.Enum.TryParse(savedClef, out ClefType result))
            return result;
        
        return ClefType.Treble;
    }
}