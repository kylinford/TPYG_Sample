using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*FinishedStep
1. Click clients to know them;
2. GenerateJob(); Click job icons to know more;
3. GenerateSmallTurrets(1); Drag turret to the right place;
4. Sun collection;
5. Navigation;
*/

public class PVZSceneManager_Tutorial : PVZSceneManager {
	public GameObject Panel_PauseMenu;

	private GameObject ArrowTutorial;
	private bool[] FinishedStep = new bool[100];
	private GameObject Text_Tutorial;
	bool coroutineStarted = false;
	Vector3 turretOrigPosition;

	protected override void Start () {
		base.Start ();
		for (int i=0;i<100;i++)
			FinishedStep[i] = false;
		Text_Tutorial = GameObject.Find ("Text_Tutorial");
		ArrowTutorial= GameObject.Find ("ArrowTutorial");
		/*
		Panel_PauseMenu = GameObject.Find ("Panel_PauseMenu");
		Panel_PauseMenu.SetActive (false);
		GameObject.Find ("Text_Sun").GetComponent<Text>().text = SunCount.ToString();
		Panel_Info = GameObject.Find ("Panel_Info");
		Panel_Info.GetComponentInChildren<Text>().text = "";

		GenerateClients ();
		//GenerateSmallTurrets (1);*/
	}
	// Update is called once per frame
	protected override void Update () {
		if (coroutineStarted)
			return;
		if (!FinishedStep [0]) { 
			StartCoroutine (WaitAndPrint (5.0f, "Your task is to fetch the best jobs for them", 0));
		} else if (!FinishedStep [1]) {
			StartCoroutine (WaitAndPrint (5.0f, "Tap a client to see more details", 1));
		} else if (!FinishedStep [2]) {
			if (Panel_Info.GetComponentInChildren<Text> ().text != "") {
				FinishedStep [2] = true;
				Text_Tutorial.GetComponent<Text> ().text = "It seems that you've already known how to do see client's details";
				GenerateJob (23);
			}
		} else if (!FinishedStep [3]) {
			StartCoroutine (WaitAndPrint (3.0f, "Now a job is coming!", 3));
			//Text_Tutorial.GetComponent<Text> ().text = "Now a job is coming!";
			FinishedStep [3] = true;
		} else if (!FinishedStep [4]) {
			StartCoroutine (WaitAndPrint (5.0f, "It's kind of a engineering job. Who do you think matches it the best?", 4));
		} else if (!FinishedStep [5]) {
			StartCoroutine (WaitAndPrint (5.0f, "You can use spells to help you navigate the job!", 5));
		} else if (!FinishedStep [6]) {
			GenerateSmallTurrets (1);
			turretOrigPosition = GameObject.FindObjectOfType<PVZTurret> ().transform.position;
			ArrowTutorial.GetComponent<PVZEffect_TutorialIcon> ().SetFinishedStepsAsTrue (1);
			FinishedStep [6] = true;
		} else if (!FinishedStep [7]) {
			PVZTurret turret = PVZTurret.FindObjectOfType<PVZTurret> ();
			if (turretOrigPosition != turret.transform.position) {
				ArrowTutorial.GetComponent<PVZEffect_TutorialIcon> ().SetFinishedStepsAsTrue (2);
				Text_Tutorial.GetComponent<Text> ().text = "Now drag the turret here!\n See what will happen!";
				FinishedStep [7] = true;
			}
		} else if (!FinishedStep [8]) {
			PVZTurret[] turrets = PVZTurret.FindObjectsOfType<PVZTurret> ();
			foreach (PVZTurret turret in turrets) {
				Vector3 distance = turret.transform.position - (ArrowTutorial.transform.position + new Vector3 (0.0f, -LAWNBLOCK_HEIGHT / 2.0f, 0.0f));
				if (turret.isPlanted ()) {
					if (distance.magnitude > 1.0f){
						turret.SellBack ();
						AddNumGeneratedPlants(-1);
					}
					else {
						FinishedStep [8] = true;
						ArrowTutorial.GetComponent<PVZEffect_TutorialIcon> ().SetFinishedStepsAsTrue (3);
						Text_Tutorial.GetComponent<Text> ().text = "Now have another try, drag it to anywhere you like!";
					}
				}
			}
		} else if (!FinishedStep [9]) {
			if (numGeneratedPlants>1){
				FinishedStep [9] = true;
				Destroy(ArrowTutorial.gameObject);
				Text_Tutorial.GetComponent<Text> ().text = "";
				CompletedTutorial();
			}

		}
	}

	IEnumerator WaitAndPrint(float waitTime, string str, int i_FinishedStep) {
		coroutineStarted = true;
		yield return new WaitForSeconds(waitTime);
		FinishedStep [i_FinishedStep]=true;
		Text_Tutorial.GetComponent<Text>().text = str;
		coroutineStarted = false;
	}

	
	void Dialog(string str){
		Panel_Info.GetComponentInChildren<Text> ().text = str;//"It's a attractor!\nTry to grab it!";
	}

	void CompletedTutorial(){//Called in Pause Button
		GameObject pausemenu = Instantiate (Panel_PauseMenu);
		pausemenu.transform.SetParent (GameObject.Find ("Canvas").transform);
		pausemenu.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		pausemenu.transform.position = new Vector3 (0.0f, 0.0f, 0.0f);

	}
}























