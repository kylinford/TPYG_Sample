using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CVMBuilding : CVMMonoBehaviour {

    public bool hasSlot = false;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void UpdateHasSlot()
    {
        hasSlot = false;
        foreach (Transform slot in transform)
        {
            if (slot.childCount <= 0)
            {
                hasSlot = true;
                GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            }
        }

    }

    void OnMouseUp()
    {
        if (!hasSlot)
            return;

        //Assign to child slot
        if (!FindObjectOfType<CVMCampusController>())
            return;
        CVMCampusController controller = FindObjectOfType<CVMCampusController>();
        GameObject cavemanDragging = controller.CavemanDragging;
        if (cavemanDragging == null)
            return;
        foreach (Transform slot in transform)
        {
            if (slot.childCount <= 0)
            {
                cavemanDragging.transform.SetParent(slot);
                cavemanDragging.transform.localPosition = new Vector3(0, 0, cavemanDragging.transform.localPosition.z);
                break;
            }
        }
    }

    public void AssignCaveman(GameObject caveman)
    {
        foreach (Transform slot in transform)
        {
            if (slot.childCount <= 0)
            {
                caveman.transform.SetParent(slot);
                caveman.transform.localPosition = Vector3.zero;
                caveman.GetComponent<CVMCaveman>().onBuilding = true;
                break;
            }
            else
            {
                AlertBuildingFull();
            }
        }
    }


    void AlertBuildingFull()
    {
        GameObject Panel_Alert = Resources.Load<GameObject>("Prefab/Map/Panel_Alert");
        GameObject newPanel = JWInstantiate_UI(Panel_Alert, FindObjectOfType<Canvas>().gameObject, true);
        newPanel.GetComponentInChildren<Text>().text = "The building is full.";
    }
}
