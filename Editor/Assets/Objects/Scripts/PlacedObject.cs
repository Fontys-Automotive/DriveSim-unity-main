using UnityEngine;
using System.Collections;

public class PlacedObject : MonoBehaviour
{
    private bool isSet = false;
    private string[] cellPlaces;
    private int cellPlacesX, cellPlacesY = 0;
    // Use this for initialization
    void Start()
    {
        
    }
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getName()
    {
        return this.name;
    }

    public void setName(string i)
    {
        this.name = i;
    }
    public string[] getCellPlaces()
    {
        return cellPlaces;
    }
    public void setReservedCells()
    {
        switch (this.name.ToLower())
        {

            case "turn":
                cellPlaces = new string[1]
                {
                    "0, 0"
                };
                break;

            case "straight":
                cellPlaces = new string[1]
                {
                    "0, 0"
                };
                break;
            case "intersection":
                cellPlaces = new string[1]
                {
                    "0, 0"
                };
                break;
            case "t-sectrion":
                cellPlaces = new string[1]
                {
                    "0, 0"
                };
                break;

            case "roundabout":
                cellPlaces = new string[4]
                {
                    "-1,-1" , "0,-1",
                    "-1, 0" , "0, 0"
                };
                break;
            case "roundabout2":
                cellPlaces = new string[4]
                {
                
                    "-1,-1"  ,"0,-1",
                    "-1, 0"  ,"0, 0"
                };
                break;

            case "round9":
                cellPlaces = new string[9]
                {
                    "-2,-2","-1,-2" , "0,-2",
                    "-2,-1","-1,-1" , "0,-1",
                    "-2, 0","-1, 0"  ,"0, 0"

                };
                break;
            default:
                break;
        }
    }
}
