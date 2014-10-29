using UnityEngine;
using System.Collections;

public class PlacedObject : MonoBehaviour
{	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	public string getName ()
	{
		return this.name;
	}

	public void setName (string i)
	{
		this.name = i;
	}
}
