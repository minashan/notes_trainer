using UnityEngine;

public class ForcePortrait : MonoBehaviour
{
    void Start()
    {
        // Принудительно ставим вертикальную ориентацию
        Screen.orientation = ScreenOrientation.Portrait;
    }
}