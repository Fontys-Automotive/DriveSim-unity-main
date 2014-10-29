using UnityEngine;
using System.Collections;

public class SelectObject : MonoBehaviour {

	public GameObject selectedObject;
	public bool guiShow = false;
	public string menuSelected = "noselection";

	GameObject selectionCube;
	GameObject selectCube;

	// Use this for initialization
	void Start () {
		selectionCube = Resources.Load ("selectionCube") as GameObject;
		selectCube = GameObject.Instantiate (selectionCube) as GameObject;
		selectCube.transform.position = new Vector3(0,1000,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(menuSelected == "0none"){
			if(GameObject.FindGameObjectWithTag("MainScripts").GetComponent<OptieGUI>().getOffset() == 0 && Input.mousePosition.x < (Screen.width - 400)){
				if (Input.GetButton ("Fire1") && !Input.GetKey(KeyCode.LeftShift)) {
					Ray ray1 = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit1;
					int mask = (1 << 8);
					if (Physics.Raycast (ray1, out hit1, Mathf.Infinity, mask)) {
						if (hit1.transform.gameObject.tag != "ghost"){
							GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelectedGameObject(hit1.transform.gameObject);
							GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelected("object");
							selectedObject = hit1.transform.gameObject;
							guiShow = true;
						}
					}else{
						//haal de richting van de huis op
						Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
						//maakt een leeg hit object aan
						RaycastHit hit2;
						//als de ray een object in de scene raakt
						if (Physics.Raycast (ray2, out hit2)) {
							if (hit2.transform.gameObject.tag == "grid"){
								GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelected("grid");
								selectedObject = hit2.transform.gameObject;
								guiShow = true;
							}
						}else{
							guiShow = false;
							GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelected("car");
						}
					}
					//Debug.Log("selected object is of type: " + GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().getSelected());
				}
			}

			if(GameObject.FindGameObjectWithTag("MainScripts").GetComponent<OptieGUI>().getOffset() > 0){
				if (Input.GetButton ("Fire1") && !Input.GetKey(KeyCode.LeftShift)) {
					Ray ray1 = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit1;
					int mask = (1 << 8);
					if (Physics.Raycast (ray1, out hit1, Mathf.Infinity, mask)) {
						if (hit1.transform.gameObject.tag != "ghost"){
							GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelectedGameObject(hit1.transform.gameObject);
							GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelected("object");
							selectedObject = hit1.transform.gameObject;
							guiShow = true;
						}
					}else{
						//haal de richting van de huis op
						Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
						//maakt een leeg hit object aan
						RaycastHit hit2;
						//als de ray een object in de scene raakt
						if (Physics.Raycast (ray2, out hit2)) {
							if (hit2.transform.gameObject.tag == "grid"){
								GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelected("grid");
								selectedObject = hit2.transform.gameObject;
								guiShow = true;
							}
						}else{
							guiShow = false;
							GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelected("car");
						}
					}
					//Debug.Log("selected object is of type: " + GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().getSelected());
				}
			}
			if(Input.GetButton ("Fire1") && Input.mousePosition.y < (Screen.height - (Screen.height - GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<GUIScript> ().getGUIHeight()))){
				guiShow = false;
			}
		}
		if(guiShow){
			if(selectCube != null && selectedObject != null){
				selectCube.transform.position = selectedObject.transform.position;
			}
		}else{
			selectCube.transform.position = new Vector3(0,1000,0);
		}
	}

}
