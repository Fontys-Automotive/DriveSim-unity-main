using UnityEngine;
using System.Collections;
using System.Windows.Forms;

public class GridScript : MonoBehaviour
{
	public Material gridMaterial;
	public GameObject selectionQuad;
	public GameObject objectToPlace;
	private GameObject ghostObject;
	private bool deleteMode = false;
	private bool selectcolor = false;
	private int selectedGridID = -1;
	private GameObject[,] cells;
	private GameObject mouseSelectedObject = null;
	private float deltaTime = 0.0f;


	private bool shiftdown = false;
	private bool deldown = false;

	
	//Moet 10 blijven!! Verneukt alles als dit wordt veranderd.
	private int gridCellSize = 10;
	private int xGridSize = 21;
	private int yGridSize = 9;
	
	//set a certain height so the grid doesn overlap with the world
	private float gridHeight = .05f;
	//private Color gridColor = new Color (1, 1, 1, 0.5f);
	private bool isGridOn = true;
	private Transform quadParent;
	public Transform worldParent;

	private PlaceItem placeItem;
	
	public void switchDeleteMode ()
	{
		if (deleteMode) {
			deleteMode = false;	
		} else {
			deleteMode = true;	
		}
	}
	
	public bool getDeleteMode ()
	{
		return deleteMode;
	}
	
	public int GetHorSize ()
	{
		return this.xGridSize;
	}

	public void SetHorSize (int i)
	{
		this.xGridSize = i;
	}
	
	public int GetVertSize ()
	{
		return this.yGridSize;
	}

	public void SetVertSize (int i)
	{
		this.yGridSize = i;
	}
	
	// Use this for initialization
	void Start ()
	{
		//Instantiate stuff
		this.quadParent = GameObject.Find ("SelectionQuads").transform;
		this.worldParent = GameObject.Find ("PlacedObjects").transform;
			
		//Create all the quads for the grid
		CreateGrid (this.yGridSize, this.xGridSize);
		placeItem = PlaceItem.GetInstance();
	}
	
