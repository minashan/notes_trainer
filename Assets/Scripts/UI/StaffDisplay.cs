using UnityEngine;

public class StaffDisplay : MonoBehaviour
{
    [Header("Staff Sprites")]
    [SerializeField] private GameObject trebleStaff;
    [SerializeField] private GameObject bassStaff;
    
    private void Start()
    {
        // Загружаем выбранный ключ из PlayerPrefs напрямую
        string savedClef = PlayerPrefs.GetString("SelectedClef", "Treble");
        UpdateStaffDisplay(savedClef == "Treble");
    }
    
    private void UpdateStaffDisplay(bool isTreble)
    {
        if (trebleStaff != null)
            trebleStaff.SetActive(isTreble);
            
        if (bassStaff != null)
            bassStaff.SetActive(!isTreble);
    }
}