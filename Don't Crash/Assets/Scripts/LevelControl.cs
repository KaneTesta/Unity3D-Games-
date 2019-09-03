using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    //Initial spawn locations of cars {Bot Left x2, Top Left x2, Top Right x2, BotRight x2 }
    private float[] startingX = new float[] {-0.4f, -1.2f, -9.9f, -9.9f, 0.4f, 1.2f, 9.9f, 9.9f};
    private float[] startingZ = new float[] {-11.0f, -11.0f, 0.4f, 1.27f, 11.0f, 11.0f, -1.27f, -0.4f};
    private float[] rotationY = new float[] {0f,0f,90f,90f,180f,180f,270f,270f};
    private float startingY = 0.35f;

    public float spawnIntervalMax = 0.1f;
    public float spawnIntervalMin = 2.0f;


    //Car Prefabs
    public GameObject defaultCar;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnCar", spawnIntervalMax, spawnIntervalMax);
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
    }
}
