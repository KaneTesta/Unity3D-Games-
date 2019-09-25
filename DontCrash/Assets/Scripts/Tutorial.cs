using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    private int car = 0;
    private bool clickOccured = false;
    public Text tutorialText;
    public Text tutorialText2;

    // Start is called before the first frame update
    void Start()
    {

        car = 0;
       
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        StartCoroutine(tutorial());
    }

    //Handle tutorial
    IEnumerator tutorial(){
        if (clickOccured == false){
            if (car == 1){
                Time.timeScale = 0f;
                tutorialText.text = "Left-click cars to speed them up";
                tutorialText2.text = "Left-click cars to speed them up";
                clickOccured = true;
                yield return new WaitForSeconds(3.0f);
            } else if (car == 2){
                Time.timeScale = 0f;
                tutorialText.text = "Right-click cars to stop them (Careful, police cars don't stop)";
                tutorialText2.text = "Right-click cars to stop them (Careful, police cars don't stop)";               
                clickOccured = true;
                yield return new WaitForSeconds(3.0f);
            } else if (car == 3){
                Time.timeScale = 1f;
                tutorialText.text = "Don't crash!";
                tutorialText2.text = "Don't crash!";
                clickOccured = true;
                yield return new WaitForSeconds(5.0f);
            } else if (car > 3){
                tutorialText.text = "";
                tutorialText2.text = "";
                car = 0;
                this.gameObject.SetActive(false);
            } else {
                object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
                foreach (GameObject o in obj){
                    if (o.name.Contains("Car") || o.name.Contains("Truck") || o.name.Contains("Police") && o.name.Contains("Clone")){
                        if (o.transform.position.x < 1.5 && o.transform.position.x > -1.5 && o.transform.position.z < 1.5 && o.transform.position.z > -1.5){
                            car += 1;
                            yield break;
                        }
                    }
                }
            }
        }
    }


    void CheckInput() {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            Time.timeScale = 1f;
            tutorialText.text = "";
            clickOccured = false;
            car += 1;
        }
    }
}