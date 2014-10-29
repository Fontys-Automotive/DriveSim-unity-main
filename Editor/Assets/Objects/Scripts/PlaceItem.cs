using UnityEngine;
using System.Collections;

public class PlaceItem : Object {
	private static PlaceItem instance;
	private GridCell previousCell;
	private ArrayList straightRoads = new ArrayList();
	private ArrayList bibeko50 = new ArrayList();
	private ArrayList allRoads = new ArrayList();
	private DrawRoad roadDrawer;

	private PlaceItem() {
		//Add the straightRoads in this arraylist so this class knows it has to use the DrawRoad algorithm
		straightRoads.Add(Resources.Load ("PlacedObjects/bibeko-50-recht") as GameObject);

		//Add the bibeko50 roads to it's arraylist
		//Make sure to use the same order as in DrawRoad.GetInstance().RoadTypes
		bibeko50.Add(Resources.Load ("PlacedObjects/bibeko-50-recht") as GameObject);
		bibeko50.Add(Resources.Load ("PlacedObjects/bibeko-50-bocht-klein") as GameObject);
		bibeko50.Add(Resources.Load ("PlacedObjects/bibeko-50-T-stuk") as GameObject);
		bibeko50.Add(Resources.Load ("PlacedObjects/bibeko-50-kruispunt") as GameObject);

		//Add the arraylists with one type of road to allRoads
		allRoads.Add (bibeko50);

		roadDrawer = DrawRoad.GetInstance();
	}

	public static PlaceItem GetInstance() {
		if(instance == null) {
			instance = new PlaceItem();
		}
		return instance;
	}

	public bool PlaceObject (GridCell cell, GameObject objToPlace)
	{
		if (cell != null) {
			
			if (objToPlace != null) {
				if(previousCell == null) {
					previousCell = new GridCell();
				}
				//Use drawroad in case a straightRoad is being placed
				if(straightRoads.Contains(objToPlace)) {
					//Find the corresponding roads
					int listThatContainsRoad = 0;
					for (int i = 0; i < allRoads.Count; i++) {
						if(((ArrayList)allRoads[i]).Contains(objToPlace)) {
							listThatContainsRoad = i;
							break;
						}
					}
					//Get the right roadpiece
					GameObject roadPiece = roadDrawer.GetRoadPiece(cell, previousCell, ((ArrayList)allRoads[listThatContainsRoad]));
					if(roadPiece != null) {
						objToPlace = roadPiece;
					}
					previousCell = cell;
					if (cell.addOccupant (objToPlace)) {
						return true;
					}
				}
				else {
					if (cell.addOccupant (objToPlace)) {
						previousCell = null;
						return true;
					}
				}
			}
		}
		return false;
	}
}
