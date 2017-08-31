using UnityEngine;
using System.Collections;

public class CVMCampusController : CVMMonoBehaviour {
    GameObject cavemanDragging;
    GameObject buildingDargTo;

    public GameObject CavemanDragging { get { return cavemanDragging; } set { cavemanDragging = value; } }
    public GameObject BuildingDargTo { get { return buildingDargTo; } set { buildingDargTo = value; } }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateBuildingsAvailability()
    {
        CVMBuilding[] buildings = FindObjectsOfType<CVMBuilding>();
        foreach (CVMBuilding building in buildings)
            building.UpdateHasSlot();
    }

}
