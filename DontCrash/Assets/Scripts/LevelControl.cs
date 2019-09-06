using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    //Initial spawn locations of cars {Bot Left x2, Top Left x2, Top Right x2, BotRight x2 }
    private float[] startingX = new float[] {-0.4f, -1.2f, -9.9f, -9.9f, 0.4f, 1.2f, 9.9f, 9.9f};
    private float[] startingZ = new float[] {-11.0f, -11.0f, 0.4f, 1.27f, 11.0f, 11.0f, -1.27f, -0.4f};
    private float[] rotationY = new float[] {0f,0f,90f,90f,180f,180f,270f,270f};
    private float startingY = 0.265f;

    private float spawnIntervalMax = 1.5f;
    private float spawnIntervalMin = 0.5f;


    //Car Prefabs
    public GameObject defaultCar;
    public GameObject copCar;
    public GameObject largeCar;
    private bool readyToSpawn = true;
    private Color[] carColor = new Color[] {new Color(0.02f,0.93f,1.0f,0.2f),new Color(0.14f,0.83f,0.19f,0.2f),new Color(0.78f,0.36f,0.34f,0.2f), Color.yellow};


    //Game controls
    public int score = 0;


    void Update()
    {
        if (readyToSpawn)
        {
            float timer = Random.Range(spawnIntervalMin, spawnIntervalMax);
            Invoke("SpawnCar", timer);
            readyToSpawn = false;
        }
    }

    void SpawnCar()
    {
        //Car starting position, salt is so they dont follow the same track
        int rand = UnityEngine.Random.Range(0,8);
        float salt = Random.Range(-0.1f,0.1f);

        //Random car type
        int randomCarType = Random.Range(0,11);       
        
        Quaternion direction = Quaternion.identity;
        direction.eulerAngles = new Vector3(0,rotationY[rand],0);

       
       // Change color of body of default car
        if (randomCarType <= 6){
            GameObject newCar = Instantiate(defaultCar, new Vector3(startingX[rand]+salt, startingY, startingZ[rand]+salt),direction);
            int randomCarColor = Random.Range(0,4);
            foreach (Transform child in newCar.transform){
                if (child.name == "Top" || child.name == "Bottom"){
                    child.GetComponent<Renderer>().material.color = carColor[randomCarColor];
                }
            }
        } else if (randomCarType <= 8){
            GameObject newCop = Instantiate(copCar, new Vector3(startingX[rand]+salt, startingY, startingZ[rand]+salt),direction);
        } else if (randomCarType <= 10){
            int randomCarColor = Random.Range(0,4);
            GameObject truck = Instantiate(largeCar, new Vector3(startingX[rand]+salt, startingY, startingZ[rand]+salt),direction);
            foreach (Transform child in truck.transform){
                if (child.name == "Top" || child.name == "Bottom"){
                    child.GetComponent<Renderer>().material.color = carColor[randomCarColor];
                }
            }
        }
       
        readyToSpawn = true;

    }
}
