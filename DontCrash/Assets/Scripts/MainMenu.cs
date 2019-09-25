using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu: MonoBehaviour
{
    public GameObject MenuUI;
    public GameObject ControlsUI;
    public GameObject Tutorial;

    private Color red = Color.red;
    private Color blue = new Color(0.03f,0.6f,1f,1f);
    private Color white = Color.white;
    private int upTo = 0;
    public TMPro.TextMeshProUGUI bg;

    public void Start(){
        float timer = 0.15f;
        InvokeRepeating("ColorChange", 0f, timer);
    }

    //Change Colour Gradient to resemble police sirens
    void ColorChange(){
        Color[] colors = new Color[] {red, white, blue, white};
        bg.color = colors[upTo];
        upTo += 1;

        if (upTo == 4){
            upTo = 0;
        }

    }

    public void ResetHighScore(){
        PlayerPrefs.DeleteAll();
    }

    public void Controls(){
        MenuUI.SetActive(false);
        ControlsUI.SetActive(true);
    }


    public void StartGame(){
        MenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameObject.Find("LevelController").GetComponent<LevelControl>().ResetGame();
        if (PlayerPrefs.GetInt("TutorialDone", 0) == 0){
            PlayerPrefs.SetInt("TutorialDone", 1);
            Tutorial.SetActive(true);
        }
    }

    public void ShowMenu(){
        Time.timeScale = 1f;
        MenuUI.SetActive(true);
        ControlsUI.SetActive(false);
    }
}
