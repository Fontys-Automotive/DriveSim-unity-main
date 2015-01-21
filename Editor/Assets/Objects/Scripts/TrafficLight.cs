using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour
{
	public enum LIGHT
	{
		RED = 0,
		ORANGE = 1,
		GREEN = 2
	}

	private float timeOnRed = 7f;
	private float timeOnOrange = 3f;
	private float timeOnGreen = 8f;

	//0 = red
	//1 = orange
	//2 = green

	private LIGHT currentLight = LIGHT.RED;
	private GameObject redLight;
	private GameObject orangeLight;
	private GameObject greenLight;
	public TrafficLight opposingLight;
	public TrafficLight connectedLight;
	private float time = 0;

	// Use this for initialization
	void Start ()
	{
		redLight = this.transform.FindChild ("Licht001").FindChild ("Light").gameObject;
		orangeLight = this.transform.FindChild ("Licht002").FindChild ("Light").gameObject;
		greenLight = this.transform.FindChild ("Licht003").FindChild ("Light").gameObject;

//		redLight = this.transform.FindChild ("Licht001").FindChild ("Light").GetComponent<Light> ();
//		orangeLight = this.transform.FindChild ("Licht002").FindChild ("Light").GetComponent<Light> ();
//		greenLight = this.transform.FindChild ("Licht003").FindChild ("Light").GetComponent<Light> ();
		redOn ();
		if (opposingLight != null) {
			opposingLight.setConnectedLight (this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
//If the other light is green, you may not turn green!

		if (opposingLight != null) {
			if (opposingLight.getCurrentLight () == LIGHT.GREEN) {
				redOn ();
				time = 3;
			} else {
				time += Time.deltaTime;
			}
		}

//		if (connectedLight.getCurrentLight () == LIGHT.RED) {
//			this.redOn ();
//			time = 0;
//		}
		//time += Time.deltaTime;

		switch (getCurrentLight ()) {
		case LIGHT.RED:
			if (time > timeOnRed) {
				greenOn ();
				time = 0;
			}
			break;
		case LIGHT.ORANGE:
			if (time > timeOnOrange) {
				redOn ();
				time = 0;
			}
			break;
		case LIGHT.GREEN:
			if (time > timeOnGreen) {
				orangeOn ();
				time = 0;
			}
			break;
		default:
			break;
		}
	}

	public LIGHT getCurrentLight ()
	{
		if (redLight != null && orangeLight != null && greenLight != null)
        {
			if (redLight.activeSelf) {
				currentLight = LIGHT.RED;
			}
			if (orangeLight.activeSelf) {
				currentLight = LIGHT.ORANGE;
			}
			if (greenLight.activeSelf) {
				currentLight = LIGHT.GREEN;
			}
			return currentLight;
		}
		return currentLight;
	}

	public void redOn ()
	{
		redLight.SetActive (true);
		orangeLight.SetActive (false);
		greenLight.SetActive (false);
	}

	public void orangeOn ()
	{
		redLight.SetActive (false);
		orangeLight.SetActive (true);
		greenLight.SetActive (false);	
	}
	public void greenOn ()
	{
		redLight.SetActive (false);
		orangeLight.SetActive (false);
		greenLight.SetActive (true);
	}

	public void setConnectedLight (GameObject tLight)
	{
		TrafficLight l = tLight.GetComponent<TrafficLight> ();
		this.opposingLight = l;
	}
}
