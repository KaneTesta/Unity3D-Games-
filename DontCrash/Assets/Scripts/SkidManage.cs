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
}
