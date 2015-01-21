using UnityEngine;
using System.Collections;
using System.Windows.Forms;

public class Road : MonoBehaviour
{
    public enum Rule
    {
        NO_SIGN,
        c_MAXSPEED_30,
        c_MAXSPEED_50,
        c_MAXSPEED_80,
        c_NO_ENTRY_2WAY,
        c_NO_ENTRY_1WAY,

        t_PRIORITY_CROSSING,
        t_PRIORITY_LEFT,
        t_PRIORITY_RIGHT,

        stop_STOP,

        s_ADVICESPEED_30,
        s_ADVICESPEED_50,
        s_ADVICESPEED_80,

        d_PRIORITY_START,
        d_PRIORITY_END
    }
	;

    public enum RoadType
    {
        STRAIGHT,
        TURN,
        INTERSECTION,
        T_SECTION,
    }
    //public boolean hasTrafficLight = false;
    private float maxDistCheck = 5.5f;
    public float maxSpeed = 50;
    public bool hasSidewalks;
    public bool hasLightposts = false;
    public bool hasTrafficLights = false;
    private GameObject sidewalks;
    //private GameObject sidewalkLeft;
    //private GameObject sidewalkRight;
    //private GameObject lightposts;
    public Rule ruleset;
    public RoadType roadType;
    private ArrayList carsOnIntersection;
    public ArrayList problemCars;

    // Use this for initialization
    void Start()
    {
        //hasSidewalks = new bool[2];
        //		this.sidewalks = this.transform.FindChild ("Sidewalk").gameObject;
        //		this.sidewalkLeft = sidewalks.transform.FindChild ("Sidewalk_l").gameObject;
        //		this.sidewalkRight = sidewalks.transform.FindChild ("Sidewalk_r").gameObject;

        //		hasSidewalks [0] = sidewalkLeft.activeSelf;
        //		hasSidewalks [1] = sidewalkRight.activeSelf;

        //Transform lPostsT = this.transform.FindChild ("Lightposts");

        //		if (lPostsT != null) {
        //			this.lightposts = lPostsT.gameObject; 
        //		}
        //SetSidewalks (true, true);
        //this.maxSpeed = 50;

        carsOnIntersection = new ArrayList();
        problemCars = new ArrayList();
    }

    public void SetRules(Rule rules, bool showSign)
    {
        this.ruleset = rules;
        if (showSign)
        {
            this.SetSign(rules);
        }
    }

    public Rule GetRules()
    {
        return this.ruleset;
    }

    public void setMaxSpeed(float speed)
    {
        this.maxSpeed = speed;
    }

    public void SetTrafficLight(bool b)
    {
        Transform tLight = this.transform.FindChild("Verkeerslichten");
        hasTrafficLights = b;
        if (tLight != null)
        {
            tLight.gameObject.SetActive(b);
        }
    }

