using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero,Vector3.left,3f*Time.deltaTime);
        transform.LookAt(Vector3.zero);
        bool nightMode = GameObject.Find("LevelController").GetComponent<LevelControl>().nightMode;

        if (!nightMode && transform.position.y < 0){
            GameObject.Find("LevelController").GetComponent<LevelControl>().nightMode = true;
        }

        if (nightMode && transform.position.y > 0){
            GameObject.Find("LevelController").GetComponent<LevelControl>().nightMode = false;
        }
    }
}
