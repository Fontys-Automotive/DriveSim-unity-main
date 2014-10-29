using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("ObjectList")]
public class ObjectList
{
	[XmlArray("SavedObjects"),XmlArrayItem("SavedObject")]
	public SavedObject[]
		savedObjects;

	public string areaFileName;

	public int vertGridSize;
	public int horGridSize;

	public Vector3 startpointpos;
	public Vector3 startpointeuler;
}
