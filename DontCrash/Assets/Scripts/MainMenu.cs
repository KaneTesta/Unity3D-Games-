using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu: MonoBehaviour
{
    public GameObject MenuUI;
    public GameObject ControlsUI;
    public GameObject Tutorial;
    public GameObject ProgressUI;
    public Text progressText;


    private Color red = Color.red;
    private Color blue = new Color(0.03f,0.6f,1f,1f);
    private Color white = Color.white;
    private int upTo = 0;
    public TMPro.TextMeshProUGUI bg;

    public GameObject cam;

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
        PlayerPrefs.SetInt("TotalScore",0);
    }

    public void Controls(){
        MenuUI.SetActive(false);
        ControlsUI.SetActive(true);
    }

    public void Progress(){
        PlayerPrefs.SetInt("Progress",1);
        MenuUI.SetActive(false);
        ProgressUI.SetActive(true);
        GameObject.Find("LevelController").GetComponent<LevelControl>().ClearWorld();
        GameObject.Find("LevelController").GetComponent<LevelControl>().scoreText.text = "Your score: " + PlayerPrefs.GetInt("TotalScore").ToString();
        
        Vector3 textPos = GameObject.Find("LevelController").GetComponent<LevelControl>().scoreText.transform.position;
        textPos.x -= 100f;


        GameObject.Find("LevelController").GetComponent<LevelControl>().scoreText.transform.position = textPos;
        Quaternion direction = Quaternion.identity;
        direction.eulerAngles = new Vector3(0,135f,0);
        GameObject[] vehicles;

        // Rotate and Move camera
        cam.GetComponent<CamMove>().pers = true;
        cam.GetComponent<CamMove>().orth = false;

        // Vehicles that are unlocked spawn with lights on

        // If you've unlocked all vehicles, print congratulations or something

        if (PlayerPrefs.GetInt("TotalScore") < 100) {
            progressText.text = "Your next vehicle is a POLICE CAR. You are " + (100 - PlayerPrefs.GetInt("TotalScore")).ToString() + " points away";
            vehicles = new GameObject[1];
            //Spawn Normal Car - Append
            vehicles[0] = Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().defaultCar, new Vector3(6f,1f,0), direction);
        }  else if (PlayerPrefs.GetInt("TotalScore") < 300) {
            progressText.text = "Your next vehicle is a TRUCK. You are " + (300 - PlayerPrefs.GetInt("TotalScore")).ToString() + " points away";
            vehicles = new GameObject[2];
            //Spawn Normal Car  - Append
            vehicles[0] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().defaultCar, new Vector3(6f,1f,0), direction));
            //Spawn Cop Car 
            vehicles[1] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().copCar, new Vector3(7f,1f,0), direction));

        } else if (PlayerPrefs.GetInt("TotalScore") < 500) {
            progressText.text = "Your next vehicle is a TAXI. You are " + (500 - PlayerPrefs.GetInt("TotalScore")).ToString() + " points away";
            vehicles = new GameObject[3];
            //Spawn Normal Car  - Append
            vehicles[0] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().defaultCar, new Vector3(6f,1f,0), direction));
            //Spawn Cop Car 
            vehicles[1] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().copCar, new Vector3(7f,1f,0), direction));
            //Spawn Truck
            vehicles[2] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().largeCar, new Vector3(5f,1f,0), direction));

        } else if (PlayerPrefs.GetInt("TotalScore") < 700) {
            progressText.text = "Your next vehicle is a JEEP. You are " + (700 - PlayerPrefs.GetInt("TotalScore")).ToString() + " points away";
            vehicles = new GameObject[4];
            //Spawn Normal Car  - Append
            vehicles[0] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().defaultCar, new Vector3(6f,1f,0), direction));
            //Spawn Cop Car 
            vehicles[1] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().copCar, new Vector3(7f,1f,0), direction));
            //Spawn Truck
            vehicles[2] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().largeCar, new Vector3(5f,1f,0), direction));
            //Spawn Taxi
            vehicles[3] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().cab, new Vector3(4f,1f,0), direction));
        } else {
            progressText.text = "Your next vehicle is a JEEP. You are " + (700 - PlayerPrefs.GetInt("TotalScore")).ToString() + " points away";
            vehicles = new GameObject[5];
            //Spawn Normal Car  - Append
            vehicles[0] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().defaultCar, new Vector3(6f,1f,0), direction));
            //Spawn Cop Car 
            vehicles[1] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().copCar, new Vector3(7f,1f,0), direction));
            //Spawn Truck
            vehicles[2] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().largeCar, new Vector3(5f,1f,0), direction));
            //Spawn Taxi
            vehicles[3] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().cab, new Vector3(4f,1f,0), direction));
            //Spawn Jeep
            vehicles[4] = (Instantiate(GameObject.Find("LevelController").GetComponent<LevelControl>().jeep, new Vector3(8f,1f,0), direction));
        }

        //Turn all headlights on
        foreach (GameObject i in vehicles){
            GameObject lights = i.transform.Find("Headlights").gameObject;
            
            if (i.name.Contains("Police")){
                i.GetComponent<CopCar>().initSpeed = 0;
                i.GetComponent<CopCar>().enabled = false;
            } else if (i.name.Contains("Car") || i.name.Contains("Taxi") || i.name.Contains("Jeep")){
                i.GetComponent<Car>().initSpeed = 0;
                i.GetComponent<Car>().enabled = false;
            } else {
                i.GetComponent<Truck>().initSpeed = 0;
                i.GetComponent<Truck>().enabled = false;
            }
        }
    }

    public void LeaveProgressMenu(){
        PlayerPrefs.SetInt("Progress",0);
        MenuUI.SetActive(true);
        ProgressUI.SetActive(false);
        GameObject.Find("LevelController").GetComponent<LevelControl>().ResetGame();
        GameObject.Find("LevelController").GetComponent<LevelControl>().readyToSpawn = true;

        // Rotate and Move camera
        cam.GetComponent<CamMove>().pers = false;
        cam.GetComponent<CamMove>().orth = true;

        //Move back to OG spot in oppostite function
        Vector3 textPos = GameObject.Find("LevelController").GetComponent<LevelControl>().scoreText.transform.position;
        textPos.x += 100f;

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
