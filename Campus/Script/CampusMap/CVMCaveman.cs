using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CVMCaveman : JWMonoBehaviour
{
    public static GameObject cavemanOnHand;
    public static GameObject silhouette;
    Transform startParent;
    GameObject CvmDestination;
    bool init = true;
    string major = "No Major";
    bool majorAssigned = false;
    TextMesh textmesh;
    GameObject Anim_Star;
    GameObject Anim_PieProgress;
    Sprite CAvemanSprite_Default;
    int numFail = 0;
    bool studyFinished = false;

    public bool onBuilding = false;
    public string Major{get{return major;}set{major = value;}}

    // Use this for initialization
    void Start () {
        CvmDestination = GameObject.Find("CvmDestination");
        textmesh = GetComponentInChildren<TextMesh>();
        Anim_Star = Resources.Load<GameObject>("Prefab/Map/Anim_Star");
        Anim_PieProgress = Resources.Load<GameObject>("Prefab/Map/Anim_PieProgress");
        CAvemanSprite_Default = Resources.Load<Sprite>("Art/Caveman/CAvemanSprite");
        StartCoroutine(MovetoInitDest(0.1f));
        StartCoroutine(AptitudeOverview());
    }
	
	// Update is called once per frame
	void Update () {
        if (!init)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        UpdateMajorName3DText();
    }

    IEnumerator Study()
    {
        Sprite Sprite_Sit = Resources.Load<Sprite>("Art/Caveman/Caveman_Sprite_Sit");
        Sprite Sprite_Sit_Angry = Resources.Load<Sprite>("Art/Caveman/Caveman_Sprite_Sit_Angry");
        GetComponent<SpriteRenderer>().sprite = Sprite_Sit;
        JWInstantiate(Anim_PieProgress, gameObject, new Vector3(0, 2, 0));
        yield return new WaitForSeconds(20);

        int frastrated = Random.Range(1, 2);
        if (frastrated == 1)
        {
            GetComponent<SpriteRenderer>().sprite = Sprite_Sit_Angry;
            numFail += 1;
        }
        studyFinished = true;

    }

    void BehaveAfterFrastrated()
    {
        if (numFail == 1)
        {
            Alert("Prefab/UI/Panel_Alert_trans", "This student got frastrated during study. But you have another chance to reassign major and courses for him/her.");
            StartCoroutine(MovetoInitDest(0.1f));
        }
        else
        {
            Alert("Prefab/UI/Panel_Alert_trans", "This student has chosen to drop out after getting frastrated too many time.");
        }
        onBuilding = false;
        majorAssigned = false;
        studyFinished = false;

    }

    void WalkSwitch(bool onWalk)
    {
        if (onWalk)
        {
            GetComponent<Animator>().enabled = true;
        }
        else
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = CAvemanSprite_Default;
        }
    }


    void UpdateMajorName3DText()
    {
        textmesh.text = major;
        textmesh.offsetZ = -transform.position.z-1;
    }

    public void AssignMajor(string majorname)
    {
        major = majorname;
        majorAssigned = true;

        JWInstantiate(Anim_Star, gameObject, Vector3.zero);

        print(major);
    }

    IEnumerator MovetoInitDest(float speed)
    {
        init = true;
        GetComponent<Animator>().enabled = true;
        while (transform.position != CvmDestination.transform.position)
        {
            yield return new WaitForEndOfFrame();
            transform.position = Vector3.MoveTowards(transform.position, CvmDestination.transform.position, speed);
            
        }
        init = false;
        WalkSwitch(false);
    }

    IEnumerator AptitudeOverview()
    {
        yield return new WaitUntil(()=>!init);
        StartCoroutine(InstantiatePanelAssignMajor());
    }

    void MoveOnBuilding()
    {
        GameObject Building = silhouette.GetComponent<CVMCaveman_Silhouette>().Building;
        Destroy(silhouette);
        if (Building != null)
        {
            Building.GetComponent<CVMBuilding>().AssignCaveman(gameObject);
            StartCoroutine(Study());
        }
    }

    void OnMouseUp()
    {
        /*print("Caveman OnMouseUp");
     
        if (!FindObjectOfType<CVMCampusController>())
            return;
        CVMCampusController controller = FindObjectOfType<CVMCampusController>();
        controller.UpdateBuildingsAvailability();
        controller.CavemanDragging = gameObject;
        */
        if (majorAssigned)
        {
            if (!onBuilding)
                MoveOnBuilding();
            else
            {
                if (studyFinished)
                {
                    if (numFail > 0)
                        BehaveAfterFrastrated();
                    
                }
                
            }
        }
        else
        {
            //Panel_AssignMajor

            StartCoroutine(InstantiatePanelAssignMajor());
        }

    }

    IEnumerator InstantiatePanelAssignMajor()
    {
        //Panel_AssignMajor
        if (!FindObjectOfType<CVMPanel_AssignMajor>())
        {
            GameObject Panel_AssignMajor = Resources.Load<GameObject>("Prefab/Map/Panel_AssignMajor");
            GameObject newPanel = JWInstantiate_UI(Panel_AssignMajor, FindObjectOfType<Canvas>().gameObject, true);
            yield return new WaitForEndOfFrame();
            newPanel.GetComponent<CVMPanel_AssignMajor>().caveman = this;
        }       
    }

    void OnMouseDown()
    {
        if (!majorAssigned)
            return;
        if (onBuilding)
            return;
        GameObject templateSilhouette = Resources.Load<GameObject>("Prefab/Map/Caveman_Silhouette");
        cavemanOnHand = gameObject;
        silhouette = Instantiate(templateSilhouette);
        Color originColor = silhouette.GetComponent<SpriteRenderer>().color;
        silhouette.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        silhouette.GetComponent<SpriteRenderer>().color = new Color(originColor.r, originColor.g, originColor.b, 0.5f);
    }

    void OnMouseDrag()
    {
        if (!majorAssigned)
            return;
        if (onBuilding)
            return;
        //silhouette.transform.position = Input.mousePosition;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        silhouette.transform.position = objPosition;
    }
    
}
