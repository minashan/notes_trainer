using UnityEngine;
using UnityEngine.UI;

public class KeySelectionManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button trebleClefButton;
    [SerializeField] private Button bassClefButton;
    
    [Header("Bass Clef Lock")]
    [SerializeField] private GameObject bassLockedPanel;
    
    private void Start()
    {
        InitializeButtons();
    }
    
    private void InitializeButtons()
    {
        if (trebleClefButton != null)
        {
            trebleClefButton.onClick.AddListener(OnTrebleClefClicked);
        }
        
        if (bassClefButton != null)
        {
            bassClefButton.interactable = false;
            bassClefButton.onClick.AddListener(OnBassClefClicked);
        }
        
        if (bassLockedPanel != null)
        {
            bassLockedPanel.SetActive(true);
        }
        
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }
    
    private void OnTrebleClefClicked()
    {
        SceneNavigator.Instance?.LoadLevelSelection();
    }
    
    private void OnBassClefClicked()
    {
        // Bass clef is locked - do nothing
    }
    
    private void OnExitButtonClicked()
    {
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}