using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    //Car attributes that are altered as time goes on
    public float speedMultiplier = 1.0f;

    // Global car attributes
    private int speed = 1;
    private bool cantStop = false; // for cars that never can be stopped so player must prioritise getting them through

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = this.transform.position;
        newPos.z += speed*speedMultiplier*Time.deltaTime;
        this.transform.position = newPos;
    }
}