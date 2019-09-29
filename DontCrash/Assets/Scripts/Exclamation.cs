using UnityEngine;

public class Exclamation : MonoBehaviour
{
    private float velocity = 150f;
    private float lifespan = 1.5f;

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(transform.position, transform.up, Time.deltaTime * velocity);
        lifespan -= Time.deltaTime;
    }
}
