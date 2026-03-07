using UnityEngine;

public class ForceLandscape : MonoBehaviour
{
    private void Start()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}