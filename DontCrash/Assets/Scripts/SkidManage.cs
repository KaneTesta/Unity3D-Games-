using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidManage : MonoBehaviour
{
    public GameObject skid;

    public void addSkid(GameObject car){
        foreach (Transform child in car.transform){
            if (child.name == "Wheels"){
                foreach(Transform child2 in child.transform){
                    if (child2.name == "Wheel"){
                        Instantiate(skid, new Vector3(child2.transform.position.x, 0.14f,child2.transform.position.z), new Quaternion(0,0,0,0), this.transform);
                    }
                }
            }
        }
    }

    public void addParticles(GameObject car){
        foreach (Transform child in car.transform){
            if (child.name.Contains("Trail")){
                child.gameObject.SetActive(true);
            }
        }
    }
    //Turn on brake spotlight
    public void brakeLightsOn(GameObject car){
        GameObject lights = car.transform.Find("BrakeLights").gameObject;
        foreach (Transform child in lights.transform){
            foreach(Transform spotlight in child.transform){
                spotlight.gameObject.SetActive(true);
            }
        }
    }

    //Turn on brake spotlight
    public void brakeLightsOff(GameObject car){
        GameObject lights = car.transform.Find("BrakeLights").gameObject;
        foreach (Transform child in lights.transform){
            foreach(Transform spotlight in child.transform){
                spotlight.gameObject.SetActive(false);
            }
        }
    }

    public void headLightsOn(GameObject car){
        GameObject lights = car.transform.Find("Headlights").gameObject;
        foreach (Transform child in lights.transform){
            foreach(Transform spotlight in child.transform){
                spotlight.gameObject.SetActive(true);
            }
        }
    }

    //Turn on brake spotlight
    public void headLightsOff(GameObject car){
        GameObject lights = car.transform.Find("Headlights").gameObject;
        Debug.Log(lights);
        foreach (Transform child in lights.transform){
            foreach(Transform spotlight in child.transform){
                spotlight.gameObject.SetActive(false);
            }
        }
    }
}
