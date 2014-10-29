using UnityEngine;
using System.Collections;

public class WorldScript : MonoBehaviour
{
	private Texture defaultTex;
	private Texture2D overlay;
	private bool overlayShown = true;
	private bool btnPressed = false;

	// Use this for initialization
	void Start ()
	{
		defaultTex = this.renderer.material.mainTexture;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.O) && !Input.GetKey(KeyCode.LeftControl)) {
			if(!btnPressed) {
				overlayShown = !overlayShown;
				GameObject.FindGameObjectWithTag("ground").GetComponent<WorldScript>().setOverlay(null, overlayShown);
				btnPressed = true;
			}
		}
		else if(Input.GetKey (KeyCode.F12)) {
			if(!btnPressed) {
				Application.CaptureScreenshot("C:\\DriveSim_exports\\Screenshot.png");
			}
		}
		else {
			btnPressed = false;
		}
	}

	public void setOverlay(Texture2D image, bool enabled) {
		if(image != null) {
			this.overlay = image;
			overlayShown = true;
		}
		if(enabled && overlay != null) {
			this.renderer.material.mainTexture = overlay;
			this.renderer.material.mainTextureScale = new Vector2(1, 1);
		}
		else {
			this.renderer.material.mainTexture = defaultTex;
			int x = (int)(this.renderer.bounds.size.x / 15);
			int z = (int)(this.renderer.bounds.size.z / 15);
			Debug.Log("Bounds: " + x + "/" + z);
			this.renderer.material.mainTextureScale = new Vector2(x, z);
		}
	}

	public bool getOverlayShown() {
		return overlayShown;
	}
	
	public void resizePlane ()
	{
		GameObject ground = this.gameObject;
		
		//Checks the grid size and scales the ground to the same size
		float vertSize = ((GameObject)GameObject.FindGameObjectWithTag ("MainCamera")).GetComponent<GridScript> ().GetVertSize ();
		float horSize = ((GameObject)GameObject.FindGameObjectWithTag ("MainCamera")).GetComponent<GridScript> ().GetHorSize ();
		ground.transform.localScale = new Vector3 (horSize, 1, vertSize);
		
		//Align the ground plane with the grid
		float halfWidth = ground.renderer.bounds.size.x / 2;
		float halfDepth = ground.renderer.bounds.size.z / 2;
		
		ground.renderer.material.SetTextureScale("_MainTex",new Vector2(horSize/1.5f,vertSize/1.5f));
		ground.transform.position = new Vector3 (halfWidth, 0, halfDepth);

	}
}
