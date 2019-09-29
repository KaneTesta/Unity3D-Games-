using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOver: MonoBehaviour
{
    public GameObject GameOverUI;
    public GameObject MainMenuUI;

    private Color red = Color.red;
    private Color blue = new Color(0.03f,0.6f,1f,1f);
    private Color white = Color.white;
    private int upTo = 0;
    public TMPro.TextMeshProUGUI bg;
    public Text score;

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

    public void Restart(){
        GameOverUI.SetActive(false);
        GameObject.Find("LevelController").GetComponent<LevelControl>().ResetGame();
    }

    public void ShowMainMenu(){
        MainMenuUI.SetActive(true);
        GameOverUI.SetActive(false);
        GameObject.Find("LevelController").GetComponent<LevelControl>().ResetGame();
    }

    public void ShowGameOverMenu(){
        
        GameOverUI.SetActive(true);
        GameObject levelControl = GameObject.Find("LevelController");
        int points = levelControl.GetComponent<LevelControl>().score;
        
        score.text = "Your score was " + points.ToString();
        if (points > PlayerPrefs.GetInt("HighScore",0)){
            score.text += ". You beat your high score!";
            PlayerPrefs.SetInt("HighScore", points);
        } else if (points > 100){
            score.text += ". Wow! You're great at this.";
        } else if (points > 75) {
            score.text += ". Great!";
        } else if (points > 50) {
            score.text += ". You can do better!!";
        } else if (points > 30) {
            score.text += ". Can we forget that round ever happened?";
        } else if (points > 15) {
            score.text += ". It's probably harder to do that badly than to do good.";
        } else {
            score.text += ". That was embarassing...";
        }

    }
}
