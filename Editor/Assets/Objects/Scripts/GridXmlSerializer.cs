using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//using UnityEditor;


public class GridXmlSerializer : Object
{
	
	private SavedObject savedObject = null;
	private ObjectList objectList = null;
	private Vector3 pos;

	public void resizeGrid (int newx, int newy)
	{
		GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<SelectObject> ().guiShow = false;
		SaveXml ("resize.nlb");
		GridScript tempgrid = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();
		tempgrid.ResetGrid (newx, newy);

		var serializer = new XmlSerializer (typeof(ObjectList));
		var stream = new FileStream ("resize.nlb", FileMode.Open);
		ObjectList desObjectList = serializer.Deserialize (stream) as ObjectList;
		stream.Close ();

		GameObject.FindWithTag ("startpoint").transform.position = desObjectList.startpointpos;
		GameObject.FindWithTag ("startpoint").transform.localEulerAngles = desObjectList.startpointeuler;

		GameObject[] fullGridquads = GameObject.FindGameObjectsWithTag ("grid");

		foreach (GameObject gridquad in fullGridquads) {
			int tempindex = 0;
			foreach (SavedObject savObj in desObjectList.savedObjects) {
				GridCell cellScript = gridquad.GetComponent<GridCell> ();
				if (savObj != null) {
					if (cellScript.getX () == savObj.gridx && cellScript.getY () == savObj.gridy) {
						int tempindex2 = 0;
						foreach (string name in savObj.names) {
							//create the object in this grid
							//Place an object on it.
							GameObject copiedObject = Resources.Load ("PlacedObjects/" + name) as GameObject;
							copiedObject.transform.eulerAngles = savObj.degrees [tempindex2];
							if (savObj.isroad [tempindex2]) {
								copiedObject.GetComponent<Road> ().SetSidewalks (savObj.curb);
								copiedObject.GetComponent<Road> ().SetLightposts (savObj.lightposts);
								copiedObject.GetComponent<Road> ().SetTrafficLight (savObj.trafficlight);
								copiedObject.GetComponent<Road> ().setMaxSpeed(savObj.maxspeed);
								copiedObject.GetComponent<Road> ().SetRules(savObj.rules, false);
							}
							cellScript.addOccupant (copiedObject);
//							Debug.Log ("Occupant >" + name + "< has been added");
							tempindex2 ++;
						}
						desObjectList.savedObjects [tempindex] = null;
					}
				}
				tempindex ++;
			}
		}
	}
	
	public void ReadXml (string path)
	{
		//var path = "World.xml";
		if (path.Length > 1) {
			var serializer = new XmlSerializer (typeof(ObjectList));
			var stream = new FileStream (path, FileMode.Open);
			ObjectList desObjectList = serializer.Deserialize (stream) as ObjectList;
			stream.Close ();
			
			//Reset the grid to the loaded xml values
			GridScript tempgrid = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();

			tempgrid.ResetGrid (desObjectList.vertGridSize, desObjectList.horGridSize);
			GameObject.FindWithTag("MainScripts").GetComponent<GUIScript>().getLoadAreaFile().loadAreaFile(desObjectList.areaFileName);
	
			GameObject.FindWithTag ("startpoint").transform.position = desObjectList.startpointpos;
			GameObject.FindWithTag ("startpoint").transform.localEulerAngles = desObjectList.startpointeuler;

			GameObject[] fullGridquads = GameObject.FindGameObjectsWithTag ("grid");

			foreach (GameObject gridquad in fullGridquads) {
				int tempindex = 0;
				foreach (SavedObject savObj in desObjectList.savedObjects) {
					GridCell cellScript = gridquad.GetComponent<GridCell> ();
					if (savObj != null) {
						if (cellScript.getX () == savObj.gridx && cellScript.getY () == savObj.gridy) {
							int tempindex2 = 0;
							foreach (string name in savObj.names) {
								//create the object in this grid
								//Place an object on it.
								GameObject copiedObject = Resources.Load ("PlacedObjects/" + name) as GameObject;
								copiedObject.transform.eulerAngles = savObj.degrees [tempindex2];
								if (savObj.isroad [tempindex2]) {
									copiedObject.GetComponent<Road> ().SetSidewalks (savObj.curb);
									copiedObject.GetComponent<Road> ().SetLightposts (savObj.lightposts);
									copiedObject.GetComponent<Road> ().SetTrafficLight (savObj.trafficlight);
									copiedObject.GetComponent<Road> ().setMaxSpeed(savObj.maxspeed);
									copiedObject.GetComponent<Road> ().SetRules(savObj.rules, false);
								}
								cellScript.addOccupant (copiedObject);
								//Debug.Log ("Occupant >" + name + "< has been added");
								tempindex2 ++;
							}
							desObjectList.savedObjects [tempindex] = null;
						}
					}
					tempindex ++;
				}
			}
		}
		Debug.Log ("Xml loaded.");
	}
	
	public void SaveXml (string path)
	{
		GameObject[] fullGrid = GameObject.FindGameObjectsWithTag ("grid");
		objectList = new ObjectList ();
		GridScript tempgrid = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();
		objectList.vertGridSize = tempgrid.GetVertSize ();
		objectList.horGridSize = tempgrid.GetHorSize ();
		objectList.startpointpos = GameObject.FindWithTag ("startpoint").transform.position;
		objectList.startpointeuler = GameObject.FindWithTag ("startpoint").transform.localEulerAngles;
		objectList.areaFileName = GameObject.FindWithTag("MainScripts").GetComponent<GUIScript>().getLoadAreaFile().getAreaFileName();
		
		objectList.savedObjects = new SavedObject[(int)objectList.vertGridSize * (int)objectList.horGridSize];
		int tempindex = 0;
		foreach (GameObject x in fullGrid) {
			int occupantsCount = x.GetComponent<GridCell> ().countOccupants ();
			if (occupantsCount > 0) {
				int gridid = x.GetComponent<GridCell> ().getId ();
				savedObject = new SavedObject ();
				savedObject.names = new string[occupantsCount];
				savedObject.degrees = new Vector3[occupantsCount];
				int i = 0;
				foreach (GameObject tempobj in x.GetComponent<GridCell>().getOccupants()) {
					savedObject.names [i] = tempobj.name;
					savedObject.degrees [i] = tempobj.gameObject.transform.eulerAngles;
					if (tempobj.gameObject.layer == 8) {
						savedObject.curb = tempobj.GetComponent<Road> ().HasSidewalks ();
						savedObject.lightposts = tempobj.GetComponent<Road> ().HasLightposts ();
						savedObject.trafficlight = tempobj.GetComponent<Road> ().HasTrafficLightBoolean ();
						savedObject.isroad [i] = true;
						savedObject.maxspeed = tempobj.GetComponent<Road>().maxSpeed;
						savedObject.rules = tempobj.GetComponent<Road>().GetRules();
						//Debug.Log("sidewalk opgeslagen");
					}
					i++;
				}
				savedObject.gridid = gridid;
				savedObject.gridx = x.GetComponent<GridCell> ().getX ();
				savedObject.gridy = x.GetComponent<GridCell> ().getY ();
				objectList.savedObjects [tempindex] = savedObject;
				tempindex ++;
			}
		}
		var serializer = new XmlSerializer (typeof(ObjectList));
		//var path = "World.xml";
		var stream = new FileStream (path, FileMode.Create);
		serializer.Serialize (stream, objectList);
		stream.Close ();
		
		Debug.Log ("Xml saved.");
	}
}
