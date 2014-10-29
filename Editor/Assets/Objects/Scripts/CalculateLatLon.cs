using UnityEngine;
using System.Collections;

public class CalculateLatLon : MonoBehaviour {
	private GameObject worldPlane;
	private double lat = 0;
	private double lon = 0;

	private float groundX;
	private float groundZ;
	private float posXRelativeToGround;
	private float posZRelativeToGround;
	private float groundWidth;
	private float groundDepth;

	private double nwLat = 51.4838440020225;
	private double nwLon = 5.6807798225553325;
	private double neLat = 51.48459794409825;
	private double neLon = 5.683554298299979;
	private double seLat = 51.48385553725515;
	private double seLon = 5.684071792083513;
	private double swLat = 51.483101607793735;
	private double swLon = 5.681297353044403;

	private bool keepWriting = true;
	
	// Use this for initialization
	void Start () {
		worldPlane = GameObject.FindGameObjectsWithTag("ground")[0];
		setNewWorldWidthHeight();
		setNewWorldLatLon(new AreaFile());
	}
	
	// Update is called once per frame
	void Update () {
		calculate();
		if(!BluetoothConnect.getConnectionStarted()) {
			BluetoothConnect.startConnectionToDevice();
			keepWriting = true;
		}
		if(keepWriting) {
			BluetoothConnect.writeToFile("loc:" + lat + "x" + lon);
		}
		if(Input.GetKeyDown(KeyCode.Q) && keepWriting) {
			keepWriting = false;
			BluetoothConnect.closeConnection();
		}
	}

	/**
	 * Calculates the new lat/lon coordinates based on
	 * the position of the vehicle this class is on
	 */
	private void calculate() {
		posXRelativeToGround = this.transform.position.x - groundX;
		posZRelativeToGround = this.transform.position.z - groundZ;

		double xPercentage = posXRelativeToGround / groundWidth;
		double zPercentage = posZRelativeToGround / groundDepth;

		double westY = swLat + ((nwLat - swLat) * zPercentage);
		double eastY = seLat + ((neLat - seLat) * zPercentage);
		this.lat = westY + ((eastY - westY) * xPercentage);

		double northX = nwLon + ((neLon - nwLon) * xPercentage);
		double southX = swLon + ((seLon - swLon) * xPercentage);
		this.lon = northX + ((southX - northX) * (1.0 - zPercentage));
	}

	/**
	 * Sets a new width/height of the worldPlane.
	 * Needed to calculate the right lat/lon coordinates.
	 */
	public void setNewWorldWidthHeight() {
		if(worldPlane == null) {
			return;
		}
		groundX = worldPlane.transform.position.x - worldPlane.renderer.bounds.extents.x;
		groundZ = worldPlane.transform.position.z - worldPlane.renderer.bounds.extents.z;
		
		groundWidth = worldPlane.renderer.bounds.size.x;
		groundDepth = worldPlane.renderer.bounds.size.z;
	}

	/**
	 * Sets new lat/lon coordinates.
	 * Needed to calculate the right lat/lon coordinates.
	 * @Param areaFile - The {@code AreaFile} which contains the new lat/lon coordinates.
	 */
	public void setNewWorldLatLon(AreaFile areaFile) {
		if(areaFile.getIsEmpty()) {
			return;
		}
		this.nwLat = areaFile.getNwLat();
		this.nwLon = areaFile.getNwLon();
		this.neLat = areaFile.getNeLat();
		this.neLon = areaFile.getNeLon();
		this.seLat = areaFile.getSeLat();
		this.seLon = areaFile.getSeLon();
		this.swLat = areaFile.getSwLat();
		this.swLon = areaFile.getSwLon();
	}
}
