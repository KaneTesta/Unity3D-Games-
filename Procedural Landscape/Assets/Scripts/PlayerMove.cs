using UnityEngine;
using System.Collections;
 
public class PlayerMove : MonoBehaviour
{
	//Cam Stuff
	public float sensitivity = 90;
	public float moveSpeed = 25f;
	public float lookSpeed = 5f;

	private float cameraOffset;

	private Vector2 rotation = new Vector2(0,0);



	//Collision Stuff
	private float terrainWidth;
	private float terrainHeight;
	public GameObject terrain;
	public Rigidbody rb; 


 
	void Start ()
	{

		//Terrain dimensions
		terrainWidth = ((terrain.GetComponent<DiamondSquareTerrain>().width)/2) - 3;
		terrainHeight = (terrain.GetComponent<DiamondSquareTerrain>().height);

		// MAKE THE CAMERA START BY FACING THE TERRAIN
		cameraOffset = 3.0f*terrainHeight;
		transform.position = new Vector3 (terrainWidth, cameraOffset + terrainHeight, terrainWidth);
		transform.LookAt(terrain.transform);
		//Camera Rigid Body
		rb = GetComponent<Rigidbody>();

	}
 
 	void Update ()
	{
 
		//Make camera look around based on mouse input
		float yaw = Input.GetAxis("Mouse X") * lookSpeed;
		float pitch = - Input.GetAxis("Mouse Y") * lookSpeed;

		yaw += transform.eulerAngles.y;
		pitch += transform.eulerAngles.x; 

		//Stops glitching at start
		if (pitch <= 180.0f) {
            pitch = Mathf.Min(pitch, 90.0f);
        } else {
            pitch = Mathf.Max(pitch, 270.0f);
        }

		transform.eulerAngles = new Vector3(pitch, yaw, 0);
		
		//Move around based on keyboard input
		transform.position += transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
		transform.position += transform.right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		
		//Keep within boundaries of terrain
		if (transform.position.x > terrainWidth){
			transform.position = new Vector3(terrainWidth, transform.position.y, transform.position.z);
		} else if (transform.position.x < -terrainWidth){
			transform.position = new Vector3(-terrainWidth, transform.position.y, transform.position.z);
		} else if (transform.position.z > terrainWidth){
			transform.position = new Vector3(transform.position.x, transform.position.y, terrainWidth);
		} else if (transform.position.z < -terrainWidth){
			transform.position = new Vector3(transform.position.x, transform.position.y, -terrainWidth);
		}


		//Stop collision from rotating or translating the camera 
		rb.angularVelocity = Vector3.zero; 
		rb.velocity = Vector3.zero; 
	}
}