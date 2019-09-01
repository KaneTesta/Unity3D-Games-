using UnityEngine;
using UnityEngine.UI;
 
//Stolen from https://forum.unity.com/threads/fps-counter.505495/

public class FPSDisplay : MonoBehaviour
{
    public int avgFrameRate;
    public Text display_Text;
 
    public void Update ()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.text = avgFrameRate.ToString() + " FPS";
    }
}