using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CVMButton_AssignMajor : JWMonoBehaviour {

	// Use this for initialization
	void Start () {
        RandomMajor();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void RandomMajor()
    {
        CVMDataStorage ds = FindObjectOfType<CVMDataStorage>();
        int majorID = Random.Range(0, ds.CourseReq.Length / ds.CourseNames.Length);
        GetComponentInChildren<Text>().text = FindObjectOfType<CVMDataStorage>().GetPurdueMajor(majorID);
    }

    public void AssignMajor()
    {
        string majorname = GetComponentInChildren<Text>().text;
        GameObject Panel_MajorReq = Resources.Load<GameObject>("Prefab/Map/Panel_MajorReq");
        GameObject newPanel = JWInstantiate_UI(Panel_MajorReq, FindObjectOfType<Canvas>().gameObject, true);
        newPanel.GetComponent<CVMPanel_MajorReq>().caveman = FindObjectOfType<CVMPanel_AssignMajor>().caveman;
        newPanel.GetComponent<CVMPanel_MajorReq>().majorname = majorname;
        newPanel.GetComponent<CVMPanel_MajorReq>().panel_AssignMajor = FindObjectOfType<CVMPanel_AssignMajor>().gameObject;

    }
}
