using UnityEngine;
using System.Collections;

public class GridCell : MonoBehaviour
{
	private int gridId;
	private int xDis;
	private int yDis;

	private ArrayList occupants = new ArrayList ();
	private GameObject ghost = null;
	private bool ghosting = false;
	private GameObject copiedGhost = null;
    private PlacedObject copiedPlacedObject = null;

	//if Occupied is true, the gridcell has something on it
	private bool Occupied = false;

	public bool isOccupied
	{
		get	{	return Occupied;	}	
		set {	Occupied = value;	}
	}

	// Use this for initialization
	void Start ()
	{
		occupants = new ArrayList ();	
	}
	
	// Update is called once per frame
	void Update ()
	{
		GameObject ghost = null;

		if (ghosting) {

			//Ghosting methode
			GridScript gridScript = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();
			GameObject sObj = gridScript.getSelectedObject ();


			if (sObj != null) {
				if (isAllowed (sObj)) {
					if (copiedGhost != null) {
                        PlacedObject placedObj = sObj.GetComponent<PlacedObject>();
                        if (placedObj == null)
                            Debug.Log("MISTAKE!");
                        
						//If the ghost is not null, all you need to do is update ghost to selection
                       // for (int iCell = 0; iCell < GameObject.FindWithTag("MainCamera").GetComponent<GridScript>().getSelectedObject().GetComponent<PlacedObject>().getCellPlaces().Length; iCell++)
                        //{
                            /*
                            #region Create new ghost
                            //CREATE NEW GHOST FOR EACH GRID CELL TAKEN BY THE OBJECT
                            ghost = sObj;
                            copiedGhost = GameObject.Instantiate(ghost) as GameObject;
                            copiedGhost.tag = "Untagged";
                            
                            //Remove all colliders
                            Collider[] colliders = copiedGhost.GetComponents<Collider>();
                            Collider[] colliders2 = copiedGhost.GetComponentsInChildren<Collider>();
                            for (int i = 0; i < colliders.Length; i++)
                            {
                                colliders[i].GetComponent<Collider>().enabled = false;
                            }
                            for (int i = 0; i < colliders2.Length; i++)
                            {
                                colliders2[i].GetComponent<Collider>().enabled = false;
                            }
                            #endregion
                            */
                            //Set position for the new ghost, using the array of cells, reserved by the PlacedObject
                            /*xDis = int.Parse(GameObject.FindWithTag("MainCamera").GetComponent<GridScript>().getSelectedObject().GetComponent<PlacedObject>().getCellPlaces()[iCell].Split(',')[0]);
                            yDis = int.Parse(GameObject.FindWithTag("MainCamera").GetComponent<GridScript>().getSelectedObject().GetComponent<PlacedObject>().getCellPlaces()[iCell].Split(',')[1]);
                                Vector3 gridPosition = GameObject.FindObjectOfType<GridScript>().getCells()[GameObject.FindWithTag("MainCamera").GetComponent<GridScript>().getHoveredCellIndexes()[0] + xDis, GameObject.FindWithTag("MainCamera").GetComponent<GridScript>().getHoveredCellIndexes()[1] + yDis].transform.position;
                                gridPosition.y = 0.1f;
                                //copiedGhost.transform.position = gridPosition;
                             * */
                    //    }
						
					} else {
						//If it is, create a new ghost
						//Instantiate it
						ghost = sObj;
						copiedGhost = GameObject.Instantiate (ghost) as GameObject;
						copiedGhost.tag = "Untagged";
						Vector3 gridPosition = this.transform.position;
						gridPosition.y = 0.1f;
						copiedGhost.transform.position = gridPosition;
						copiedGhost.transform.rotation = sObj.transform.rotation;
						//Remove all colliders

						Collider[] colliders = copiedGhost.GetComponents<Collider> ();
						Collider[] colliders2 = copiedGhost.GetComponentsInChildren<Collider> ();
					
						for (int i = 0; i < colliders.Length; i++) {
							colliders [i].GetComponent<Collider> ().enabled = false;							
						}

						for (int i = 0; i < colliders2.Length; i++) {
							colliders2 [i].GetComponent<Collider> ().enabled = false;							
						}

					}
				}
			}


		} else {

		}
	}
	
	void OnMouseEnter ()
	{

		bool running = GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SimulationScript> ().IsSimulationRunning ();
		if (!running) {
            GridScript gridScript = GameObject.FindWithTag("MainCamera").GetComponent<GridScript>();
            if (!gridScript.getDeleteMode())
            {
                ghosting = true;
            }
            //!Todo - Call GridScript method to show ghosts, using this gridCell as base position.
            //float x = this.transform.position.x;
            //float y = this.transform.position.y;
            //float z = this.transform.position.z;

          //  ghost.transform.position.Set(x,y,z);
			EnterColor ();
          //  int[] indexes = gridScript.getHoveredCellIndexes();
			
		}
	} 

	void OnMouseExit ()
	{
		ExitColor ();
		removeGhost ();
		ghosting = false;
	}

