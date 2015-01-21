using UnityEngine;
using System.Collections;

public class AICarScript : MonoBehaviour
{
	private GameObject goal;
	public enum Direction
	{

		LEFT,
		RIGHT,
		FORWARD
	}
	private float KM_FACTOR = 0.010f;
	private float currentSpeed = 0f;
	private float acceleration = 0.002f;
	private float deacceleration = 0.05f;
	private float baseSpeed = 0.83f;
	private float distToStop = 10;
	private Vector3 startingPos;
	private Quaternion startingRot;
	private ArrayList currentRoute;
	private int progress = 0;
	private bool isPlaced;
	private bool routePlanningMode;
	private bool driving;
	private float personalMaxSpeed;
	private bool canPlace = false;
	private bool repeat = true;
	private float timewaited = 0;
	private GameObject lastOfPreviousRoute = null;
	// Use this for initialization
	void Start ()
	{
		driving = false;
		currentRoute = new ArrayList ();
		startingPos = this.transform.position;
	}
	
	public void StartRandomPlotting ()
	{
		currentRoute = PlotRandomRoute (99);
	}
	
	// Update is called once per frame
	void Update ()
	{    
		//Debug.DrawRay (this.transform.position, Vector3.forward * 20f);
		if (routePlanningMode) {
			//If you are in route planning mode, clicking on a road piece 
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			int mask = (1 << 8);

			if (Input.GetMouseButtonDown (0)) {
				//Layermask
				//Test only on layer 8, the road layer.

				RaycastHit hit;

				if (canPlace)
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, mask)) {
					currentRoute.Add (hit.transform.gameObject);
				} else {
					routePlanningMode = false;
				}
			}
			canPlace = true;
			if (Input.GetMouseButtonDown (1)) {	
				RaycastHit hit;
				
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, mask)) {
					currentRoute.Remove (hit.transform.gameObject);
				}
			}
		} else {
			canPlace = false;
		}
			
		if (Input.GetKeyDown (KeyCode.Z)) {
			if(driving == false){
			driving = true;
			routePlanningMode = false;
			if (currentRoute == null || currentRoute.Count == 0) {
				int routeLength = Random.Range (20, 40);
				currentRoute = PlotRandomRoute (routeLength);
			}
			}
			else
			{
				driving = false;
				this.transform.position = startingPos;
			}

		}

		if (driving) {
			if (currentRoute != null && currentRoute.Count > 0) {	

				//If you finish driving to object i...
				if (Drive ((GameObject)currentRoute [progress])) {
				
					//Go to next object.
					if (progress < currentRoute.Count - 1) {
						progress++;
					} else {
						//CAR IS DONE DRIVING. FINISH HERE. CLEAN UP, ETC
						progress = 0;

						if (repeat) {
							currentRoute = PlotRandomRoute (99);
						} else {
							this.transform.position = startingPos;
							this.transform.rotation = startingRot;
							driving = false;
							currentSpeed = 0f;
						}
						//currentRoute = new ArrayList ();

						//int routeLength = Random.Range (20, 40);
						//currentRoute = PlotRandomRoute (routeLength);
					}
				}
			}
		}
	}
	
	public bool Drive (GameObject tgt)
	{		
		if (tgt == null) {
			return false;
		}

		Vector3 carPos = this.transform.FindChild ("AIcar").position;
		carPos.y = 0.7f;

		//Position and rotation while driving.
		Vector3 pos = tgt.transform.position;
		pos.y = this.transform.position.y;
		float dist = Vector3.Distance (this.transform.position, pos);
		
		Quaternion rot = Quaternion.LookRotation (pos - transform.position);
		Quaternion rotrot = Quaternion.Slerp (this.transform.rotation, rot, Time.deltaTime * 6);
		this.transform.rotation = rotrot;

		//Movement.
		//speed 0.1 = 1,7s p/ 10m = 5,96484km/u
		//Random deviation of 10km below or over max speed. Convert km to unity units with above formula.
		// Use 0.02f multiplier for km > unit.
	
		Road nextRoad = tgt.GetComponent<Road> ();		
		float maxSpeed = KM_FACTOR * nextRoad.maxSpeed;
		//Debug.Log ("maxspeed is " + maxSpeed);
		//Slow down at bends
	
		personalMaxSpeed = maxSpeed;
		
		int r = Random.Range (-3, 3);
		float rFac = r * KM_FACTOR;
		if ((personalMaxSpeed + rFac) < 0) {
			//personalMaxSpeed = 0;
		} else {
			personalMaxSpeed += rFac;
		}

		//FROM HERE ON, MAKE ADJUSTMENTS TO THE ACTUAL PERSONALMAXSPEED WITH ROAD RULES
		////////////////////////////////////////////////////
		////////////////////////////////////////////////////
		if (nextRoad.roadType == Road.RoadType.TURN || nextRoad.roadType == Road.RoadType.INTERSECTION || nextRoad.roadType == Road.RoadType.T_SECTION) {
			personalMaxSpeed = 20 * KM_FACTOR;
		}

		//Special roadtypes, special behaviour
		//Stopping for stopsign.
		if (nextRoad.ruleset == Road.Rule.stop_STOP) {
			personalMaxSpeed = 0.01f;
		}

		//Debug.Log (GetTargetDirection (g));
		//PRIORITY STUFF
		//if (nextRoad.roadType == Road.RoadType.INTERSECTION || nextRoad.roadType == Road.RoadType.T_SECTION) {
            
		//This will only be done once, check method itself. Can only be added once.

		ArrayList oppCars = nextRoad.GetCarsOnIntersection ();
		GameObject futureRoad = null;
		if (progress + 1 < currentRoute.Count - 1) {
			futureRoad = currentRoute [progress + 1] as GameObject;
		}

		if (futureRoad == null) {
			return true;
		}
		//GetTargetDirection (futureRoad);
		Debug.DrawRay (futureRoad.transform	.position, Vector3.up * 20, Color.blue);
		//
		//Traffic Light
		//
		bool hasLight = nextRoad.HasTrafficLightBoolean ();
		if (hasLight) { 
			TrafficLight myLight = nextRoad.GetClosestTrafficLight (carPos);
				
			//If it's not null it means a traffic light is coming up.
			if (myLight.getCurrentLight () == TrafficLight.LIGHT.RED) {
				//If the light is red, stop.
				personalMaxSpeed = 0;
				nextRoad.RemoveCar (this);
				nextRoad.RemoveProblem (this);
			} else {
				//The light is green
				nextRoad.AddCar (this);
				//nextRoad.AddProblem (this);
				//foreach (AICarScript c in oppCars) {


				if (oppCars.Count > 1) {
					if (GetTargetDirection (futureRoad) == Direction.LEFT) {
						personalMaxSpeed = 0;
						nextRoad.AddProblem (this);
					}

					if (nextRoad.problemCars.Count == oppCars.Count) {
						AICarScript car = nextRoad.problemCars [0] as AICarScript;
						car.personalMaxSpeed = maxSpeed;
						nextRoad.RemoveProblem (this);
					}
				}



				//if (c != this) {
				//Check if I have to turn left (Which means I have to wait)
//				if (nextRoad.problemCars.Count < oppCars.Count) {
//					//If I need to make a left turn, wait until I am last.
//					if (GetTargetDirection (futureRoad) == Direction.LEFT) {
//						//Wait until I am the last one
//						Debug.DrawRay (transform.position, Vector3.up * 20, Color.cyan);
//						personalMaxSpeed = 0;
//						nextRoad.AddProblem (this);
//					}
//				} else {
//					AICarScript car = nextRoad.problemCars [0] as AICarScript;
//					car.personalMaxSpeed = maxSpeed;
//					nextRoad.RemoveProblem (car);
//				}			
				//}           
				//}
			}
		}

		if (nextRoad.roadType == Road.RoadType.INTERSECTION || nextRoad.roadType == Road.RoadType.T_SECTION) {
			//Failsafe

			timewaited += 1 * Time.deltaTime;
			if (timewaited > 20) {

				personalMaxSpeed = maxSpeed;

			}

			if (!hasLight) {
				nextRoad.AddCar (this);

//				if (oppCars.Count > 1) {
//
//
//				}

				foreach (AICarScript c in oppCars) {
					//If there are only 3 cars, there is no problem
					if (c != this) {
//					if (GetTargetDirection (c.gameObject) == Direction.FORWARD && GetTargetDirection (futureRoad) == Direction.LEFT) {
//						personalMaxSpeed = 0;
//					}

						if (c == null) {
							break;
						}

						if (nextRoad.problemCars.Count < oppCars.Count) {
							if (GetTargetDirection (c.gameObject) == Direction.FORWARD && GetTargetDirection (futureRoad) == Direction.LEFT) {
								//Wait until I am the last one
								Debug.DrawRay (transform.position, Vector3.up * 20, Color.green);
								personalMaxSpeed = 0;
								nextRoad.AddProblem (this);
							}

							if (GetTargetDirection (c.gameObject) == Direction.RIGHT) {
								//Wait until it is gone
								personalMaxSpeed = 0;
								nextRoad.AddProblem (this);
								//break;
							}
						} else {

							//if (nextRoad.problemCars.Count == oppCars.Count) {
							//If there are 4 cars that need to wait, just pick the first one so the count decreases
							AICarScript car = oppCars [0] as AICarScript;

							//Debug.DrawRay (car.transform.position, Vector3.up * 30, Color.blue);
							car.personalMaxSpeed = maxSpeed;
							nextRoad.RemoveProblem (car);
						}
					}
				}
			}
		} else {
			if (timewaited > 20) {
				timewaited = 0;
			}
		}
		//Check 100m in front of yourself. //0.002f
		Ray ray = new Ray (carPos, transform.forward);
		RaycastHit hit;
		Debug.DrawRay (carPos, transform.forward * 25f);
		if (Physics.Raycast (ray, out hit, 25f)) {
			//If something is in front of you, drive slower.
			float distToFront = Vector3.Distance (carPos, hit.transform.position);
			//Debug.Log ("dist is " + distToFront);

			if (distToFront < 7) {
				personalMaxSpeed = 0;
			} else {
				if (!hasLight) {
					personalMaxSpeed = (distToFront) * KM_FACTOR;
				}
			}
		}
		//Also do this if there are no traffic lights.
//				if (oppCars.Count > 1) {
//					//Do I need to go left?
//					if (GetTargetDirection (g) == Direction.LEFT) {
//						//Wait until there are no more cars.
//						//personalMaxSpeed = 0;
//					}
//
//					int carCount = 0;
//					foreach (AICarScript c in oppCars) {
//						if (GetTargetDirection (c.gameObject) == Direction.RIGHT) {
//							//Wait until it is gone
//							personalMaxSpeed = 0;
//							break;
//						}
//
//						//If all cars are standing still...
//						if (c.personalMaxSpeed == 0) {
//							carCount++;
//						}
//						if (oppCars.Count == carCount) {
//							AICarScript cc = oppCars [0] as AICarScript;
//							cc.personalMaxSpeed = 5;
//						}
//					}
//				}

				
		//Actual acceleration/deacceleration
		if (currentSpeed < personalMaxSpeed) {
			currentSpeed += acceleration;

			if (currentSpeed <= 0) {
				currentSpeed = 0;
			}
		} else {

			if (currentSpeed > 0) {
				currentSpeed -= deacceleration;
			} else {
				currentSpeed = 0;
			}
		}	

		this.transform.Translate (Vector3.forward * currentSpeed);

		float distVar = 2f;
		if (nextRoad.roadType == Road.RoadType.STRAIGHT) {
			distVar = 2;
		}

		//If you are nearing an intersection, start the turning earlier.
		if (dist < distVar) {	
			//Finished driving to obj

			//Remove if I was on the intersection, but the method itself will clean up.
			nextRoad.RemoveCar (this);
			nextRoad.RemoveProblem (this);
			return true;
		} else {
			return false;
		}
	}

	public Direction GetTargetDirection (GameObject trgt)
	{

		float turnMargin = 5f;

		Vector3 relPos = this.transform.InverseTransformPoint (trgt.transform.position);

		if (relPos.x < -turnMargin) {
			//Debug.Log ("Going left");
			return Direction.LEFT;
		}
		if (relPos.x > turnMargin) {
			//Debug.Log ("Going Right");
			return Direction.RIGHT;
		}

		//Going straight
		//Debug.Log ("Going Forward");
		return Direction.FORWARD;

	}
	void OnGUI ()
	{
//		GUI.backgroundColor = Color.black;
//		Vector3 c = Camera.main.WorldToScreenPoint (this.transform.position);
//		Rect xxx = new Rect (0, 0, 150, 50);
//		xxx.x = c.x;
//		xxx.y = Screen.height - (c.y) - 10;
//		GUI.backgroundColor = Color.white;
//		GUI.Box (xxx, "cr:" + currentSpeed + " -- mx:" + personalMaxSpeed);

		if (routePlanningMode) {
			//Show car is selected
			GUI.backgroundColor = Color.black;
			Vector3 cPos = Camera.main.WorldToScreenPoint (this.transform.position);
			Rect rc = new Rect (0, 0, 70, 20);
			rc.x = cPos.x;
			rc.y = Screen.height - (cPos.y) - 10;
			GUI.Box (rc, "Selected");
			GUI.backgroundColor = Color.white;

			if (currentRoute != null) {
				int i = 1;

				foreach (GameObject r in currentRoute) {
					Vector3 point = Camera.main.WorldToScreenPoint (r.transform.position);
					GUI.backgroundColor = Color.black;	
					Rect rect = new Rect (0, 0, 20, 20);
					rect.x = point.x;
					rect.y = Screen.height - (point.y - 5);
					GUI.Box (rect, i.ToString ());
					i++;				
					GUI.backgroundColor = Color.white;
				}
			}
		}
	}


	void OnMouseDown ()
	{
		if (!routePlanningMode) {
			routePlanningMode = true;
		} else {
			routePlanningMode = false;
		}
	}

	public ArrayList PlotRandomRoute (int amount)
	{
		ArrayList route = new ArrayList ();
		
		//Draw a ray straight down to find your starting point...
		Vector3 pos = this.transform.position;
		//pos.y +=1;
		Ray ray = new Ray (this.transform.position, Vector3.down);
		Debug.DrawRay (pos, Vector3.down * 5);
		
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 5)) {
			//If you're on the road, use that as starting point.
			if (hit.transform.gameObject.layer == 8) {	
				route.Add (hit.transform.gameObject);				
			}
		}
		
		//Only continue if you found a starting point.
		if (route.Count > 0) {
			for (int i = 0; i < amount-1; i++) {
				//Current road
				GameObject currentWay = (GameObject)route [i];
				//Next road
				ArrayList options = currentWay.GetComponent<Road> ().GetAdjacentRoad (currentWay);		
				
				if (options.Count < 1) {
					return route;
				}

				foreach (GameObject c in options) {
					if (lastOfPreviousRoute != null) {
						if (lastOfPreviousRoute.transform.position == c.transform.position) {
							options.Remove (c);
							break;
						}

					}
				}
				//Previous road
				GameObject previous = null;
				if (i > 0) {
					previous = (GameObject)route [i - 1];	
				}		

				//Randomly select one of the road options
				int r = Random.Range (0, options.Count - 1);
				GameObject nextWay = (GameObject)options [r];	

				//Secure way of checking if previous way and next way are the same
				if (previous != null) {
					if (previous.transform.position == nextWay.transform.position) {
						nextWay = (GameObject)options [options.Count - 1];				
					}
				}

				//DEBUG
				//nextWay.GetComponentInChildren<Renderer> ().material.color = Color.red;
				//DEBUG
				route.Add (nextWay);
			}

			lastOfPreviousRoute = route [route.Count - 1] as GameObject;
			//Debug.DrawRay (lastOfPreviousRoute.transform.position, Vector3.up * 20, Color.green);
		}


		return route;
	}
}