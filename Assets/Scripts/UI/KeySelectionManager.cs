using UnityEngine;
using UnityEngine.UI;

public class KeySelectionManager : MonoBehaviour
{
    [SerializeField] private Button trebleClefButton;
    [SerializeField] private Button bassClefButton;
    [SerializeField] private GameObject bassLockedPanel;
    
    
    void Start()
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
        
    }
    
    void OnTrebleClefClicked()
    {
        Debug.Log("Скрипичный ключ выбран");
        SceneNavigator.Instance.LoadLevelSelection();
    }
    
    void OnBassClefClicked()
    {
        Debug.Log("Басовый ключ заблокирован");
    }
}