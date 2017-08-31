using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CVMPanel_MajorReq : JWMonoBehaviour {
    public CVMCaveman caveman;
    public string majorname;
    public GameObject panel_AssignMajor;

    string[] courses = {"Agriculture", "Art", "Biology", "Business",
        "Chemistry", "Computer Science", "Education", "Engineering",
        "English and Communication", "Family and Consumer Science",
        "Foreign Language", "Health", "History and Culture",
        "Math and Stats", "Physics", "Physics", "Physics",
        "Physics", "Physics", "Social Science", "Technology", "Free Electives",
        "Physics", "Physics", "Physics" };

    // Use this for initialization
    void Start () {
        UpdatePanel();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    List<string> ReqCourses()
    {
        int countCourses = courses.Length;
        List<string> ret = new List<string>();
        for (int i = 0; i < 8; i++)
        {
            int currNum = Random.Range(0, countCourses);
            ret.Add(courses[currNum]);
        }
        return ret;
    }

    void UpdatePanel()
    {
        List<string> reqCourses = FindObjectOfType<CVMDataStorage>().getCourseReq(majorname);

        Text[] courseTexts = GetComponentsInChildren<Text>();
        int i = 0;
        foreach (Text text in courseTexts)
        {
            if (text.name.Contains("Text_Course"))
            {
                text.text = reqCourses[i];
                i++;
            }
        }
    }

    public void AssignMajor()
    {
        caveman.AssignMajor(majorname);
        Destroy(panel_AssignMajor);
        Destroy(gameObject);
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
