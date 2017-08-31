using UnityEngine;
using System.Collections;

public class CVMPanel_Graduated : JWMonoBehaviour {
    GameObject content;

	// Use this for initialization
	void Start () {
        content = transform.FindChild("ScrollView").FindChild("Viewport").FindChild("Content").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddGraduated()
    {
        GameObject buttonTemplate = Resources.Load<GameObject>("Prefab/Map/CVMButton_CavemanAssignedCareer");
        JWInstantiate_UI(buttonTemplate, content, true);
    }
}