	private void EnterColor ()
	{
		Color color = renderer.material.color;
		//color.a = 0.25f;

		GridScript gridScript = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();
		if (!gridScript.getDeleteMode ()) {
					
			//Normal mode. white squares.
			color.r = 0;
			color.g = 1;
			color.b = 0;
		} else {	
			//Delete mode. Red squares.
			color.r = 1;
			color.g = 0;
			color.b = 0;
		}		
		renderer.material.SetColor ("_Color", color);
		renderer.material.SetColor ("_Emission", color);
	}
//	void EnterColor ()
//	{
//		Color color = renderer.material.color;
//		color.a = 0.25f;
//			
//		bool running = GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SimulationScript> ().IsSimulationRunning ();
//
//		if (!running) {
//			GridScript gridScript = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();
//			
//			if (!gridScript.getDeleteMode ()) {
//				if (this.getOccupants ().Count == 0) {
//		
//					string tempname = gridScript.getSelectedObjectName ();
//					if (tempname != null) {
//						if (tempname == "gebouw-01" || tempname == "gebouw-02") {
//							ghost = Resources.Load ("PlacedObjects/" + tempname + "-ghost") as GameObject;
//						} else {
//							ghost = Resources.Load ("PlacedObjects/" + tempname) as GameObject;
//						}
//						copiedGhost = GameObject.Instantiate (ghost) as GameObject;
//						if (tempname == "tree_big") {
//							//copiedGhost.GetComponent<SceneryScript> ().enabled = false;
//
//							Collider[] colliders = copiedGhost.GetComponents<Collider> ();
//							for (int i = 0; i < colliders.Length; i++) {
//								colliders [i].GetComponent<Collider> ().enabled = false;							
//							}
//
//						}
//						copiedGhost.tag = "ghost";
//						Vector3 gridPosition = this.transform.position;
//						gridPosition.y = 0.1f;
//						copiedGhost.transform.position = gridPosition;
//					}
//				}		
//				//Normal mode. white squares.
//				color.r = 1;
//				color.g = 1;
//				color.b = 1;
//			} else {	
//				
//				//Delete mode. Red squares.
//				color.r = 1;
//				color.g = 0;
//				color.b = 0;
//			}
//			
//			renderer.material.SetColor ("_Color", color);
//			renderer.material.SetColor ("_Emission", color);
//		}
//	}



	void ExitColor ()
	{

		Color color = renderer.material.color;
		color.a = 0;
		renderer.material.color = color;
	}
	
//	public bool getIsTaken ()
//	{
//		if (this.occupier == null) {
//			return false;
//		} else {
//			return true;
//		}
//		
//	}
	
	public bool addOccupant (GameObject occ)
	{
		float tempspeed = 0;
		if((occ.GetComponent<Road>() != null & !isOccupied) || (occ.GetComponent<Road>() == null))
		{
		if (isAllowed (occ)) {
			if (occupants == null) {
				occupants = new ArrayList ();	
			}
			if(occ.layer == 8){
				if(occ.GetComponent<Road>().maxSpeed != 50){
					tempspeed = occ.GetComponent<Road>().maxSpeed;
				}
			}
			GameObject copiedObject = GameObject.Instantiate (occ) as GameObject;
			if(tempspeed > 0){
				copiedObject.GetComponent<Road>().maxSpeed = tempspeed;
			}
			//copiedObject.tag = "copy";
							
			//Links the two ids together so you know which object belongs to which grid cell.
			copiedObject.AddComponent<PlacedObject> ();
			copiedObject.GetComponent<PlacedObject> ().setName (occ.name);
							
			Vector3 gridPosition = this.transform.position;
		
			if (copiedObject.layer == 8) {
				gridPosition.y = -0.02f;
			} else {
				gridPosition.y = 0.2f;
			}							
			copiedObject.transform.position = gridPosition;
			//copiedObject.transform.parent = worldParent;

			if (copiedObject.tag == "startpoint") {
				isOccupied = true;
				return true;
			}
			occupants.Add (copiedObject);
			//Debug.Log("occupant loaded");
			isOccupied = true;
			return true;
		}
		}
		return false;
	}

	public bool isAllowed (GameObject orig)
	{
		foreach (GameObject obj in occupants) {
			
			//Means object with same name (one of each object is allowed) is already on cell
			if (orig.name == obj.name) {
//				Debug.Log ("name error");
				return false;
			}

			//Big objects get layer 7.
			//Roads get layer 8.
			//There can only be one 7 or one 8 on the layer.
			//Means a road is already on the cell
		
			if (obj.layer == 8 || obj.layer == 9) {
				if (orig.layer == 8 || orig.layer == 9) {
//					Debug.Log ("layer error");
					return false;
				}
			}
             
		}
		return true;
	}
	public void removeGhost ()
	{
		if (copiedGhost != null) {
            //copiedGhost.transform.position.Set(-999, 999, 999);
            //(PlacedObject)copiedGhost.transform.position.Set(-999, 999, 999);
			GameObject.DestroyImmediate (copiedGhost);
			copiedGhost = null;
		}
	}
	
	public int countOccupants ()
	{
		if (occupants != null) {
			return occupants.Count;
		}
		return 0;
	}

	public ArrayList getOccupants ()
	{
		if (occupants != null) {
			return occupants;
		}
		return null;
	}

	public void removeOccupant(int removeAt) {
		GameObject objectToDelete = (GameObject)occupants[removeAt];
		occupants.RemoveAt(removeAt);
		GameObject.DestroyImmediate(objectToDelete);
	}
	
	public bool removeOccupants ()
	{
		if (occupants != null) {
			Debug.Log("cell is occupied? " + this.isOccupied);
			foreach (GameObject obj in occupants) {
				GameObject.DestroyImmediate (obj);
			}
			//Debug.Log("clear the list");
			occupants.Clear ();
			//Debug.Log("create a new list");
			occupants = new ArrayList ();
			this.isOccupied = false;
			return true;
		}
		return false;
	}
	
	public int getId ()
	{
		return this.gridId;
	}
	
	public void setId (int i)
	{
		gridId = i;
	}

	public void setId (int i, int x, int y)
	{
		gridId = i;
		this.xDis = x;
		this.yDis = y;
	}

	public int getX ()
	{
		return xDis;
	}

	public int getY ()
	{
		return yDis;
	}
}
