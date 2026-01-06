using UnityEngine;

public class ForceLandscape : MonoBehaviour
{
    void Start()
    {
       
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        
        Screen.autorotateToPortrait = false; 
        Screen.autorotateToPortraitUpsideDown = false; 
        
       
        Screen.autorotateToLandscapeLeft = true; 
        Screen.autorotateToLandscapeRight = true; 
        
        
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}