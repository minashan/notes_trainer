using UnityEngine;
using NotesTrainer;

public class PianoPanelSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject treblePiano;
    [SerializeField] private GameObject bassPiano;
    
    private void Start()
    {
        ClefType currentClef = SceneNavigator.Instance.LoadSelectedClef();
        UpdatePianoPanel(currentClef);
    }
    
    private void UpdatePianoPanel(ClefType clef)
    {
        if (treblePiano != null)
            treblePiano.SetActive(clef == ClefType.Treble);
            
        if (bassPiano != null)
            bassPiano.SetActive(clef == ClefType.Bass);
    }
}