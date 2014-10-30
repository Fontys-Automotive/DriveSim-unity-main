using UnityEngine;
using System.Collections;

public class SceneryScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	int rotation = Random.Range(0,360);
		this.transform.eulerAngles = new Vector3(0,rotation,0);
		
		
		//Small deviation in local position
		int d = Random.Range(0,5);
		Vector3 pos = this.transform.position;
		this.transform.position = new Vector3(pos.x+d,pos.y+d,pos.z+d);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
