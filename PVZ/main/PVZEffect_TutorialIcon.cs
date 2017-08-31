using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PVZEffect_TutorialIcon : PVZMonoBehaviour {
	float timer_blink = 1.0f;
	float init_timer_alpha;
	bool inverse_Blink = false;
	bool movingUp = false;
	int stepi = 0;
	int stepN = 300;
	Vector3 posUp;
	Vector3 posDown;
	PVZJob job;
	PVZTurret turret;
	bool[] finishedSteps;
	Vector3 firstTurretOverPos;
	// Use this for initialization
	void Start () {
		finishedSteps = new bool[100];
		for (int i=0; i<100; i++) {
			finishedSteps[i] = false;
		}
		init_timer_alpha = timer_blink;
		posUp = new Vector3 (LAWN_CENTER_X - 3.0f * LAWNBLOCK_WIDTH, LAWN_CENTER_Y + 2.0f * LAWNBLOCK_HEIGHT);
		posDown = new Vector3 (LAWN_CENTER_X - 3.0f * LAWNBLOCK_WIDTH, LAWN_CENTER_Y - 2.0f * LAWNBLOCK_HEIGHT);
		transform.Rotate (0.0f, 0.0f, -90.0f);
	}
	
	// Update is called once per frame
	override protected void Update () {
		if (!finishedSteps [0]) {
			//blink
			blink ();
			//Moving Up and Down
			MovingBetween (posUp, posDown);
			if (GameObject.FindObjectOfType<PVZJob> ())
				finishedSteps [0] = true;
		} else if (!finishedSteps [1]) {
			//blink
			blink ();
			if (job == null) {
				job = PVZJob.FindObjectOfType<PVZJob> ();
				transform.Rotate (0.0f, 0.0f, 180.0f);
			}
			transform.position = job.transform.position + new Vector3 (-LAWNBLOCK_WIDTH, 0.0f, 0.0f);

		} else if (!finishedSteps [2]) {
			if (turret == null) {
				turret = PVZTurret.FindObjectOfType<PVZTurret> ();
				transform.Rotate (0.0f, 0.0f, -90.0f);
				transform.position = turret.transform.position + new Vector3 (0.0f, LAWNBLOCK_HEIGHT, 0.0f);
				firstTurretOverPos = transform.position;
			}
			blink ();
		} else if (!finishedSteps [3]) {
			Vector3 newPos = new Vector3 (-4.27f, 0.74f, 0.0f);
			if (transform.position != newPos)
				transform.position = newPos;
			blink ();
		} else if (!finishedSteps [4]) {
			Vector3 newPos = firstTurretOverPos;
			if (transform.position != newPos)
				transform.position = newPos;
			blink ();
		}

	}

	public void SetFinishedStepsAsTrue(int num){
		for (int i=0; i<=num; i++)
			finishedSteps [i] = true;
	}

	void blink(){
		if (!inverse_Blink){//must be called in Update()
			timer_blink -= Time.deltaTime;
			if (timer_blink < 0)
				inverse_Blink = true;
		} else {
			timer_blink += Time.deltaTime;
			if (timer_blink > init_timer_alpha)
				inverse_Blink = false;
		}
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1.0f - timer_blink / init_timer_alpha);
	}


	void MovingBetween (Vector3 vUp, Vector3 vDown){//must be called in Update()
		//Move between points
		if (!movingUp) {
			stepi++;
			if (stepi == stepN) {
				movingUp = true;
				stepi = 0;
			}
			Vector3 currPostion = vUp + (vDown - vUp) * ((float)stepi / (float)stepN);
			transform.position = currPostion;
		} else {
			stepi++;
			if (stepi == stepN) {
				movingUp = false;
				stepi = 0;
			}
			Vector3 currPostion = vDown + (vUp - vDown) * ((float)stepi / (float)stepN);
			transform.position = currPostion;
		}
	}
}
