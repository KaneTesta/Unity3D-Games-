using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    private int score = 0;
    private int lastScore = 0;
    private bool inZone = false;
    private bool scoreChanged = true;

    public Text tutorialText;
    public Text tutorialText2;

    public bool tut = false;

    void Start(){
        if (PlayerPrefs.GetInt("HighScore",0)==0){
            tut = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (lastScore != score){
            scoreChanged = true;
            lastScore = score;
        }
        
        tutorial();
        CheckInput();
    }

    //Handle tutorial
    void tutorial(){
        if (inZone == true && scoreChanged && tut && !GameObject.Find("MenuControllers").GetComponent<MainMenu>().MenuUI.activeSelf){
            if ((score+1) == 1){
                Time.timeScale = 0f;
                tutorialText.text = "Left-click cars to speed them up";
                tutorialText2.text = "Left-click cars to speed them up";
            } else if ((score+1) == 2){
                Time.timeScale = 0f;
                tutorialText.text = "Right-click cars to stop them (Careful, police cars don't stop)";
                tutorialText2.text = "Right-click cars to stop them (Careful, police cars don't stop)";               
            } else if ((score+1) == 3){
                Time.timeScale = 1f;
                tutorialText.text = "'Esc' or 'P' will pause.";
                tutorialText2.text = "'Esc' or 'P' will pause.";
            } else if ((score+1) == 4){
                Time.timeScale = 1f;
                tutorialText.text = "Don't crash!";
                tutorialText2.text = "Don't crash!";
            } else if ((score+1) > 4){
                tutorialText.text = "";
                tutorialText2.text = "";
                this.gameObject.SetActive(false);
            } 
            scoreChanged = false;
            inZone = false;
        } else if (score < 2){
            object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
            foreach (GameObject o in obj){
                if (o.name.Contains("Car") || o.name.Contains("Truck") || o.name.Contains("Police") || o.name.Contains("Taxi") || o.name.Contains("Jeep") && o.name.Contains("Clone")){
                    if (o.transform.position.x < 1.5 && o.transform.position.x > -1.5 && o.transform.position.z < 1.5 && o.transform.position.z > -1.5){
                        score = GameObject.Find("LevelController").GetComponent<LevelControl>().score;
                        inZone = true;
                        break;
                    }
                }
            }
        }
    }


    void CheckInput() {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            Time.timeScale = 1f;
            tutorialText.text = "";
            tutorialText2.text = "";
            score = GameObject.Find("LevelController").GetComponent<LevelControl>().score;
        }
    }
}