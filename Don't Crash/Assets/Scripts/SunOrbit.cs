using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunOrbit : MonoBehaviour
{

    private float orbitRadius = 20.0f; 
    public float rotateSpeed = 0.1f;
    private float timeCount = 0;
    public bool nightMode = false;

    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(orbitRadius,0.0f,0.0f);  
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += (Time.deltaTime*rotateSpeed);
        float x = Mathf.Cos(timeCount)*orbitRadius;
        float y = Mathf.Sin(timeCount)*orbitRadius;

        if (y>0 && nightMode == true){
            nightMode = false;
        } else if (y<0 && nightMode == false){
            nightMode = true;
        }
        transform.position = new Vector3(x,y,0.0f);
    }

    public Vector3 GetPosition() {
        return this.transform.position;
    }
}
