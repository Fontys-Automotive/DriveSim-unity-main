using UnityEngine;
using System.Collections;
using System;

public class AreaFile {
	private readonly int gridSizeX = 0;
	private readonly int gridSizeY = 0;
	private readonly double nwLat = 0;
	private readonly double nwLon = 0;
	private readonly double neLat = 0;
	private readonly double neLon = 0;
	private readonly double seLat = 0;
	private readonly double seLon = 0;
	private readonly double swLat = 0;
	private readonly double swLon = 0;

	/**
	 * Empty constructor. All variables are readonly so they can't be changed to any value other than 0.
	 * Check out AreaFile(int gridSizeX, int gridSizeY, double nwLat, double nwLon, double neLat, double neLon,
	 * double seLat, double seLon, double swLat, double swLon) if you want custom values.
	 */
	public AreaFile() {

	}

	/**
	 * AreaFile constructor. This constructor is used to make a new AreaFile which contains all variables
	 * regarding an area.
	 * @Param gridSizeX - The gridsize on the x-axis of the area.
	 * @Param gridSizeY - The gridsize on the y-axis of the area.
	 * @Param nwLat - The north-west latitude of the area.
	 * @Param nwLon - The north-west longitude of the area.
	 * @Param neLat - The north-east latitude of the area.
	 * @Param neLon - The north-east longitude of the area.
	 * @Param seLat - The south-east latitude of the area.
	 * @Param seLon - The south-east longitude of the area.
	 * @Param swLat - The south-west latitude of the area.
	 * @Param swLon - The south-west longitude of the area.
	 */
	public AreaFile(int gridSizeX, int gridSizeY, double nwLat, double nwLon, double neLat, double neLon,
	                double seLat, double seLon, double swLat, double swLon) {
		this.gridSizeX = gridSizeX;
		this.gridSizeY = gridSizeY;
		this.nwLat = nwLat;
		this.nwLon = nwLon;
		this.neLat = neLat;
		this.neLon = neLon;
		this.seLat = seLat;
		this.seLon = seLon;
		this.swLat = swLat;
		this.swLon = swLon;
	}

	/*
	 * Returns wether this {@code AreaFile} is empty (meaning that all values are 0)
	 */
	public bool getIsEmpty() {
		if(nwLat == 0 && nwLon == 0 && neLat == 0 && neLon == 0 && seLat == 0 && seLon == 0 && swLat == 0 && swLon == 0) {
			return true;
		}
		return false;
	}

	/*
	 * Returns gridSizeX
	 */
	public int getGridSizeX() {
		return gridSizeX;
	}

	/*
	 * Returns gridSizeY
	 */
	public int getGridSizeY() {
		return gridSizeY;
	}

	/*
	 * Returns nwLat
	 */
	public double getNwLat() {
		return nwLat;
	}

	/*
	 * Returns nwLon
	 */
	public double getNwLon() {
		return nwLon;
	}

	/*
	 * Returns neLat
	 */
	public double getNeLat() {
		return neLat;
	}

	/*
	 * Returns neLon
	 */
	public double getNeLon() {
		return neLon;
	}

	/*
	 * Returns seLat
	 */
	public double getSeLat() {
		return seLat;
	}

	/*
	 * Returns seLon
	 */
	public double getSeLon() {
		return seLon;
	}

	/*
	 * Returns swLat
	 */
	public double getSwLat() {
		return swLat;
	}

	/*
	 * Returns swLon
	 */
	public double getSwLon() {
		return swLon;
	}
}
