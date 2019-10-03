using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    public GameObject car;
    public GameObject truck;
    public GameObject cam;
    public GameObject skidManager;
    public GameObject sparkManager;
    public GameObject sparks;

    private GameObject carMain;
    private GameObject truckMain;
    private bool truckSpawned = false;
    private bool particlesSpawned = false;
    private float timer = 3.25f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCar());
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f){
            carMain.transform.Translate(Vector3.forward * 7f * Time.deltaTime);
        
            if (carMain.transform.position.z > 0f && carMain.transform.position.z < 11.25f){
                
                if (!truckSpawned){
                    truckSpawned = true;
                    truckMain = Instantiate(truck, new Vector3(-9.5f, 0.24f, .5f), Quaternion.Euler(0,90f,0));
                }

                Vector3 camPos = cam.transform.position;
                camPos.z -= 5f * Time.deltaTime;
                cam.transform.position = camPos;

                if (timer < 1f){
                    skidManager.GetComponent<SkidManage>().addSkid(truckMain);
                    skidManager.GetComponent<SkidManage>().addSkid(carMain);
                    HeadlightsOn();

                }

            }
            timer -= Time.deltaTime;
        } else if (!particlesSpawned){
            particlesSpawned = true;

            GameObject dash = carMain.transform.Find("Headlights").gameObject;
            Vector3 dashPos = dash.transform.position;
            dashPos.y += 0.5f;
            //Spawn Particle System
            GameObject s2 = Instantiate(sparks, dashPos, new Quaternion(0,0,0,0));
            s2.SetActive(true);
            s2.GetComponent<ParticleSystem>().Play();
            GameObject s3 = Instantiate(sparks, dashPos, new Quaternion(0,0,0,0));
            s3.SetActive(true);
            s3.GetComponent<ParticleSystem>().Play();

        }
    
    }
    IEnumerator SpawnCar(){
        enabled = false;
        yield return new WaitForSeconds(5f);
        carMain = Instantiate(car, new Vector3(0.4f, 0.125f, 23f), Quaternion.Euler(0,180f,0));
        enabled = true;

    }

    void HeadlightsOn(){
        foreach (Transform child in carMain.transform){
            if (child.name.Contains("Brake")){
                Debug.Log("H");
                foreach (Transform child2 in child.transform){
                    if (child2.name.Contains("Light")){
                        child2.Find("Spot Light").gameObject.SetActive(true);
                    }
                }
            }
        }
        foreach (Transform child in truckMain.transform){
            if (child.name.Contains("Brake")){
                foreach (Transform child2 in child.transform){
                    if (child2.name.Contains("Light")){
                        child2.Find("Spot Light").gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