    public bool HasTrafficLightBoolean()
    {
        Transform tLight = this.transform.FindChild("Verkeerslichten");

        if (tLight != null)
        {
            if (tLight.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    public TrafficLight GetClosestTrafficLight(Vector3 obj)
    {
        Transform tLight = this.transform.FindChild("Verkeerslichten");

        TrafficLight closest = null;
        float prevDist = Mathf.Infinity;

        //Debug.Log (tLight.childCount);
        foreach (Transform child in tLight)
        {
            float newDist = Vector3.Distance(obj, child.position);
            //Debug.Log (dist);
            if (newDist < prevDist)
            {
                closest = child.gameObject.GetComponent<TrafficLight>();
                prevDist = newDist;
            }

        }
        return closest;
    }

    public TrafficLight HasTrafficLight()
    {
        Transform trafficLight = this.transform.FindChild("Verkeerslichten");
        if (trafficLight != null)
        {
            return trafficLight.gameObject.GetComponent<TrafficLight>();
        }
        return null;
    }

    public void SetSidewalks(bool b)
    {
        hasSidewalks = b;
        GameObject sidewalkLeft = transform.FindChild("Sidewalk").transform.FindChild("Sidewalk_l").gameObject;
        GameObject sidewalkRight = transform.FindChild("Sidewalk").transform.FindChild("Sidewalk_r").gameObject;

        //	if (sidewalkLeft != null && sidewalkRight != null) {
        sidewalkLeft.SetActive(b);
        sidewalkRight.SetActive(b);
        //	}
    }

    public bool HasSidewalks()
    {
        //hasSidewalks = new bool[2];
        return hasSidewalks;
    }

    public void SetLightposts(bool b)
    {
        hasLightposts = b;
        Transform lPostsT = transform.FindChild("Lightposts");
        if (lPostsT != null)
        {
            lPostsT.gameObject.SetActive(b);
        }
    }

    public bool HasLightposts()
    {
        Transform lPostsT = transform.FindChild("Lightposts");

        if (lPostsT != null)
        {
            return lPostsT.gameObject.activeSelf;
        }
        return false;
    }

    private void SetSign(Rule s)
    {
        //Set the sign type 
        this.ruleset = s;
        GameObject sObject;
        if (this.transform.FindChild("Verkeersborden") != null)
        {
            sObject = this.transform.FindChild("Verkeersborden").gameObject;
            foreach (Transform t in sObject.transform)
            {
                t.gameObject.SetActive(false);
            }
        }
        else
        {
            return;
        }

        //r is round
        //t is triangle
        //stop is stop
        //s is square
        //45 is diamond
        //Check what category needs to be enabled
        string shape = "";
        string[] words = new string[] { };
        if (this.ruleset != Rule.NO_SIGN)
        {
            //Pole always needs to be enabled! Unless you turn off the signs...
            sObject.transform.FindChild("Pole").gameObject.SetActive(true);

            words = this.ruleset.ToString().Split('_');
            shape = words[0];
            sObject.transform.FindChild(shape).gameObject.SetActive(true);

            sObject = sObject.transform.FindChild(shape).gameObject;

            //Now disable all signs inside a certain category
            foreach (Transform t in sObject.transform)
            {
                t.gameObject.SetActive(false);
            }
            try { sObject.transform.FindChild("Bord").gameObject.SetActive(true); }
            catch {  }
        }

        //		//Check for each sign what models need to be enabled etc.
        switch (this.ruleset.ToString())
        {
            case "c_MAXSPEED_30":
                try
                {
                    sObject.transform.FindChild("MAX_30_c").gameObject.SetActive(true);
                }
                catch {  }
                break;
            case "c_MAXSPEED_50":
                try
                {
                    sObject.transform.FindChild("MAX_50_c").gameObject.SetActive(true);
                }
                catch {  }
                break;
            case "c_MAXSPEED_80":
                try
                {
                    sObject.transform.FindChild("MAX_80_c").gameObject.SetActive(true);
                }
                catch {  }
                break;
            case "c_NO_ENTRY_2WAY":
                try
                {
                    sObject.transform.FindChild("NO_ENTRY_2WAY_c").gameObject.SetActive(true);
                }
                catch { }
                break;
            case "c_NO_ENTRY_1WAY":
                try
                {
                    sObject.transform.FindChild("NO_ENTRY_1WAY_c").gameObject.SetActive(true);
                }
                catch { }
                break;
            case "s_ADVICE_30":
                try
                {
                    //sObject.transform.FindChild("stop_STOP").gameObject.SetActive(true);
                }
                catch { }
                break;
            //case Sign.ADVICE_30_s:
            //    sObject.transform.FindChild("A4-030").gameObject.SetActive(true);
            //    break;
            //case Sign.ADVICE_50_s:
            //    sObject.transform.FindChild("A4-050").gameObject.SetActive(true);
            //    break;
            //case Sign.ADVICE_80_s:
            //    sObject.transform.FindChild("A4-080").gameObject.SetActive(true);
            //    break;
            //case Sign.PRIORITY_CROSSING_t:
            //    sObject.transform.FindChild("B3").gameObject.SetActive(true);
            //    break;
            //case Sign.PRIORITY_LEFT_t:
            //    sObject.transform.FindChild("B4").gameObject.SetActive(true);
            //    break;
            //case Sign.PRIORITY_RIGHT_t:
            //    sObject.transform.FindChild("B5").gameObject.SetActive(true);
            //    break;

            default:
                break;
        }
    }

    public ArrayList GetCarsOnIntersection()
    {
        return carsOnIntersection;
    }

    public void AddCar(AICarScript script)
    {

        if (!carsOnIntersection.Contains(script))
        {
            carsOnIntersection.Add(script);
        }
    }

    public void RemoveCar(AICarScript script)
    {
        if (carsOnIntersection.Contains(script))
        {
            carsOnIntersection.Remove(script);
        }
    }
    public void AddProblem(AICarScript script)
    {

        if (!problemCars.Contains(script))
        {
            problemCars.Add(script);
        }
    }

    public void RemoveProblem(AICarScript script)
    {
        if (problemCars.Contains(script))
        {
            problemCars.Remove(script);
        }
    }

    public ArrayList GetAdjacentRoad(GameObject roadObj)
    {
        ArrayList destinations = new ArrayList();
        Vector3 roadPos = roadObj.transform.position;
        roadPos.y += 0.01f;
        Ray rForward = new Ray(roadPos, Vector3.forward);
        Ray rLeft = new Ray(roadPos, Vector3.left);
        Ray rRight = new Ray(roadPos, Vector3.right);
        Ray rBack = new Ray(roadPos, -Vector3.forward);

        RaycastHit hit;
        if (Physics.Raycast(rForward, out hit, maxDistCheck))
        {
            if (hit.transform.gameObject.layer == 8)
            {
                GameObject cell = hit.transform.gameObject;
                destinations.Add(cell);
            }
        }

        if (Physics.Raycast(rLeft, out hit, maxDistCheck))
        {
            if (hit.transform.gameObject.layer == 8)
            {
                GameObject cell = hit.transform.gameObject;
                destinations.Add(cell);
            }
        }

        if (Physics.Raycast(rRight, out hit, maxDistCheck))
        {
            if (hit.transform.gameObject.layer == 8)
            {
                GameObject cell = hit.transform.gameObject;
                destinations.Add(cell);

            }
        }

        if (Physics.Raycast(rBack, out hit, maxDistCheck))
        {
            if (hit.transform.gameObject.layer == 8)
            {
                GameObject cell = hit.transform.gameObject;
                destinations.Add(cell);
            }
        }
        return destinations;
    }
}
