using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PVZClient : PVZMonoBehaviour {
	public string NameClient = "Client's Name";
	public GameObject Text_Matching;
	public string[] Talks;
	public GameObject PanelInfo;
	public bool isForTutorial = false;
	
	private GameObject newPanelInfo;
	public int ID = 2;
	private string IdealJob;
	private string[] Characteristics;
	private float[] CharacteristicsValues;
	private float HighestMatchingScore = 0;
	private PVZDataStorage ds_pvz;
	private METADataStorage ds_meta;
	private int currCounsellorID;
	
	void Start () {
		ds_pvz = FindObjectOfType<TPYGDataStorageRef>().ds_pvz;
		ds_meta = FindObjectOfType<TPYGDataStorageRef> ().ds_meta;
		currCounsellorID = ds_meta.CounsellerID;
		ID = UnityEngine.Random.Range(0, NUM_JOB);
		Set (ID);
	}
	
	public string[] GetCharacteristics(){
		return Characteristics;
	}
	public string GetCharacteristics(int i){
		return Characteristics[i];
	}
	
	public string[] GetTalks(){
		return Talks;
	}
	
	void Set(int ID){
		Characteristics = new string[]{
			"Task Flexibility: ", 
			"Female Friendly: ", 
			"Midwest: ", 
			"Family Friendly: ", 
			"Verbal Skill: ", 
			"Quantitative Skill: ", 
			"Reasoning Skill: ", 
			"Salary: "};
		CharacteristicsValues = new float[Characteristics.Length];
		IdealJob = ds_pvz.getJobData ("c", ID, 0);
		for (int i = 0; i<NUM_CHARACTERISTICS; i++) {
			string value_Characteristic = ds_pvz.getJobData ("c", ID, i + 1);
			CharacteristicsValues[i] = Convert.ToSingle (value_Characteristic);
			//Characteristics [i] += CharacteristicsValues[i].ToString("P0");
		}
		
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PVZJob> ()) {
			int ID_Job = other.GetComponent<PVZJob>().GetID();
			float value_Matching = Convert.ToSingle(ds_pvz.getJobData ("m", ID, ID_Job))/100.0f;
			//Vector3 textPosition = new Vector3(transform.localPosition.x+1.0f, transform.localPosition.y, 3.0f);
			GameObject newText_Matching = InstantiateUnderParent (Text_Matching, gameObject, new Vector3 (1, 4, 0));
			newText_Matching.GetComponentInChildren<PVZText3D_Matching>().SetScore(value_Matching);

			if (HighestMatchingScore < value_Matching)
				SetHighestMatchingScore(value_Matching);
			other.GetComponent<PVZJob>().setHealth(0);
		}
	}
	
	void OnMouseDown(){
		//int i = UnityEngine.Random.Range (0, Characteristics.Length);
		//UpdateTextMeshInfo (i);
		//UpdateInfoPanel (i);
		if (Time.timeScale == 0)
			return;
		Time.timeScale = 0;
		InstantiatePanelInfo ();
		/*if (FindObjectOfType<PVZSceneManager_Tutorial2> ()) {
			PVZSceneManager_Tutorial2 sceneManager = FindObjectOfType<PVZSceneManager_Tutorial2> ();
			if (sceneManager.GetState () == 13)
				sceneManager.NextState ();
		}*/
	}
	void OnMouseUp(){
		transform.FindChild ("Text3D_Title").GetComponent<TextMesh>().text = "";
		transform.FindChild ("Text3D_Characteristics").GetComponent<TextMesh>().text = "";
		Time.timeScale = 1;
		Destroy (newPanelInfo);
		if (FindObjectOfType<PVZSceneManager_Tutorial2>()) {
			FindObjectOfType<PVZSceneManager_Tutorial2> ().ReadClient ();
		}
	}


	void InstantiatePanelInfo(){
		newPanelInfo = InstantiateUnderParent (PanelInfo, GameObject.Find ("Panel_Main"), Vector3.zero);
		Text[] texts = newPanelInfo.GetComponentsInChildren<Text> ();
		texts [0].text = NameClient;
		texts [1].text = "";
		//foreach (string characteristic in Characteristics) 
			//texts[1].text += characteristic + "\n";

		GameObject PVZAttributeBar = newPanelInfo.transform.FindChild ("PVZAttributeBar").gameObject;
		float heightPVZAttributeBar = PVZAttributeBar.GetComponent<RectTransform> ().rect.height;
		for (int i = 0; i<Characteristics.Length; i++) {
			if (i==0){
				PVZAttributeBar.GetComponent<Text>().text = Characteristics[i];
				PVZAttributeBar.GetComponentInChildren<Slider>().value = CharacteristicsValues[i];
				continue;
			}
			GameObject newPVZAttributeBar = InstantiateUnderParent(PVZAttributeBar, newPanelInfo, 
			    PVZAttributeBar.transform.localPosition - new Vector3(0, 1.4f*(float)i*heightPVZAttributeBar,0));
			newPVZAttributeBar.GetComponent<Text>().text = Characteristics[i];
			newPVZAttributeBar.GetComponentInChildren<Slider>().value = CharacteristicsValues[i];
		}
	}
	
	void UpdateTextMeshInfo(int numCharacteristics){
		transform.FindChild ("Text3D_Title").GetComponent<TextMesh>().text = IdealJob;
		transform.FindChild ("Text3D_Characteristics").GetComponent<TextMesh>().text = Characteristics [numCharacteristics];
	}
	
	void UpdateInfoPanel(int i){
		if (GameObject.Find ("Text_Talks") && GameObject.Find ("Image_Talks")) {
			GameObject.Find ("Text_Talks").GetComponent<Text> ().text = IdealJob + "\n" + GetCharacteristics (i);
			GameObject.Find ("Image_Talks").GetComponent<Image> ().sprite = GetComponent<SpriteRenderer> ().sprite;
		}
	}
	
	void SetHighestMatchingScore(float score){
		score = score < 1 ? score : 1;
		HighestMatchingScore = score;
		transform.FindChild ("Text3D_HighestScore").GetComponent<TextMesh>().text = String.Format ("{0:P2}", HighestMatchingScore);
		
		//Complete the level if all the matching scores are higher than a number.
		if (currCounsellorID < 0)//Not a main level
			return;
		PVZClient[] clients = FindObjectsOfType<PVZClient> ();
		float scoreGoal = 1.1f;
		if (!FindObjectOfType<PVZSceneManager_Tutorial2>())
			scoreGoal = ds_pvz.ScoreGoals [currCounsellorID];
		if (HighestMatchingScore > scoreGoal){
			foreach (PVZClient client in clients) {
				if (client.GetComponent<PVZClient>().HighestMatchingScore <  scoreGoal)
					return;
			}
			LevelCompleted();
		}
		if (!FindObjectOfType<PVZSceneManager_Tutorial2>())
			GetComponent<TPYGClient> ().highestMatchingScore = HighestMatchingScore;
	}
	
	public void LevelCompleted(){
		ds_meta.Counsellors [currCounsellorID].GetComponent<TPYGCounsellor> ().AddAllHelpedClient ();
		StartCoroutine( GameObject.Find ("PVZPanel_Banner").GetComponent<PVZBanner>().ShowWinningMessage("<color=red><size=30>Congrats!</size></color> " +
			"\nAll client have got ideal jobs!"));
		

		/*
		if (currCounsellorID < ds_pvz.ScoreGoals.Length - 1) {
			SceneManager.LoadScene  ("METAMinigameMap");
			ds_meta.numPVZTurretUnlocked++;
		} else {
			GameObject.Find ("Panel_Info").GetComponentInChildren<Text>().text = "Congratulations! You have finished all the tasks!";
			Time.timeScale = 0;
		}
		*/
	}
	
}
