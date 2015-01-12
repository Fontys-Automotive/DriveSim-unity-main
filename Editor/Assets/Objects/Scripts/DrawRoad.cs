using UnityEngine;
using System.Collections;

public class DrawRoad : Object {
	private static DrawRoad instance;
	private GridScript gScript;
	
	private DrawRoad() {
		gScript = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();
	}

	public static DrawRoad GetInstance() {
		if(instance == null) {
			instance = new DrawRoad();
		}
		return instance;
	}

	public enum RoadTypes {
		STRAIGHT,
		CORNER,
		T_JUNCTION,
		INTERSECTION,
		ROUNDABOUT,
		ROUNDABOUT2,

	};

	public GameObject GetRoadPiece(GridCell currentGridcell, GridCell previousGridcell, ArrayList roadPieces) {
		//Check if currentGridcell contains no objects
		if(currentGridcell.getOccupants().Count == 0) {
			//Define the rotation of the roadPiece
			int x = currentGridcell.getX();
			int y = currentGridcell.getY();
			int pX = previousGridcell.getX();
			int pY = previousGridcell.getY();

			bool movedOneCell = false;


//			IEnumerator i = ((ArrayList)previousGridcell.getOccupants().Clone()).GetEnumerator();
//			while(i.MoveNext()) {
//				if(i.Current.ToString().Equals(((GameObject)roadPieces[(int)RoadTypes.STRAIGHT]))) {
//					prevRoadIsStraight = true;
//				}
//			}

			GameObject returnValue = (GameObject)roadPieces[(int)RoadTypes.STRAIGHT];
			if(x - 1 == pX && y == pY) { //From west to east
				returnValue.transform.localEulerAngles = new Vector3(270, 270, 0);
				movedOneCell = true;
			}
			else if(x + 1 == pX && y == pY) { //From east to west
				returnValue.transform.localEulerAngles = new Vector3(270, 90, 0);
				movedOneCell = true;
			}
			else if(y - 1 == pY && x == pX) { //From south to north
				returnValue.transform.localEulerAngles = new Vector3(270, 180, 0);
				movedOneCell = true;
			}
			else if(y + 1 == pY && x == pX) { //From north to south
				returnValue.transform.localEulerAngles = new Vector3(270, 0, 0);
				movedOneCell = true;
			}

			if(movedOneCell) {
				tryToCreateTurn(returnValue, currentGridcell, previousGridcell, roadPieces);
			}

			return returnValue;
		}

		return null;
	}

