using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{

    //Level Controller
    private GameObject controller;
    private GameObject SkidManager;
    private GameObject sparks;
    private GameObject ex;
    private bool gameOver;


    //Car attributes that are altered as time goes on
    public float speedMultiplier = 1.0f;

    //Map attributes
    private int mapWidth = 20;

    // Global car attributes
    private float speed;
    private int initSpeed = 7;
    private bool cantStop = false; // for cars that never can be stopped so player must prioritise getting them through
    private int zCoEff;
    private int xCoEff;
    private float initX;
    private float initZ;

    //Controls
    private float boostVal = 10f;
    private bool speedingUp = false;
    
    private float stoppedTimer = 0.0f;
    private bool stoppedCar = false;
    private bool haveHonked = false;



    // Start is called before the first frame update
    void Start()
    {
        speed = initSpeed;
        controller = GameObject.Find("LevelController");
        SkidManager = GameObject.Find("SkidManager");
        sparks = GameObject.Find("Sparks");
        zCoEff = zDirection(this.transform.eulerAngles);
        xCoEff = xDirection(this.transform.eulerAngles);
        initX = this.transform.position.x;
        initZ = this.transform.position.z;

        if (GameObject.Find("LevelController").GetComponent<LevelControl>().nightMode){
            SkidManager.GetComponent<SkidManage>().headLightsOn(this.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = this.transform.position;
        newPos.x = newPos.x + xCoEff*speed*speedMultiplier*Time.deltaTime;
        newPos.z = newPos.z + zCoEff*speed*speedMultiplier*Time.deltaTime;
        this.transform.position = newPos;

        WheelRotate();
        StoppedCheck();
        SpeedManager();
        killCheck();
    }

    void WheelRotate(){
        foreach (Transform child in this.transform){
            if (child.name == "Wheels"){
                foreach(Transform child2 in child.transform){
                    if (child2.name == "Wheel"){
                        Vector3 rot = child2.rotation.eulerAngles;
                        rot.z += Time.deltaTime * 500 * speed;
                        child2.rotation = Quaternion.Euler(rot);
                    }
                }
            }
        }
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
        if (this.GetComponent<AudioSource>().isPlaying == false){
            if (this.transform.position.y < 0f){
                Destroy(this.gameObject);
                controller.GetComponent<LevelControl>().addPoint();
            }
        }
    }

    void OnMouseOver () {
        if (Input.GetMouseButtonDown(0) && !gameOver) {
            if (speedingUp == false){
                GameObject.Find("AudioManager").GetComponent<AudioManager>().RevSound(this.gameObject);
            }
            speedingUp = true;
            stoppedCar = false;
            SkidManager.GetComponent<SkidManage>().addParticles(this.gameObject);
            SkidManager.GetComponent<SkidManage>().brakeLightsOff(this.gameObject);
            if (ex != null){
                Destroy(ex);
            }
        }
        if (Input.GetMouseButtonDown(1) && cantStop == false) {
            if (stoppedCar == false){
                GameObject.Find("AudioManager").GetComponent<AudioManager>().SkidSound(this.gameObject);
            }
            stoppedTimer = 0.0f;
            stoppedCar = true;
            speedingUp = false;
            haveHonked = false;
            SkidManager.GetComponent<SkidManage>().brakeLightsOn(this.gameObject);

        }
    }
    void SpeedManager() {
        //If car is stopped
        if (stoppedCar && speed > 0){
            speed -= 0.5f;
            SkidManager.GetComponent<SkidManage>().addSkid(this.gameObject);
        } 

        //If car is boosted and speeding up
        else if ((speed < initSpeed + boostVal) && speedingUp){
            speed += 0.5f;
        } 

        //if car is slowing after boost
        else if (speed > initSpeed){
            speed -= 0.5f;
            speedingUp = false;
        }
    }
    void StoppedCheck() {
        if (stoppedCar){
            stoppedTimer += Time.deltaTime;
        }

        if (stoppedTimer > 2.0f && !haveHonked){
            GameObject.Find("AudioManager").GetComponent<AudioManager>().HonkSound(this.gameObject); 
            haveHonked = true;
            if (this.transform.Find("Top").Find("Exclamation(Clone)") == null){
                ex = Instantiate(controller.GetComponent<LevelControl>().exclamationMark, this.transform.Find("Top"), false); 
            }
        }


        if (stoppedTimer > 3.0f && stoppedCar){
            stoppedCar = false;
            speedingUp = true;
            SkidManager.GetComponent<SkidManage>().brakeLightsOff(this.gameObject);
            Destroy(ex);
        }
    }

    void OnCollisionEnter(Collision collisionInfo){
        bool collided = false;
        if (collisionInfo.collider.name.Contains("Car")){
            collided = true;
        }
        if (collisionInfo.collider.name.Contains("Truck")){
            collided = true;
        }
        if (collisionInfo.collider.name.Contains("Police")){
            collided = true;
        }
            
            
        if (collided == true){
            stoppedCar = true;
            speed = 0;
            enabled = false;
            gameOver = true;
            controller.GetComponent<LevelControl>().GameOver();
            GameObject.Find("AudioManager").GetComponent<AudioManager>().CrashSound(this.gameObject); 

            //Spawn Particle System
            ContactPoint collisionPoint = collisionInfo.contacts[0];
            GameObject s2 = Instantiate(sparks, collisionPoint.point, new Quaternion(0,0,0,0));
            s2.SetActive(true);
            s2.GetComponent<ParticleSystem>().Play();
            controller.GetComponent<LevelControl>().cameraShake();
        }
    }

}