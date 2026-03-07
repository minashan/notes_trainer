using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [Header("Loading Settings")]
    [SerializeField] private float loadingTime = 2f;
    [SerializeField] private bool skipButtonEnabled;
    
    private const string SCENE_NAVIGATOR_NAME = "SceneNavigator";
    
    private void Start()
    {
        EnsureSceneNavigator();
        Invoke(nameof(LoadKeySelection), loadingTime);
        
        if (skipButtonEnabled)
        {
            SetupSkipButton();
        }
    }
    
    private void EnsureSceneNavigator()
    {
        if (SceneNavigator.Instance != null) return;
        
        GameObject navigatorObj = new(SCENE_NAVIGATOR_NAME);
        navigatorObj.AddComponent<SceneNavigator>();
    }
    
    private void LoadKeySelection()
    {
        SceneNavigator.Instance?.LoadKeySelection();
    }
    
    private void SetupSkipButton()
    {
        // Reserved for future implementation
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            CancelInvoke(nameof(LoadKeySelection));
            LoadKeySelection();
        }
    }
}