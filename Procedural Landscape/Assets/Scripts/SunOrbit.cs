using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunOrbit : MonoBehaviour
{

    public GameObject terrain;
    private float orbitRadius = 0; 
    public float rotateSpeed = 0.1f;
    private float timeCount = 0;

    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        orbitRadius = 0.75f * (terrain.GetComponent<DiamondSquareTerrain>().width); 
        transform.position = new Vector3(orbitRadius,0.0f,0.0f);  
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += (Time.deltaTime*rotateSpeed);
        float x = Mathf.Cos(timeCount)*orbitRadius;
        float y = Mathf.Sin(timeCount)*orbitRadius;
        transform.position = new Vector3(x,y,0.0f);
    }

    public Vector3 GetPosition() {
        return this.transform.position;
    }
}
