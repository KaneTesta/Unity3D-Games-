using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;
    Quaternion currentAngle;
    public bool orth = true;
    public bool pers = false;

    void Start(){
        currentAngle = views[0].rotation;
        currentView = views[0];
    }

    void Update(){
        if (pers){
            currentView = views[1];
            currentAngle = views[1].rotation;
            ChangeProjection();
            pers = false;
        }

        if (orth){
            currentView = views[0];
            currentAngle = views[0].rotation;
            ChangeProjection();
            orth = false;
        }
    }

    void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime*transitionSpeed);
        Vector3 ang = new Vector3(
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentAngle.eulerAngles.x, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentAngle.eulerAngles.y, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentAngle.eulerAngles.z, Time.deltaTime * transitionSpeed));
        transform.rotation = Quaternion.Euler(ang);
    }

    void ChangeProjection(){
        if (currentView == views[0]){
            this.GetComponent<Camera>().orthographic = true;
            this.GetComponent<Camera>().nearClipPlane = -4;
        } else {
            this.GetComponent<Camera>().nearClipPlane = 0.1f;
            this.GetComponent<Camera>().orthographic = false;
        }

    }
}
