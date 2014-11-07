using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class OptieGUI : MonoBehaviour
{
	private GUIStyle style;
	private Texture2D bgTex;
	private GUIStyle styleButton;
	private Texture2D bgTexButton;
	private int guiHeight = 0;
	public int guiWidth = 0;
	private string objectSelected = "car";
	private GameObject selectedGameobject;

	public Texture2D btnLockOpen;
	public Texture2D btnLockClose;
	private Texture2D lockTexture;
	private bool guiLock;
	public bool guiLock2;

	//Car variables
	private string topSpeed;
	private string gearAmount;
	
	//Grid variables
	private string gridx;
	private string gridy;
	
	//Road
	private GameObject carGameobject;
	private GUIContent[] trafficSignList;
	private bool showList = false;
	public GUIStyle dropdownStyle;

	private int guiOffset = 350;
	private float guiOffsetfloat = 350;

	private GridXmlSerializer ser;
	
	float x = 1;
	float y = 1;

	Car carScript = null;
	Boolean keyboardon = true;
	Boolean wheelOn = false;
	Boolean simulatorOn = false;
	Boolean prevKeyboardOn = false;
	Boolean prevWheelOn = false;
	Boolean prevSimulatorOn = false;
	int drag_x = 0;
	int drag_y = 0;
	int drag_z = 0;
	int topSpeedcar = 0;
	int numberOfGears = 0;
	int maximumTurn = 0;
	int minimumTurn = 0;
	int extremumSlip = 0;
	int extremumValue = 0;
	int asymptoteSlip = 0;
	int asymptoteValue = 0;
	int stiffness = 0;
	int suspensionRange = 0;
	int suspensionDamper = 0;
	int suspensionSpringFront = 0;
	int suspensionSpringRear = 0;

	bool curbmulti;
	bool lightmulti;
	bool trafficmulti;
	Road.Rule currentRule = Road.Rule.NO_SIGN;

	// Use this for initialization
	void Start ()
	{
		dropdownStyle = new GUIStyle ();		
		dropdownStyle.normal.textColor = Color.white; 
		//dropdownStyle.onHover.background =
		dropdownStyle.hover.background = new Texture2D (2, 2);
		dropdownStyle.padding.left = 1;
		dropdownStyle.padding.right = 1;
		dropdownStyle.padding.top = 1;
		dropdownStyle.padding.bottom = 4;
		trafficSignList = new GUIContent[System.Enum.GetValues (typeof(Road.Rule)).Length];
		style = new GUIStyle ();
		styleButton = new GUIStyle ();
		guiHeight = (int)(UnityEngine.Screen.height - 150);

		gridx = "15";
		gridy = "15";
		bgTex = new Texture2D (1, 1);
		bgTex.SetPixel (0, 0, new Color (0, 0, 0, 0.75f));
		bgTex.Apply ();

		bgTexButton = new Texture2D (1, 1);
		bgTexButton.SetPixel (0, 0, new Color (1, 1, 1, 0.75f));
		bgTexButton.Apply ();

		style.normal.background = bgTex;
		styleButton.normal.background = bgTexButton;
		carScript = carGameobject.transform.GetComponent<Car> ();
		loadCarValues ();
		lockTexture = btnLockOpen;
		guiLock = false;
		guiLock2 = false;
		curbmulti = false;
		lightmulti = false;
		trafficmulti = false;
		ser = new GridXmlSerializer ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (guiLock == false) {
			guiWidth = 400 - guiOffset;
			if (Input.mousePosition.y < (UnityEngine.Screen.height - (UnityEngine.Screen.height - GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<GUIScript> ().getGUIHeight ()))) {
				if (guiOffset > 0) {
					guiOffsetfloat -= 10f;
					guiOffset = (int)guiOffsetfloat;
				}
			} else {
				if (Input.mousePosition.x < (UnityEngine.Screen.width - 400)) {
					if (guiOffset < 350) {
						guiOffsetfloat += 10f;
						guiOffset = (int)guiOffsetfloat;
					}
				}
			}
			if (Input.mousePosition.x > (UnityEngine.Screen.width - 50)) {
				if (guiOffset > 0) {
					guiOffsetfloat -= 10f;
					guiOffset = (int)guiOffsetfloat;
				}
			}

			if (Input.mousePosition.x > (UnityEngine.Screen.width - 400) && Input.mousePosition.x < (UnityEngine.Screen.width - 50) && guiOffset < 350) {
				if (guiOffset > 0) {
					guiOffsetfloat -= 10f;
					guiOffset = (int)guiOffsetfloat;
				}
			}
			if (guiOffset > 350) {
				guiOffset = 350;
				guiOffsetfloat = 350;
			}

			if (guiOffset < 0) {
				guiOffset = 0;
				guiOffsetfloat = 0;
			}
		} else {
			if (guiLock) {
				guiOffset = 0;
			}
		}
		if (guiLock2) {
			lockTexture = btnLockOpen;
			guiLock = false;
			guiOffset = 350;
			guiOffsetfloat = 350;
		}
	}
	
	public void setSelectedGameObject (GameObject selectedobject)
	{
		selectedGameobject = selectedobject;
	}
	
	public void setSelected (string selectedtype)
	{
		objectSelected = selectedtype;
	}
	
	public string getSelected ()
	{
		return objectSelected;
	}

	public int getOffset ()
	{
		return guiOffset;
	}

	public void setCarGameObject (GameObject car)
	{
		carGameobject = car;
	}

	private void loadCarValues ()
	{
		keyboardon = carScript.usekeyboard;
		simulatorOn = carScript.usesimulator;
		wheelOn = carScript.usewheel;
		drag_x = (int)carScript.dragMultiplier.x;
		drag_y = (int)carScript.dragMultiplier.y;
		drag_z = (int)carScript.dragMultiplier.z;
		topSpeedcar = (int)carScript.topSpeed;
		numberOfGears = (int)carScript.numberOfGears;
		maximumTurn = (int)carScript.maximumTurn;
		minimumTurn = (int)carScript.minimumTurn;
		extremumSlip = (int)carScript.extremumSlip;
		extremumValue = (int)carScript.extremumValue;
		asymptoteSlip = (int)carScript.asymptoteSlip;
		asymptoteValue = (int)carScript.asymptoteValue;
		stiffness = (int)carScript.stiffness;
		suspensionRange = (int)carScript.suspensionRange;
		suspensionDamper = (int)carScript.suspensionDamper;
		suspensionSpringFront = (int)carScript.suspensionSpringFront;
		suspensionSpringRear = (int)carScript.suspensionSpringRear;
	}

	private void saveCarValues ()
	{
		carScript.usekeyboard = keyboardon;
		carScript.usewheel = wheelOn;
		carScript.usesimulator = simulatorOn;
		carScript.dragMultiplier.x = drag_x >= 1 ? drag_x : 1;
		carScript.dragMultiplier.y = drag_y >= 1 ? drag_y : 1;
		carScript.dragMultiplier.z = drag_z >= 1 ? drag_z : 1;
		carScript.topSpeed = topSpeedcar >= 1 ? topSpeedcar : 1;
		carScript.numberOfGears = numberOfGears >= 1 ? numberOfGears : 1;
		carScript.maximumTurn = maximumTurn >= 1 ? maximumTurn : 1;
		carScript.minimumTurn = minimumTurn >= 1 ? minimumTurn : 1;
		carScript.extremumSlip = extremumSlip >= 1 ? extremumSlip : 1;
		carScript.extremumValue = extremumValue >= 1 ? extremumValue : 1;
		carScript.asymptoteSlip = asymptoteSlip >= 1 ? asymptoteSlip : 1;
		carScript.asymptoteValue = asymptoteValue >= 1 ? asymptoteValue : 1;
		carScript.stiffness = stiffness >= 1 ? stiffness : 1;
		carScript.suspensionRange = suspensionRange >= 1 ? suspensionRange : 1;
		carScript.suspensionDamper = suspensionDamper >= 1 ? suspensionDamper : 1;
		carScript.suspensionSpringFront = suspensionSpringFront >= 1 ? suspensionSpringFront : 1;
		carScript.suspensionSpringRear = suspensionSpringRear >= 1 ? suspensionSpringRear : 1;
		loadCarValues ();
	}
	
	void OnGUI ()
	{	
		//What to do if the grid is selected
		if (objectSelected == "grid" && GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<MultiSelect> ().multiSelectionActive == false) {
			GridScript gridScript = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth, 0, guiWidth, guiHeight), bgTex, style);

			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 10, 100, 50), "Grid X");
			gridx = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 110 + guiOffset, 10, 75, 20), gridx, 25);
			gridx = Regex.Replace (gridx, @"[^0-9]", "");				
		
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 10, 100, 50), "Grid Y");
			gridy = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 10, 75, 20), gridy, 25);
			gridy = Regex.Replace (gridy, @"[^0-9]", "");
			
			if (GUI.Button (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 50, 100, 50), "Apply")) {
				if (gridx.Length > 0 && gridy.Length > 0) {
					int gridXint = int.Parse (gridx);
					int gridYint = int.Parse (gridy);
					ser.resizeGrid (gridXint, gridYint);
					//gridScript.ResetGrid (gridYint, gridXint);
				}
			}
		}
		if (GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SelectObject> ().menuSelected == "0none") {
			if (GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<MultiSelect> ().multiSelectionActive) {
				GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth, 0, guiWidth, guiHeight), bgTex, style);
				
				curbmulti = GUI.Toggle (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 10, 150, 50), curbmulti, "Curbs");
				lightmulti = GUI.Toggle (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 60, 150, 50), lightmulti, "Lightposts");
				trafficmulti = GUI.Toggle (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 110, 150, 50), trafficmulti, "Traffic Lights");

				//maxspeed
				GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 10, 100, 50), "Max speed");
				string maxSpeedString = selectedGameobject.GetComponent<Road> ().maxSpeed.ToString ();
				
				maxSpeedString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 10, 75, 20), maxSpeedString, 25);
				maxSpeedString = Regex.Replace (maxSpeedString, @"[^a-zA-Z0-9 ]", "");
				int maxSpeed;
				int.TryParse (maxSpeedString, out maxSpeed);

				GUIContent currentEntry = new GUIContent (currentRule.ToString());
				int i = 0;
				int entryNumber = 0;
				foreach (var sign in System.Enum.GetValues(typeof(Road.Rule))) {
					trafficSignList [i] = new GUIContent (sign.ToString ());
					if (trafficSignList [i].ToString () == currentEntry.ToString ())
						entryNumber = i;
					i++;
				}
				
				if (Popup.List (new Rect (UnityEngine.Screen.width - guiWidth + 250 + guiOffset, 50, 150, 20), ref showList, ref entryNumber, currentEntry, trafficSignList, dropdownStyle)) {
					Road.Rule chosen = (Road.Rule)System.Enum.GetValues (typeof(Road.Rule)).GetValue (entryNumber);
					GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<MultiSelect> ().setRulesMultiselect(chosen);
					currentRule = chosen;
				}

				GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<MultiSelect> ().setPropertiesMultiselect (curbmulti, lightmulti, trafficmulti, maxSpeed);
			}
		}
		
		//What to do if the road is selected
		if (objectSelected == "object" && GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<MultiSelect> ().multiSelectionActive == false) {
			if (selectedGameobject != null && selectedGameobject.layer == 8) {
				GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth, 0 , guiWidth, guiHeight), bgTex, style);

			if (Input.GetKeyDown (KeyCode.Mouse1)) {
					this.enabled = true;
					x = Input.mousePosition.x;
					y = guiHeight - Input.mousePosition.y + 50;
				}

				GUI.Label(new Rect (/*Screen.width - guiWidth, 0, guiWidth, guiHeight*/x, y, 200,150), bgTex, style);

				//hassidewalk
				//bool[] bools = selectedGameobject.GetComponent<Road> ().HasSidewalks ();
				bool curbs = selectedGameobject.GetComponent<Road> ().HasSidewalks ();
				;
				//bool hasRightCurb = bools [1];

				bool hasLightpost = selectedGameobject.GetComponent<Road> ().HasLightposts ();
				bool hasTrafficLight = selectedGameobject.GetComponent<Road> ().HasTrafficLightBoolean ();
	//				Debug.Log (leftSW);
	//				Debug.Log (rightSW);

				curbs = GUI.Toggle (new Rect (x, y, 150, 50), curbs, "Curbs");
					//hasRightCurb = GUI.Toggle (new Rect (Screen.width - guiWidth + 10 + guiOffset, 60, 150, 50), hasRightCurb, "Right curb");
				hasLightpost = GUI.Toggle (new Rect (x, y + 20, 150, 50), hasLightpost, "Lightposts");
				hasTrafficLight = GUI.Toggle (new Rect (x, y + 40, 150, 50), hasTrafficLight, "Traffic Lights");

				selectedGameobject.GetComponent<Road> ().SetSidewalks (curbs);
				selectedGameobject.GetComponent<Road> ().SetLightposts (hasLightpost);
				selectedGameobject.GetComponent<Road> ().SetTrafficLight (hasTrafficLight);
				//maxspeed
				GUI.Label (new Rect (x + 5, y + 60, 100, 50), "Max speed");
				string maxSpeedString = selectedGameobject.GetComponent<Road> ().maxSpeed.ToString ();
					
				maxSpeedString = GUI.TextField (new Rect (x + 100, y + 60, 75, 20), maxSpeedString, 25);
				maxSpeedString = Regex.Replace (maxSpeedString, @"[^a-zA-Z0-9 ]", "");
				int maxSpeed;
				int.TryParse (maxSpeedString, out maxSpeed);
				if (maxSpeed > 0) {
					selectedGameobject.GetComponent<Road> ().maxSpeed = maxSpeed;
				}
					
				GUIContent currentEntry = new GUIContent (selectedGameobject.GetComponent<Road> ().GetRules ().ToString ());
				int i = 0;
				int entryNumber = 0;
				foreach (var sign in System.Enum.GetValues(typeof(Road.Rule))) {
					trafficSignList [i] = new GUIContent (sign.ToString ());
				if (trafficSignList [i].ToString () == currentEntry.ToString ())
					entryNumber = i;
					i++;
				}
					
				if (GUI.Button (new Rect (x + 5, y + 125, 50, 20), "Apply")) {
					this.enabled = false;
					Road.Rule chosen = (Road.Rule)System.Enum.GetValues (typeof(Road.Rule)).GetValue (entryNumber);
					selectedGameobject.GetComponent<Road> ().SetRules (chosen, true);
				}
			} 
		}
		
		if (objectSelected == "car" && GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<MultiSelect> ().multiSelectionActive == false) {
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth, 0, guiWidth, guiHeight), bgTex, style);
			
			//keyboard
			keyboardon = GUI.Toggle (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 10, 100, 20), keyboardon, "Use Keyboard");
			simulatorOn = GUI.Toggle (new Rect (UnityEngine.Screen.width - guiWidth + 160 + guiOffset, 10, 100, 20), simulatorOn, "Use Simulator");
			wheelOn = GUI.Toggle (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 10, 100, 20), wheelOn, "Use Wheel");

			if(keyboardon == true && prevKeyboardOn == false) {
				simulatorOn = false;
				wheelOn = false;
			}
			if(simulatorOn == true && prevSimulatorOn == false) {
				keyboardon = false;
				wheelOn = false;
			}
			if(wheelOn == true && prevWheelOn == false) {
				keyboardon = false;
				simulatorOn = false;
			}

			if(keyboardon == false && simulatorOn == false && wheelOn == false) {
				keyboardon = true;
			}

			prevKeyboardOn = keyboardon;
			prevSimulatorOn = simulatorOn;
			prevWheelOn = wheelOn;
			carScript.usekeyboard = keyboardon;
			carScript.usewheel = wheelOn;
			carScript.usesimulator = simulatorOn;

			string dragMpl_xString = "";
			string dragMpl_yString = "";
			string dragMpl_zString = "";
			//drag x
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 50, 100, 50), "Dragmultiplier X");
			dragMpl_xString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 50, 75, 20), drag_x.ToString (), 25);
							
			//drag y
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 50, 100, 50), "Dragmultiplier Y");
			dragMpl_yString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 110 + guiOffset, 50, 75, 20), drag_y.ToString (), 25);
			
			//drag z
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 90, 100, 50), "Dragmultiplier Z");
			dragMpl_zString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 90, 75, 20), drag_z.ToString (), 25);						

			int.TryParse (dragMpl_xString, out drag_x);
			int.TryParse (dragMpl_yString, out drag_y);
			int.TryParse (dragMpl_zString, out drag_z);
		
			//topspeed
			string carSpeedString;
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 90, 100, 50), "Top speed");				
			carSpeedString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 110 + guiOffset, 90, 75, 20), topSpeedcar.ToString (), 25);
			int.TryParse (carSpeedString, out topSpeedcar);
			
			//numbergears
			string numberOfGearsString;
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 130, 100, 50), "Number of gears");
			numberOfGearsString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 130, 75, 20), numberOfGears.ToString (), 25);
			int.TryParse (numberOfGearsString, out numberOfGears);
				
			//maxturn
			string maxTurnString;
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 130, 100, 50), "Max turn");
			maxTurnString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 110 + guiOffset, 130, 75, 20), maximumTurn.ToString (), 25);
			int.TryParse (maxTurnString, out maximumTurn);
			
			//minturn
			string minTurnString;
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 170, 100, 50), "Min turn");
			minTurnString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 170, 75, 20), minimumTurn.ToString (), 25);
			int.TryParse (minTurnString, out minimumTurn);
			
			//extremumSlip
			string extremumSlipString;
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 170, 100, 50), "Extremum slip");
			extremumSlipString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 110 + guiOffset, 170, 75, 20), extremumSlip.ToString (), 25);
			int.TryParse (extremumSlipString, out extremumSlip);
			
			//extremumValue
			string exValueString;
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 210, 100, 50), "Extremum value");
			exValueString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 210, 75, 20), extremumValue.ToString (), 25);
			int.TryParse (exValueString, out extremumValue);
			
			//asymptoteSlip
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 210, 100, 50), "Asymptote Slip");
			string asSlipString;
			asSlipString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 110 + guiOffset, 210, 75, 20), asymptoteSlip.ToString (), 25);
			int.TryParse (asSlipString, out asymptoteSlip);
			
			//asymptoteValue
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 250, 100, 50), "Asymptote value");
			string asValueString;
			asValueString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 250, 75, 20), asymptoteValue.ToString (), 25);
			int.TryParse (asValueString, out asymptoteValue);
			
			//stiffness
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 250, 100, 50), "Stiffness");
			string stifString;
			stifString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 110 + guiOffset, 250, 75, 20), stiffness.ToString (), 25);
			int.TryParse (stifString, out stiffness);
			
			//suspensionRange
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 290, 100, 50), "Suspension range");
			string susString;
			susString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 290, 75, 20), suspensionRange.ToString (), 25);
			int.TryParse (susString, out suspensionRange);
			
			//suspensionDamper
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 290, 100, 50), "Suspension damper");
			string damperString;			
			damperString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 110 + guiOffset, 290, 75, 20), suspensionDamper.ToString (), 25);
			int.TryParse (damperString, out suspensionDamper);
			
			//suspensionSpringFront
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 330, 100, 50), "Suspension front");
			string springFrontString;
			springFrontString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 310 + guiOffset, 330, 75, 20), suspensionSpringFront.ToString (), 25);
			int.TryParse (springFrontString, out suspensionSpringFront);
			
			//suspensionSpringRear
			GUI.Label (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 330, 100, 50), "Suspension rear");
			string springRearString;
			springRearString = GUI.TextField (new Rect (UnityEngine.Screen.width - guiWidth + 110 + guiOffset, 330, 75, 20), suspensionSpringRear.ToString (), 25);
			int.TryParse (springRearString, out suspensionSpringRear);

			if (GUI.Button (new Rect (UnityEngine.Screen.width - guiWidth + 210 + guiOffset, 370, 70, 50), "Apply")) {
				saveCarValues ();
			}
		}
		if (objectSelected == "car" || objectSelected == "grid" || objectSelected == "object") {
			if (GUI.Button (new Rect (UnityEngine.Screen.width - guiWidth + 10 + guiOffset, 370, 70, 50), lockTexture)) {
				if (guiLock) {
					lockTexture = btnLockOpen;
					guiLock = false;
				} else {
					lockTexture = btnLockClose;
					guiLock = true;
				}
			}
		}
	}
}
