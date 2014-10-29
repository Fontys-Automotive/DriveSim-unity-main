using UnityEngine;
using System.Collections;

public class MultiSelect : MonoBehaviour
{


	GameObject selectionCube;
	GameObject selectCube;

	GameObject selectedObject1 = null;
	GameObject selectedObject2 = null;

	GameObject[,] gridcells;
	ArrayList gridcellselection;
	ArrayList multiselection;
	ArrayList selectcubes;
	public bool multiSelectionActive = false;
	
	// Use this for initialization
	void Start ()
	{
		selectionCube = Resources.Load ("selectionCube") as GameObject;
		multiselection = new ArrayList ();
		selectcubes = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SelectObject> ().menuSelected == "0none") {
			if (Input.GetButtonDown ("Fire1") && !Input.GetKey(KeyCode.LeftShift)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				int mask = (1 << 10);		
				//Check the object you hit...
				
				//Layer 10 is gridcell.
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, mask)) {	
					selectedObject1 = hit.transform.gameObject;
				}
			}
			if (Input.GetButtonDown ("Fire1") && Input.GetKey (KeyCode.LeftShift) && Input.mousePosition.x < (Screen.width - 400)) {
				Ray ray1 = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit1;
				int mask = (1 << 10);
				if (Physics.Raycast (ray1, out hit1, Mathf.Infinity, mask)) {
					if (hit1.transform.gameObject.tag != "ghost") {
						if (selectedObject1 == null) {
							selectedObject1 = hit1.transform.gameObject;
						} else {
							selectedObject2 = hit1.transform.gameObject;
						}
					}
				}
			}
			if (selectedObject1 != null && selectedObject2 != null) {
				multiSelect (selectedObject1.transform.gameObject, selectedObject2.transform.gameObject);
				GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SelectObject> ().guiShow = false;
			}

			if (Input.GetButtonDown ("Fire1") && GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().getOffset () > 0) {
				if (selectcubes.Count > 0) {
					foreach (GameObject item in selectcubes) {
						DestroyImmediate (item);
					}
					selectcubes.Clear ();
					selectcubes = new ArrayList ();
					multiselection.Clear ();
					multiselection = new ArrayList ();
					multiSelectionActive = false;
				}
			}
			if (Input.GetButtonDown ("Fire1") && Input.GetKey (KeyCode.LeftShift) && GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().getOffset () > 0) {
				Ray ray1 = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit1;
				int mask = (1 << 10);
				if (Physics.Raycast (ray1, out hit1, Mathf.Infinity, mask)) {
					if (hit1.transform.gameObject.tag != "ghost") {
						if (selectedObject1 == null) {
							selectedObject1 = hit1.transform.gameObject;
						} else {
							selectedObject2 = hit1.transform.gameObject;
						}
					}
				}
			}
			if (selectedObject1 != null && selectedObject2 != null) {
				multiSelect (selectedObject1.transform.gameObject, selectedObject2.transform.gameObject);
				GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SelectObject> ().guiShow = false;
				selectedObject1 = null;
				selectedObject2 = null;
			}
		} else {
			if(selectcubes.Count > 0){
				foreach (GameObject item in selectcubes) {
					DestroyImmediate (item);
				}
				selectcubes.Clear ();
				selectcubes = new ArrayList ();
			}
			multiselection.Clear ();
			multiselection = new ArrayList ();
			multiSelectionActive = false;
			selectedObject1 = null;
			selectedObject2 = null;
		}
	}

	public void setRulesMultiselect (Road.Rule rule)
	{
		int tempindex = 0;
		if (multiselection.Count > 0) {
			foreach (GameObject item in multiselection) {
				//item.GetComponent<Road>().maxSpeed = maxspeed;
				ArrayList temparray = item.GetComponent<GridCell> ().getOccupants ();
				if (temparray != null) {
					foreach (GameObject occupant in temparray) {
						if (occupant.layer == 8) {
							if(tempindex == 0){
								occupant.GetComponent<Road>().SetRules(rule, true);
							}else{
								occupant.GetComponent<Road>().SetRules(rule, false);
							}
							tempindex ++;
						}
					}
				}
			}
		}
	}

	public void setPropertiesMultiselect (bool leftcurb, bool lightpost, bool trafficlight, int maxspeed)
	{
		if (multiselection.Count > 0) {
			foreach (GameObject item in multiselection) {
				//item.GetComponent<Road>().maxSpeed = maxspeed;
				ArrayList temparray = item.GetComponent<GridCell> ().getOccupants ();
				if (temparray != null) {
					int tempindex = 0;
					foreach (GameObject occupant in temparray) {
						if (occupant.layer == 8) {
							occupant.GetComponent<Road> ().SetSidewalks (leftcurb);
							occupant.GetComponent<Road> ().SetLightposts (lightpost);
							occupant.GetComponent<Road> ().SetTrafficLight (trafficlight);
							if (maxspeed > 0) {
								occupant.GetComponent<Road> ().maxSpeed = maxspeed;
							}
							tempindex ++;
						}
					}
				}
			}
		}
	}

	private void multiSelect (GameObject pos1, GameObject pos2)
	{
		multiSelectionActive = true;
		//locationindex tell where pos2 position is from pos1 position
		//-1 = notfound
		// 0 = right top
		// 1 = right bottom
		// 2 = left top
		// 3 = left bottom
		int locationindex = -1;
		int xsize = 0;
		int ysize = 0;
		if (pos1 != null && pos2 != null && pos1.layer == 10 && pos2.layer == 10) {
			gridcells = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<GridScript> ().getCells ();
			if (pos2.GetComponent<GridCell> ().getX () >= pos1.GetComponent<GridCell> ().getX ()) {
				if (pos2.GetComponent<GridCell> ().getY () >= pos1.GetComponent<GridCell> ().getY ()) {
					locationindex = 0;
					xsize = pos2.GetComponent<GridCell> ().getX () - pos1.GetComponent<GridCell> ().getX ();
					ysize = pos2.GetComponent<GridCell> ().getY () - pos1.GetComponent<GridCell> ().getY ();
				} else {
					locationindex = 1;
					xsize = pos2.GetComponent<GridCell> ().getX () - pos1.GetComponent<GridCell> ().getX ();
					ysize = pos1.GetComponent<GridCell> ().getY () - pos2.GetComponent<GridCell> ().getY ();
				}
			} else {
				if (pos2.GetComponent<GridCell> ().getY () >= pos1.GetComponent<GridCell> ().getY ()) {
					locationindex = 2;
					xsize = pos1.GetComponent<GridCell> ().getX () - pos2.GetComponent<GridCell> ().getX ();
					ysize = pos2.GetComponent<GridCell> ().getY () - pos1.GetComponent<GridCell> ().getY ();
				} else {
					locationindex = 3;
					xsize = pos1.GetComponent<GridCell> ().getX () - pos2.GetComponent<GridCell> ().getX ();
					ysize = pos1.GetComponent<GridCell> ().getY () - pos2.GetComponent<GridCell> ().getY ();
				}
			}
			gridcellselection = new ArrayList ();

			switch (locationindex) {
			case 0:
				for (int y = pos1.GetComponent<GridCell>().getY(); y <= pos1.GetComponent<GridCell>().getY() + ysize; y++) {
					for (int x = pos1.GetComponent<GridCell>().getX(); x <= pos1.GetComponent<GridCell>().getX() + xsize; x++) {
						gridcellselection.Add (gridcells [y, x]);
					}
				}
				break;
			case 1:
				for (int y = pos2.GetComponent<GridCell>().getY(); y <= pos2.GetComponent<GridCell>().getY() + ysize; y++) {
					for (int x = pos1.GetComponent<GridCell>().getX(); x <= pos1.GetComponent<GridCell>().getX() + xsize; x++) {
						gridcellselection.Add (gridcells [y, x]);
					}
				}
				break;
			case 2:
				for (int y = pos1.GetComponent<GridCell>().getY(); y <= pos1.GetComponent<GridCell>().getY() + ysize; y++) {
					for (int x = pos2.GetComponent<GridCell>().getX(); x <= pos2.GetComponent<GridCell>().getX() + xsize; x++) {
						gridcellselection.Add (gridcells [y, x]);
					}
				}
				break;
			case 3:
				for (int y = pos2.GetComponent<GridCell>().getY(); y <= pos2.GetComponent<GridCell>().getY() + ysize; y++) {
					for (int x = pos2.GetComponent<GridCell>().getX(); x <= pos2.GetComponent<GridCell>().getX() + xsize; x++) {
						gridcellselection.Add (gridcells [y, x]);
					}
				}
				break;
			}

			foreach (GameObject cell in gridcellselection) {
				selectCube = GameObject.Instantiate (selectionCube) as GameObject;
				selectCube.transform.position = cell.transform.position;
				selectcubes.Add (selectCube);
				multiselection.Add (cell);
			}
		}
	}
}
