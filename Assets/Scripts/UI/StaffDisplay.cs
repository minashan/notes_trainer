using UnityEngine;

public class StaffDisplay : MonoBehaviour
{
    [SerializeField] private GameObject trebleStaff;
    [SerializeField] private GameObject bassStaff;
    
    private void Start()
{
    string savedClef = PlayerPrefs.GetString("SelectedClef", "Treble");
    bool isTreble = savedClef == "Treble";
    
    Debug.Log($"StaffDisplay: savedClef={savedClef}, isTreble={isTreble}");
    Debug.Log($"trebleStaff assigned: {trebleStaff != null}, bassStaff assigned: {bassStaff != null}");
    
    if (trebleStaff != null)
    {
        trebleStaff.SetActive(isTreble);
        Debug.Log($"trebleStaff active set to: {isTreble}");
    }
    
    if (bassStaff != null)
    {
        bassStaff.SetActive(!isTreble);
        Debug.Log($"bassStaff active set to: {!isTreble}");
    }
}
}