	private void tryToCreateTurn(GameObject placedObject, GridCell currentGridcell, GridCell previousGridcell, ArrayList roadPieces) {
		//Check if you need to replace a roadpiece for a corner
		ArrayList prevOccupants = previousGridcell.getOccupants();
		IEnumerator i = ((ArrayList)prevOccupants.Clone()).GetEnumerator();
		while(i.MoveNext()) {
			if((calcYRotation((float)((GameObject)i.Current).transform.localEulerAngles.y) != calcYRotation((float)(placedObject.transform.localEulerAngles.y))) &&
			   (calcYRotation((float)((GameObject)i.Current).transform.localEulerAngles.y) != calcYRotation((float)(placedObject.transform.localEulerAngles.y + 180)))) {
				GameObject turn = (GameObject)roadPieces[(int)RoadTypes.CORNER];
				bool[] joiningRoads = getJoiningRoads(previousGridcell, roadPieces);
				int joiningRoadsCount = 0;
				for (int c = 0; c < joiningRoads.Length; c++) {
					if(joiningRoads[c]) {
						joiningRoadsCount++;
					}
				}

				switch(joiningRoadsCount) {
				case(0):
					GameObject straight = (GameObject)roadPieces[(int)RoadTypes.STRAIGHT];
					if(currentGridcell.getX() < previousGridcell.getX ()) {
						straight.transform.localEulerAngles = new Vector3(270,90,0);
					}
					else if(currentGridcell.getX() > previousGridcell.getX ()) {
						straight.transform.localEulerAngles = new Vector3(270,270,0);
					}
					else {
						if(currentGridcell.getY () < previousGridcell.getY ()) {
							straight.transform.localEulerAngles = new Vector3(270,0,0);
						}
						else {
							straight.transform.localEulerAngles = new Vector3(270,180,0);
						}
					}
//					straight.transform.localEulerAngles = ((GameObject)i.Current).transform.localEulerAngles + new Vector3(0,90,0);
					previousGridcell.removeOccupant(prevOccupants.IndexOf(i.Current));
					previousGridcell.addOccupant(straight);
					return;
				case(1):
					if(joiningRoads[0]) {
						if(currentGridcell.getX() < previousGridcell.getX ()) {
							turn.transform.localEulerAngles = new Vector3(270,0,0);
						}
						else {
							turn.transform.localEulerAngles = new Vector3(270,90,0);
						}
					}
					else if(joiningRoads[1]) {
						if(currentGridcell.getY() < previousGridcell.getY ()) {
							turn.transform.localEulerAngles = new Vector3(270,180,0);
						}
						else {
							turn.transform.localEulerAngles = new Vector3(270,90,0);
						}
					}
					else if(joiningRoads[2]) {
						if(currentGridcell.getX() < previousGridcell.getX ()) {
							turn.transform.localEulerAngles = new Vector3(270,270,0);
						}
						else {
							turn.transform.localEulerAngles = new Vector3(270,180,0);
						}
					}
					else if(joiningRoads[3]) {
						if(currentGridcell.getY() < previousGridcell.getY ()) {
							turn.transform.localEulerAngles = new Vector3(270,270,0);
						}
						else {
							turn.transform.localEulerAngles = new Vector3(270,0,0);
						}
					}
					else {
						if(i.Current.ToString().Equals(((GameObject)roadPieces[(int)RoadTypes.STRAIGHT]))) {
							GameObject straight2 = (GameObject)roadPieces[(int)RoadTypes.STRAIGHT];
							if(currentGridcell.getX() < previousGridcell.getX ()) {
								straight2.transform.localEulerAngles = new Vector3(270,90,0);
							}
							else if(currentGridcell.getX() > previousGridcell.getX ()) {
								straight2.transform.localEulerAngles = new Vector3(270,270,0);
							}
							else {
								if(currentGridcell.getY () < previousGridcell.getY ()) {
									straight2.transform.localEulerAngles = new Vector3(270,0,0);
								}
								else {
									straight2.transform.localEulerAngles = new Vector3(270,180,0);
								}
							}
//							straight2.transform.localEulerAngles = ((GameObject)i.Current).transform.localEulerAngles + new Vector3(0,-90,0);
							previousGridcell.removeOccupant(prevOccupants.IndexOf(i.Current));
							previousGridcell.addOccupant(straight2);
							return;
						}
					}
					break;
				case(2):
					turn = (GameObject)roadPieces[(int)RoadTypes.T_JUNCTION];
					if(joiningRoads[0]) {
						if(joiningRoads[1]) {
							if(currentGridcell.getX() < previousGridcell.getX ()) {
								turn.transform.localEulerAngles = new Vector3(0,0,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,90,0);
							}
						}
						else if(joiningRoads[2]) {
							if(currentGridcell.getX() < previousGridcell.getX ()) {
								turn.transform.localEulerAngles = new Vector3(0,270,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,90,0);
							}
						}
						else {
							if(currentGridcell.getY () < previousGridcell.getY()) {
								turn.transform.localEulerAngles = new Vector3(0,270,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,0,0);
							}
						}
					}
					else if(joiningRoads[1]) {
						if(joiningRoads[2]) {
							if(currentGridcell.getY () > previousGridcell.getY ()) {
								turn.transform.localEulerAngles = new Vector3(0,90,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,180,0);
							}
						}
						else {
							if(currentGridcell.getY () > previousGridcell.getY ()) {
								turn.transform.localEulerAngles = new Vector3(0,0,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,180,0);
							}
						}
					}
					else if(joiningRoads[2]) {
						if(currentGridcell.getX() > previousGridcell.getX ()) {
							turn.transform.localEulerAngles = new Vector3(0,180,0);
						}
						else {
							turn.transform.localEulerAngles = new Vector3(0,270,0);
						}
					}
					break;
				default:
					turn = (GameObject)roadPieces[(int)RoadTypes.INTERSECTION];
					turn.transform.localEulerAngles = new Vector3(0,0,0);
					break;
				}

				previousGridcell.removeOccupant(prevOccupants.IndexOf(i.Current));
				previousGridcell.addOccupant(turn);
			}
			else {
				GameObject turn = (GameObject)roadPieces[(int)RoadTypes.T_JUNCTION];
				bool[] joiningRoads = getJoiningRoads(previousGridcell, roadPieces);
				int joiningRoadsCount = 0;
				for (int c = 0; c < joiningRoads.Length; c++) {
					if(joiningRoads[c]) {
						joiningRoadsCount++;
					}
				}
				
				switch(joiningRoadsCount) {
				case(0):
					GameObject straight2 = (GameObject)roadPieces[(int)RoadTypes.STRAIGHT];
					if(currentGridcell.getX() < previousGridcell.getX ()) {
						straight2.transform.localEulerAngles = new Vector3(270,90,0);
					}
					else if(currentGridcell.getX() > previousGridcell.getX ()) {
						straight2.transform.localEulerAngles = new Vector3(270,270,0);
					}
					else {
						if(currentGridcell.getY () < previousGridcell.getY ()) {
							straight2.transform.localEulerAngles = new Vector3(270,0,0);
						}
						else {
							straight2.transform.localEulerAngles = new Vector3(270,180,0);
						}
					}
//					straight2.transform.localEulerAngles = ((GameObject)i.Current).transform.localEulerAngles + new Vector3(0,-180,0);
					previousGridcell.removeOccupant(prevOccupants.IndexOf(i.Current));
					previousGridcell.addOccupant(straight2);
					return;
//					GameObject straight = (GameObject)roadPieces[(int)RoadTypes.STRAIGHT];
//					straight.transform.localEulerAngles = ((GameObject)i.Current).transform.localEulerAngles + new Vector3(0,-180,0);
//					previousGridcell.removeOccupant(prevOccupants.IndexOf(i.Current));
//					previousGridcell.addOccupant(straight);
//					return;
//				case(1):
//					if(joiningRoads[0]) {
//						if(currentGridcell.getX() < previousGridcell.getX ()) {
//							turn.transform.localEulerAngles = new Vector3(270,0,0);
//						}
//						else {
//							turn.transform.localEulerAngles = new Vector3(270,90,0);
//						}
//					}
//					else if(joiningRoads[1]) {
//						if(currentGridcell.getY() < previousGridcell.getY ()) {
//							turn.transform.localEulerAngles = new Vector3(270,180,0);
//						}
//						else {
//							turn.transform.localEulerAngles = new Vector3(270,90,0);
//						}
//					}
//					else if(joiningRoads[2]) {
//						if(currentGridcell.getX() < previousGridcell.getX ()) {
//							turn.transform.localEulerAngles = new Vector3(270,270,0);
//						}
//						else {
//							turn.transform.localEulerAngles = new Vector3(270,180,0);
//						}
//					}
//					else if(joiningRoads[3]) {
//						if(currentGridcell.getY() < previousGridcell.getY ()) {
//							turn.transform.localEulerAngles = new Vector3(270,270,0);
//						}
//						else {
//							turn.transform.localEulerAngles = new Vector3(270,0,0);
//						}
//					}
//					else {


//					}
//					break;
				case(2):
					if(joiningRoads[0]) {
						if(joiningRoads[1]) {
							if(currentGridcell.getX() < previousGridcell.getX ()) {
								turn.transform.localEulerAngles = new Vector3(0,0,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,90,0);
							}
						}
						else if(joiningRoads[2]) {
							if(currentGridcell.getX() < previousGridcell.getX ()) {
								turn.transform.localEulerAngles = new Vector3(0,270,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,90,0);
							}
						}
						else {
							if(currentGridcell.getY () < previousGridcell.getY()) {
								turn.transform.localEulerAngles = new Vector3(0,270,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,0,0);
							}
						}
					}
					else if(joiningRoads[1]) {
						if(joiningRoads[2]) {
							if(currentGridcell.getY () > previousGridcell.getY ()) {
								turn.transform.localEulerAngles = new Vector3(0,90,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,180,0);
							}
						}
						else {
							if(currentGridcell.getY () > previousGridcell.getY ()) {
								turn.transform.localEulerAngles = new Vector3(0,0,0);
							}
							else {
								turn.transform.localEulerAngles = new Vector3(0,180,0);
							}
						}
					}
					else if(joiningRoads[2]) {
						if(currentGridcell.getX() > previousGridcell.getX ()) {
							turn.transform.localEulerAngles = new Vector3(0,180,0);
						}
						else {
							turn.transform.localEulerAngles = new Vector3(0,270,0);
						}
					}
					break;
				case(3):
					turn = (GameObject)roadPieces[(int)RoadTypes.INTERSECTION];
					turn.transform.localEulerAngles = new Vector3(0,0,0);
					break;
				default:
					break;
				}
				if(joiningRoadsCount >= 2) {
					previousGridcell.removeOccupant(prevOccupants.IndexOf(i.Current));
					previousGridcell.addOccupant(turn);
				}
			}
		}
	}