	// Update is called once per frame
	void Update ()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		bool running = GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SimulationScript> ().IsSimulationRunning ();
		if (!running) {
			KeyControls ();
			GridControl ();
		}
	}
	
	void OnPostRender ()
	{
		if (isGridOn) {
			gridMaterial.SetPass (0);
			//Make vertical lines
			GL.PushMatrix ();
			for (int i = 0; i <= xGridSize; i++) {
				GL.Begin (GL.LINES);
				GL.Vertex3 (i * gridCellSize, gridHeight, 0);
				GL.Vertex3 (i * gridCellSize, gridHeight, gridCellSize * yGridSize);
				GL.End ();
			}
		
			//Make horizontal lines
			gridMaterial.SetPass (0);
			for (int i = 0; i <= yGridSize; i++) {
				GL.Begin (GL.LINES);
				GL.Vertex3 (0, gridHeight + 0.1f, i * gridCellSize);
				GL.Vertex3 (gridCellSize * xGridSize, gridHeight + 0.1f, i * gridCellSize);
				GL.End ();
			}	
			GL.PopMatrix ();
		}		
	}
	
	//Temporary camera movement function
	private void KeyControls ()
	{		
		//Change key input!
		if (Input.GetKeyDown (KeyCode.G)) {
			if (isGridOn) {
				isGridOn = false;
			} else {
				isGridOn = true;
			}
		}
		if (Input.GetKeyDown (KeyCode.Alpha1)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("1");
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("2");
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("3");
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("4");
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("5");
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("6");
		}
		if (Input.GetKeyDown (KeyCode.Alpha7)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("7");
		}
		if (Input.GetKeyDown (KeyCode.Alpha8)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("8");
		}
		if (Input.GetKeyDown (KeyCode.Alpha9)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("9");
		}
		if (Input.GetKeyDown (KeyCode.Alpha0)){
			objectToPlace = Resources.Load("")as GameObject;
			MessageBox.Show ("0");
		}
		
		//Change key input!
		if (Input.GetKeyDown (KeyCode.Q)) {
			RotateObjectLeft ();
		}
		
		if (Input.GetKeyDown (KeyCode.E)) {
			RotateObjectRight ();
		}
		
		if (Input.GetKeyDown (KeyCode.Delete)) {
				switchDeleteMode ();
		}

		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.Delete)){
			Debug.Log("delete all");
			var confirmResult = MessageBox.Show("Are you sure to delete this world?","", MessageBoxButtons.YesNo);
			if (confirmResult == DialogResult.Yes)
			{
				ResetGrid (yGridSize, xGridSize);
				switchDeleteMode ();
			}
			else
			{
				return;
			}
		}

		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.S)) {
			GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<GUIScript> ().showSaveBox ();
		}
		
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.O)) {
			GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<GUIScript> ().showLoadBox ();
		}
		
		if (Input.GetKeyDown (KeyCode.KeypadPlus)) {
			if (yGridSize < 2000 || xGridSize < 2000) {
				ResetGrid (yGridSize + 1, xGridSize + 1);
			}
		}
		
		if (Input.GetKeyDown (KeyCode.KeypadMinus)) {
			
			if (yGridSize > 2 || xGridSize > 2) {
				ResetGrid (yGridSize - 1, xGridSize - 1);
			}
		}
	}
	
	public void RotateObjectLeft ()
	{
		if (objectToPlace != null) {
			Vector3 currentRot = objectToPlace.transform.eulerAngles;
			objectToPlace.transform.localEulerAngles = new Vector3 (currentRot.x, currentRot.y - 90, currentRot.z);
		}
	}

	public void RotateObjectRight ()
	{
		if (objectToPlace != null) {
			Vector3 currentRot = objectToPlace.transform.eulerAngles;
			objectToPlace.transform.localEulerAngles = new Vector3 (currentRot.x, currentRot.y + 90, currentRot.z);
		}
	}
	
	public void DrawSelectionQuad (Vector3 position, int id, int x, int y)
	{
		//Get the quad from the prefabs
		GameObject quad = GameObject.Instantiate (selectionQuad) as GameObject;
		
		//GameObject quad = new GameObject();
		//quad.tag = "gridquad";
		//Now set it to the proper grid size...
		quad.transform.localScale = new Vector3 (gridCellSize, gridCellSize, gridCellSize);
		
		//Next place the quad at the first quad cell, with a little deviation
		float deviation = quad.renderer.bounds.size.x / 2;
		quad.transform.position = new Vector3 (position.x + deviation, position.y, position.z + deviation);
		
		//quad.transform.parent = quadParent;
		quad.GetComponent<GridCell> ().setId (id, x, y);
		cells [y, x] = quad;
	}
	
	public void setSelectedObject (string name)
	{
//	print (name);
		objectToPlace = Resources.Load ("PlacedObjects/" + name) as GameObject;

		//Place it outside of the screen
//		objectToPlace.transform.position = new Vector3 (-999, -999, -999);
	}

	public GameObject getSelectedObject ()
	{
		if (objectToPlace != null) {
			return objectToPlace;
		}
		return null; 
	}

	public string getSelectedObjectName ()
	{
		if (objectToPlace != null) {
			return objectToPlace.name;
		}
		return null; 
	}
	
	private void GridControl ()
	{
		//As soon as you press the left mouse button...
		if(!Input.GetKey(KeyCode.LeftShift)){
			if (Input.GetButton ("Fire1")) {
				int guiWidth = GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().guiWidth;
				int guiHeight = GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<GUIScript> ().getGUIHeight ();
				bool loadSaveBoxActive = GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<GUIScript> ().loadSaveBoxActive ();
				if (!loadSaveBoxActive && Input.mousePosition.x < (UnityEngine.Screen.width - guiWidth) && Input.mousePosition.y > guiHeight) {
					if (!deleteMode) {
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit hit;
						int mask = (1 << 10);		
						//Check the object you hit...
										
						//Layer 10 is gridcell.
						if (Physics.Raycast (ray, out hit, Mathf.Infinity, mask)) {	
							
							//If it's a grid square...
							if (hit.transform.gameObject.tag == "grid") {
								GridCell cellScript = hit.transform.GetComponent<GridCell> ();
								placeItem.PlaceObject (cellScript, objectToPlace);

							}
							/*
							//If it's a selectable object... Select it!
							if (hit.transform.gameObject.tag == "object") {
								objectToPlace = hit.transform.gameObject;
							}	*/	
						}
					} else {
						//Debug.Log ("Delete ray");
						//haal de richting van de muis op
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						//maakt een leeg hit object aan
						RaycastHit hit;
						int mask = (1 << 10);
						//als de ray een object in de scene raakt
						if (Physics.Raycast (ray, out hit, Mathf.Infinity, mask)) {
							if (hit.transform.gameObject.tag == "grid") {

								GridCell cellScript = hit.transform.gameObject.GetComponent<GridCell> ();
								this.RemoveObject (cellScript);
							}
						}
					}
				}
			}		
		}
		//kijk of de rechter muisknop is ingedrukt
//		if (Input.GetMouseButtonDown (1)) {
//
//		}
	}

	public bool RemoveObject (GridCell cell)
	{
		return cell.removeOccupants ();
	}

	private void CreateGrid (int ySize, int xSize)
	{
		this.yGridSize = ySize;
		this.xGridSize = xSize;

		cells = new GameObject[ySize, xSize];
		int id = 0;
		for (int y = 0; y < ySize; y++) {
			for (int x = 0; x < xSize; x++) {
				Vector3 pos = new Vector3 (x * gridCellSize, gridHeight, y * gridCellSize);
				DrawSelectionQuad (pos, id, x, y);	
				id++;
			}
		}

		
		GameObject.Find ("world_plane").GetComponent<WorldScript> ().resizePlane ();
	}
	
	private void ClearGrid ()
	{
		
		GameObject[] grids = GameObject.FindGameObjectsWithTag ("grid");
		foreach (GameObject cell in grids) {
			
			cell.GetComponent<GridCell> ().removeGhost ();
			//Removes the occupant...
			cell.GetComponent<GridCell> ().removeOccupants ();	
			//Then remove the gridquad itself
			GameObject.DestroyImmediate (cell);
		}
	}

	public  void OnGUI()
	{
				int w = Screen.width, h = Screen.height;
				FPSGUI s = new FPSGUI ();
				GUIStyle style = new GUIStyle ();
		
				Rect rect = new Rect (0, 0, w, h * 2 / 100);
				style.alignment = TextAnchor.UpperLeft;
				style.fontSize = h * 2 / 100;
				style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
				float msec = deltaTime * 1000.0f;
				float fps = 1.0f / deltaTime;
				string text = string.Format ("{0:0.0} ms ({1:0.} fps)", msec, fps);
				GUI.Label (rect, text, style);
	}

	public void ResetGrid (int y, int x)
	{
		this.ClearGrid ();
		this.CreateGrid (y, x);
	}

	public GameObject[,] getCells() {
		return cells;
	}

	public GridCell getCell (int x, int y)
	{
		if(y < 0 || y >= cells.GetLength(0)) {
			return null;
		}
		else {
			if(x < 0 || x >= cells.GetLength(1)) {
				return null;
			}
		}
		GameObject g = cells[y,x];
		return g.GetComponent<GridCell>();
	}
}

