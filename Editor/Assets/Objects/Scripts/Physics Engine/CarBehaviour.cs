using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {

	private int mass; // vehicle mass (kg)
	private float gravity; // acceleration due to gravity m/s2
	private float length; //full length from front to back
	private float a, b ; //a- length from centre to front; b - length from centre to back
	private int I; //moment of intertia
	private double u; //forward velocity

	// Use this for initialization
	void CarBehaviour () {
		mass = 1150;
		gravity = 9.81;
		length = 2.66;
		a = 1.06;
		b = length - a;
		I = 1850;
		u = 80 / 3.6;
	}

	void Start() {

	}



	
	// Update is called once per frame
	void Update () {
	
	}
}
