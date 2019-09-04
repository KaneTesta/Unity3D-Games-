using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    //Initial spawn locations of cars {Bot Left x2, Top Left x2, Top Right x2, BotRight x2 }
    private float[] startingX = new float[] {-0.4f, -1.2f, -9.9f, -9.9f, 0.4f, 1.2f, 9.9f, 9.9f};
    private float[] startingZ = new float[] {-11.0f, -11.0f, 0.4f, 1.27f, 11.0f, 11.0f, -1.27f, -0.4f};
    private float[] rotationY = new float[] {0f,0f,90f,90f,180f,180f,270f,270f};
    private float startingY = 0.229f;

    private float spawnIntervalMax = 3.0f;
    private float spawnIntervalMin = 1.0f;


    //Car Prefabs
    public GameObject defaultCar;
    private bool readyToSpawn = true;
    private Color[] carColor = new Color[] {new Color(0.02f,0.93f,1.0f,0.2f),new Color(0.14f,0.83f,0.19f,0.2f),new Color(0.78f,0.36f,0.34f,0.2f)};


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
        // CONFIGURE -- Random spawning point
        int rand = UnityEngine.Random.Range(0,8);

        //CONFIGURE -- Random car type
        int randomCarType = 0;       

        Quaternion direction = Quaternion.identity;
        direction.eulerAngles = new Vector3(0,rotationY[rand],0);

        GameObject newCar = Instantiate(defaultCar, new Vector3(startingX[rand], startingY, startingZ[rand]),direction);
       
       // Change color of body of default car
        if (randomCarType == 0){
            int randomCarColor = Random.Range(0,3);
            foreach (Transform child in newCar.transform){

                if (child.name == "Top" || child.name == "Bottom"){
                    child.GetComponent<Renderer>().material.color = carColor[randomCarColor];
                }
            }
        }
       
        readyToSpawn = true;

    }
}