	private bool[] getJoiningRoads(GridCell adjacentTo, ArrayList roadPieces) {
		bool[] returnValue = {false, false, false, false};
		int x = adjacentTo.getX();
		int y = adjacentTo.getY();

		GridCell north = gScript.getCell(x, y + 1);
		GridCell south = gScript.getCell(x, y - 1);
		GridCell east = gScript.getCell(x + 1, y);
		GridCell west = gScript.getCell(x - 1, y);

		if(north != null) {
			int roadPieceType = -1;
			GameObject roadPiece = null;
			for (int i = 0; i < roadPieces.Count; i++) {
				for (int j = 0; j < north.getOccupants().Count; j++) {
					if(roadPieces[i].ToString().Equals(north.getOccupants()[j].ToString())) {
						roadPieceType = i;
						roadPiece = (GameObject)north.getOccupants()[j];
					}
				}
			}

			if(roadPieceType == (int)RoadTypes.STRAIGHT) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot == 0 || yRot == 180) {
					returnValue[0] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.CORNER) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot == 180 || yRot == 270) {
					returnValue[0] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.T_JUNCTION) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot != 0) {
					returnValue[0] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.INTERSECTION) {
				returnValue[0] = true;
			}
		}

		if(south != null) {
			int roadPieceType = -1;
			GameObject roadPiece = null;
			for (int i = 0; i < roadPieces.Count; i++) {
				for (int j = 0; j < south.getOccupants().Count; j++) {
					if(roadPieces[i].ToString().Equals(south.getOccupants()[j].ToString())) {
						roadPieceType = i;
						roadPiece = (GameObject)south.getOccupants()[j];
					}
				}
			}
			
			if(roadPieceType == (int)RoadTypes.STRAIGHT) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot == 0 || yRot == 180) {
					returnValue[2] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.CORNER) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot == 0 || yRot == 90) {
					returnValue[2] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.T_JUNCTION) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot != 180) {
					returnValue[2] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.INTERSECTION) {
				returnValue[2] = true;
			}
		}

		if(east != null) {
			int roadPieceType = -1;
			GameObject roadPiece = null;
			for (int i = 0; i < roadPieces.Count; i++) {
				for (int j = 0; j < east.getOccupants().Count; j++) {
					if(roadPieces[i].ToString().Equals(east.getOccupants()[j].ToString())) {
						roadPieceType = i;
						roadPiece = (GameObject)east.getOccupants()[j];
					}
				}
			}
			
			if(roadPieceType == (int)RoadTypes.STRAIGHT) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot == 90 || yRot == 270) {
					returnValue[1] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.CORNER) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot == 270 || yRot == 0) {
					returnValue[1] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.T_JUNCTION) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot != 90) {
					returnValue[1] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.INTERSECTION) {
				returnValue[1] = true;
			}
		}

		if(west != null) {
			int roadPieceType = -1;
			GameObject roadPiece = null;
			for (int i = 0; i < roadPieces.Count; i++) {
				for (int j = 0; j < west.getOccupants().Count; j++) {
					if(roadPieces[i].ToString().Equals(west.getOccupants()[j].ToString())) {
						roadPieceType = i;
						roadPiece = (GameObject)west.getOccupants()[j];
					}
				}
			}
			
			if(roadPieceType == (int)RoadTypes.STRAIGHT) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot == 90 || yRot == 270) {
					returnValue[3] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.CORNER) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot == 90 || yRot == 180) {
					returnValue[3] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.T_JUNCTION) {
				int yRot = calcYRotation(roadPiece.transform.localEulerAngles.y);
				if(yRot != 270) {
					returnValue[3] = true;
				}
			}
			else if(roadPieceType == (int)RoadTypes.INTERSECTION) {
				returnValue[3] = true;
			}
		}

		return returnValue;
	}

	private int calcYRotation(float y) {
		while(y > 360) {
			y -= 360;
		}
		if (y > -1 && y < 1) {
				return 0;
		} else if (y > 89 && y < 91) {
				return 90;
		} else if (y > 179 && y < 181) {
				return 180;
		} else if (y > 269 && y < 271) {
				return 270;
		} else if (y > 359 && y < 361) {
				return 0;
		}
		else {
			return -1;
		}
	}












}
