using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	private float cameraMovementSpeed = 2f;
	private float border = 5;
	private Vector3 defaultPos;
	private float smoothFactor = 2f;
	private bool defaulting = false;
	// Use this for initialization
	void Start ()
	{
		defaultPos = new Vector3 (50, 100, -10);
		this.gameObject.transform.position = defaultPos;
		this.gameObject.transform.eulerAngles = new Vector3 (90, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		bool running = GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SimulationScript> ().IsSimulationRunning ();
		if (!running) {
			Control ();
		}
		
		if(defaulting)
		{
			transform.position = Vector3.Lerp(transform.position,defaultPos,Time.deltaTime * smoothFactor);
			
			//If camera is near default, stop defaulting
			if(Vector3.Distance(transform.position,defaultPos) < 50f)
			{
				defaulting = false;
			}
		}
	}
	
	//Controls the camera around. 'cameraMovementSpeed' defines the camera speed, ofcourse.
	private void Control ()
	{
		//Left
		if (Input.mousePosition.x < Screen.width - border) {
			this.transform.Translate (-cameraMovementSpeed, 0, 0);
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			this.transform.Translate (-cameraMovementSpeed, 0, 0);
		}
		
		//Right
		if (Input.mousePosition.x > border) {
			this.transform.Translate (cameraMovementSpeed, 0, 0);
		}	
		if (Input.GetKey (KeyCode.RightArrow)) {
			this.transform.Translate (cameraMovementSpeed, 0, 0);
		}
		
		//Back
		if (Input.mousePosition.y > Screen.height - border) {
			this.transform.Translate (0, 0, cameraMovementSpeed, Space.World);
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			this.transform.Translate (0, 0, cameraMovementSpeed, Space.World);
		}
		
		//Toward
		if (Input.mousePosition.y < border) {
			this.transform.Translate (0, 0, -cameraMovementSpeed, Space.World);
		}	
		if (Input.GetKey (KeyCode.DownArrow)) {
			this.transform.Translate (0, 0, -cameraMovementSpeed, Space.World);
		}
		
		if(Input.GetKeyDown(KeyCode.Backspace))
		{
			defaulting = true;
		}
		
		//Controls the zooming in and out
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		gameObject.transform.Translate (new Vector3 (0, -cameraMovementSpeed * scroll * 25, 0), Space.World);
	}
}
