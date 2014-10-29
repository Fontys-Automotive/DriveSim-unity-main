using UnityEngine;
using System.Collections;

public class StartingPoint : MonoBehaviour
{

  // Use this for initialization
  void Start ()
  {
	GameObject[] otherPoints = GameObject.FindGameObjectsWithTag ("startpoint");
	for (int i = 0; i < otherPoints.Length-1; i++) {
	  GameObject.DestroyImmediate (otherPoints [i]);
	}

  }
	
  // Update is called once per frame
  void Update ()
  {
	
  }

  public void Hide ()
  {
	this.GetComponent<MeshRenderer> ().enabled = false;
	this.transform.FindChild ("arrow").GetComponent<MeshRenderer> ().enabled = false;
  }

  public void Show ()
  {
	this.GetComponent<MeshRenderer> ().enabled = true;
	this.transform.FindChild ("arrow").GetComponent<MeshRenderer> ().enabled = true;
  }
}
