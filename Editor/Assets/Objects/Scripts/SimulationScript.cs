using UnityEngine;
using System.Collections;

public class SimulationScript : MonoBehaviour
{
	private GameObject mainCamera;
	private GameObject simCar;
	private GameObject instantiatedCar;
	private GameObject worldgrass;
	public bool isSimulationRunning = false;
	private AreaFile areaFile = new AreaFile(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
	
	// Use this for initialization
	void Start ()
	{	
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		simCar = Resources.Load ("drivingCar") as GameObject;
		instantiatedCar = GameObject.Instantiate (simCar) as GameObject;
		GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setCarGameObject (instantiatedCar);
		instantiatedCar.SetActive (false);
		worldgrass = GameObject.FindGameObjectWithTag ("grass");
		worldgrass.transform.position = new Vector3 (0, 1000, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {
			SimulationToggle ();
		}
	}
	
	public void SimulationToggle ()
	{
		if (!isSimulationRunning) {
			DisableCamera ();
			StartSimulation ();
			instantiatedCar.GetComponent<CalculateLatLon>().setNewWorldLatLon(areaFile);
			instantiatedCar.GetComponent<CalculateLatLon>().setNewWorldWidthHeight();
				
		} else {
			EnableCamera ();
			EndSimulation ();
			WorldScript world = GameObject.FindGameObjectWithTag("ground").GetComponent<WorldScript>();
			world.setOverlay(null, world.getOverlayShown());
		}
	}

	void EnableCamera ()
	{
		mainCamera.camera.enabled = true;
	}
	
	void DisableCamera ()
	{
		mainCamera.camera.enabled = false;
	}
	
	private void StartSimulation ()
	{
		GameObject.FindGameObjectWithTag("ground").GetComponent<WorldScript>().setOverlay(null, false);

		GameObject sPoint = GameObject.FindGameObjectWithTag ("startpoint");
		if (sPoint == null) {
			return;
		}
		isSimulationRunning = true;
		GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SelectObject> ().menuSelected = "simrunning";
		worldgrass = GameObject.FindGameObjectWithTag ("grass");
		worldgrass.transform.position = new Vector3 (0, -0.1f, 0);
		if (instantiatedCar != null) {
			GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelected ("none");
			GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SelectObject> ().guiShow = false;
			instantiatedCar.SetActive (true);


			instantiatedCar.transform.position = sPoint.transform.position;
			float rotY = sPoint.transform.eulerAngles.y;
			instantiatedCar.transform.eulerAngles = new Vector3 (0, 180 + rotY, 0);
			sPoint.GetComponent<StartingPoint> ().Hide ();
			
			GameObject carCam = instantiatedCar.transform.FindChild ("Main_Camera").gameObject;
			if (carCam != null) {	
				carCam.camera.enabled = true;
			} else {
				Debug.Log ("carcam is null");
			}
		} else {
			Debug.Log ("Simcar is null");
		}
		
		Screen.showCursor = false;
	}
	
	private  void EndSimulation ()
	{
		GameObject sPoint = GameObject.FindGameObjectWithTag ("startpoint");
		if (sPoint == null) {
			return;
		}
		isSimulationRunning = false;
		GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelected ("car");
		instantiatedCar.SetActive (false);


		sPoint.GetComponent<StartingPoint> ().Show ();

		worldgrass = GameObject.FindGameObjectWithTag ("grass");
		worldgrass.transform.position = new Vector3 (0, 1000, 0);
		if (instantiatedCar != null) {
			GameObject.Destroy (instantiatedCar.gameObject);
		}
		
		Screen.showCursor = true;
	}
	
	public bool IsSimulationRunning ()
	{
		return this.isSimulationRunning;
	}

	public void setNewLatLon(AreaFile areaFile) {
		this.areaFile = areaFile;
	}
}
