using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    //Level Controller
    private GameObject controller;

    //Car attributes that are altered as time goes on
    public float speedMultiplier = 1.0f;

    //Map attributes
    private int mapWidth = 20;

    // Global car attributes
    private int speed = 5;
    private bool cantStop = false; // for cars that never can be stopped so player must prioritise getting them through
    private int zCoEff;
    private int xCoEff;
    private float initX;
    private float initZ;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("LevelController");
        zCoEff = zDirection(this.transform.eulerAngles);
        xCoEff = xDirection(this.transform.eulerAngles);
        initX = this.transform.position.x;
        initZ = this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = this.transform.position;
        newPos.x = newPos.x + xCoEff*speed*speedMultiplier*Time.deltaTime;
        newPos.z = newPos.z + zCoEff*speed*speedMultiplier*Time.deltaTime;
        this.transform.position = newPos;

        killCheck();
    }

    int zDirection(Vector3 rot){
        int coEff = 0;
        if (rot.y == 180f) {
            coEff = -1;
        } else if (rot.y == 0f) {
            coEff = 1;
        } 
        return coEff;
    }

    int xDirection(Vector3 rot){
        int coEff = 0;
        if (rot.y == 90f) {
            coEff = 1;
        } else if (rot.y == 270f) {
            coEff = -1;
        }
        return coEff;    
    }

    //Kill car when out of bounds and add point
    void killCheck(){
        //ADD GLOBAL POINT ADDITION TO LEVEL CONTROLLER
        
        if (xCoEff == 1){
            if (this.transform.position.x > initX + mapWidth){
                Destroy(this.gameObject);
                addPoint();
            }
        } else if (xCoEff == -1){
            if (this.transform.position.x < initX - mapWidth){
                Destroy(this.gameObject);
                addPoint();
            }
        } else if (zCoEff == 1){
            if (this.transform.position.z > initZ + mapWidth){
                Destroy(this.gameObject);
                addPoint();
            }
        } else if (zCoEff == -1){
            if (this.transform.position.z < initZ - mapWidth){
                Destroy(this.gameObject);
                addPoint();
            }
        }
    }

    void addPoint(){
        controller.GetComponent<LevelControl>().score += 1;
    }

}