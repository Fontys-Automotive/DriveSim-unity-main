using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class SavedObject {
	
	[XmlArray("names"),XmlArrayItem("name")]
 	public string[] names;
 
 	public int gridid;
	public int gridx;
	public int gridy;
	
	public Vector3[] degrees;
		
	public bool[] isroad = new bool[4];
	public bool curb = false;
	public bool lightposts = false;
	public bool trafficlight = false;
	public float maxspeed = 50;
	public Road.Rule rules;

	
}
