using UnityEngine;
using System.Collections;

public class CVMCaveman_Silhouette : MonoBehaviour {
    GameObject building;
    public GameObject Building { get { return building; } }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<CVMBuilding>())
            building = coll.gameObject;
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<CVMBuilding>())
            building = null;
    }


}
