using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class PVZSceneManager : PVZMonoBehaviour {
	/*Job*/
	public GameObject Job;
	private float timer_JobGeneration = 3.0f;
	protected const float MAX_JOB_GENERATE_INTERVAL = 17.0f;
	protected const float MIN_JOB_GENERATE_INTERVAL = 7.0f;

	/*UI*/
	//public GameObject Button_Skip;
	protected GameObject Panel_Info;

	/*Sun*/
	public GameObject Sun;
	protected int SunCount = 20;
	private float timer_SunGeneration = 3.0f;
	protected float MAX_SUN_GENERATE_INTERVAL = 10.0f;
	protected float MIN_SUN_GENERATE_INTERVAL = 3.0f;

	/*Turret*/
	protected int numGeneratedPlants = 0;

	/*Data*/
	protected PVZDataStorage ds_pvz;
	protected METADataStorage ds_meta;

	/*Scene*/
	private int currCounsellorID = -1;
	private bool onShovel = false;
	public bool levelCompleted = false;

	public bool OnShovel {
		get {
			return onShovel;
		}
	}

	// Use this for initialization
	protected virtual void Start () {
		ds_pvz = FindObjectOfType<TPYGDataStorageRef> ().ds_pvz;
		ds_meta = FindObjectOfType<TPYGDataStorageRef> ().ds_meta;
		ds_pvz.InitDataStorage ();

		//Pause game and show the quest
		StartCoroutine(Quest());

		currCounsellorID = ds_meta.CounsellerID;
		GameObject.Find ("Text_Sun").GetComponent<Text>().text = SunCount.ToString();
		Panel_Info = FindObjectOfType<PVZBanner> ().gameObject;
		PersonalizeByCounsellor ();
		GenerateClients ();
		GenerateSmallTurrets (NUM_TURRETS);
		PVZClient[] clients = FindObjectsOfType<PVZClient> ();
		foreach(PVZClient client in clients)
			client.enabled = true;
		Time.timeScale = 1;
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

		/*Job*/
		timer_JobGeneration -= Time.deltaTime;
		if (timer_JobGeneration <= 0 && !levelCompleted){
			if (FindObjectsOfType<PVZJob>().Length < 3)
				GenerateJob(UnityEngine.Random.Range(0,NUM_JOB));
			timer_JobGeneration = UnityEngine.Random.Range(MIN_JOB_GENERATE_INTERVAL, MAX_JOB_GENERATE_INTERVAL);		
		}
		
		/*Sun*/
		//Collect Sun
		if (Time.timeScale == 1 && !levelCompleted) {
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint,Vector2.zero);
			if ( hit.collider != null ){
				if (hit.collider.gameObject.GetComponent<PVZSun>()){
					if (!hit.collider.gameObject.GetComponent<PVZSun>().GetCollected()){
						SunCount += hit.collider.gameObject.GetComponent<PVZSun>().getSunCount();
						hit.collider.gameObject.GetComponent<PVZSun>().Collect();
						GameObject.Find ("Text_Sun").GetComponent<Text>().text = SunCount.ToString();
					}
				}
			}
		}
		//Generate Sun
		timer_SunGeneration -= Time.deltaTime;
		if (timer_SunGeneration <= 0){
			GenerateSun();
			timer_SunGeneration = UnityEngine.Random.Range(MIN_SUN_GENERATE_INTERVAL, MAX_SUN_GENERATE_INTERVAL);		
		}
		
		/*Quit Game*/
		if (levelCompleted && Input.GetMouseButtonUp (0)){
			PVZClient[] clients = FindObjectsOfType<PVZClient> ();
			foreach (PVZClient client in clients)
				Destroy (client.gameObject);
			SceneManager.LoadScene ("METAProfile");
		}
		/*if (Input.GetKey ("s")) {
			if(!GameObject.Find("Button_Skip(Clone)"))
				InstantiateUnderParent(Button_Skip, GameObject.Find("Panel_Main"), new Vector3 (426f, 268.3f, 0.0f));
		}*/
	}

	IEnumerator ChangeBackgroundTint(Color color){
		print (color);
		float transitTime = 0.2f;
		float progress = 0.0f;
		SpriteRenderer bgSR = GameObject.Find ("Background").GetComponent<SpriteRenderer> ();
		Color orignColor = bgSR.color;
		Color offsetColor = color - bgSR.color;

		while (progress < 1) {
			yield return new WaitForSeconds (Time.deltaTime);
			progress += Time.deltaTime / transitTime;
			bgSR.color = new Color(orignColor.r + offsetColor.r * progress, orignColor.g + offsetColor.g * progress, orignColor.b + offsetColor.b * progress);
			print (bgSR.color);
		}
	}

	public void ToggleShovelMode(bool b){
		onShovel = b;
		if (onShovel)
			StartCoroutine(ChangeBackgroundTint (Color.grey));
		else
			StartCoroutine(ChangeBackgroundTint (Color.white));
	}

	/*Quest*/
	IEnumerator Quest(){
		yield return null;
	}

	/*Job*/

	protected void GenerateJob(int ID, int rowi){
		float INIT_JOB_Y = LAWN_CENTER_Y + rowi * LAWNBLOCK_HEIGHT;
		float INIT_JOB_X = RIGHT_THRESHOLD-0.1f;
		GameObject job = (GameObject)Instantiate(Job, new Vector3(INIT_JOB_X, INIT_JOB_Y, 0), Quaternion.identity);
		job.GetComponent<PVZJob> ().Set (ID);
	}
	protected void GenerateJob(int ID){
		int rowi = UnityEngine.Random.Range(-NUM_ROWS/2, NUM_ROWS/2+1);
		GenerateJob(ID, rowi);
	}
	/*Turret*/
	protected void GenerateSmallTurrets(int maxcount){
		int num = 0;
		for (int i = 0; i < ds_meta.numPVZTurretUnlocked; i++) {
			GameObject turret = ds_pvz.Turrets [i];
			float turretX = LAWN_CENTER_X - LAWNBLOCK_WIDTH * (NUM_COLS / 2) + num * LAWNBLOCK_WIDTH;
			float turretY = LAWN_CENTER_Y - LAWNBLOCK_HEIGHT * (NUM_ROWS / 2.0f);
			Instantiate (turret, new Vector3 (turretX, turretY, -5.0f), Quaternion.identity);
			num++;
			if (num == maxcount)
				return;
		}
	}

	public void AddNumGeneratedPlants(int num){
		numGeneratedPlants += num;
	}

	/*Clients*/
	protected void GenerateClients(){	
		int numClients = ds_meta.numClientsInEachMinigame [ds_meta.minigameID] < ds_meta.currClients.Count
		? ds_meta.numClientsInEachMinigame [ds_meta.minigameID] : ds_meta.currClients.Count;
		for (int i = 0; i<numClients; i++) {
			//GameObject Client = ds_pvz.Client;
			GameObject Client = ds_meta.currClients[i];
			Client.GetComponent<TPYGClient>().SetVisibility (true);

			float clientX = LAWN_CENTER_X - LAWNBLOCK_WIDTH * (NUM_COLS/2 + 1);
			float clientY = LAWN_CENTER_Y + (float)(i-2) * LAWNBLOCK_HEIGHT - 4.0f;
			//Instantiate(Client, new Vector3(clientX, clientY, 0.0f), Quaternion.identity);

			Client.GetComponent<JWEffect>().destinationOffset = new Vector3(clientX, clientY, 0) 
				- Client.GetComponent<JWEffect>().InitPosition;
			Client.GetComponent<JWEffect>().destinationScale = Vector3.one;
			Client.GetComponent<SpriteRenderer>().enabled = false;
			Client.transform.FindChild("Torso").GetComponent<SpriteRenderer>().enabled = false;
			Client.transform.FindChild("Leg").GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	/*Suns*/
	protected void GenerateSun(){
		float sunX = LAWN_CENTER_X + LAWNBLOCK_WIDTH*UnityEngine.Random.Range (-NUM_COLS / 2.0f, NUM_COLS / 2.0f);
		float sunY = LAWN_CENTER_Y + LAWNBLOCK_HEIGHT*(NUM_ROWS/2.0f+2.0f);
		Instantiate (Sun, new Vector3 (sunX, sunY, Sun.transform.position.z),Quaternion.identity);
	}

	public void SetSunCount(int i){
		SunCount = i;
		GameObject.Find ("Text_Sun").GetComponent<Text>().text = SunCount.ToString();

	}
	public int GetSunCount(){
		return SunCount;
	}

	void PersonalizeByCounsellor(){
		string ret = "";
		if (currCounsellorID<0){
			ret = "Tutorial Level";

		}else{
			GameObject.Find("Background").GetComponent<SpriteRenderer>().sprite = ds_pvz.backgroudImages[ds_meta.CounsellerID];
			string goal = String.Format ("{0:P0}", ds_pvz.ScoreGoals [currCounsellorID]);
			switch (currCounsellorID) {
			case 0:
			ret = "Farley's Boat\n<size=15>At least <size=17><color=red>" + goal + "</color></size> matching job for each client.</size>"; break;
			case 1:
			ret = "Lucy's Playground\n<size=15>At least <size=17><color=red>" + goal + "</color></size> matching job for each client.</size>"; break;
			case 2:
			ret = "Coach's Football Field\n<size=15>At least <size=17><color=red>" + goal + "</color></size> matching job for each client.</size>"; break;
			case 3:
			ret = "TED Woman's Hall\n<size=15>At least <size=17><color=red>" + goal + "</color></size> matching job for each client.</size>"; break;
			case 4:
			ret = "The Sage's Heaven\n<size=15>At least <size=17><color=red>" + goal + "</color></size> matching job for each client.</size>"; break;
				
			}
		}
		Panel_Info.GetComponentInChildren<Text> ().text = ret;
	}

}

