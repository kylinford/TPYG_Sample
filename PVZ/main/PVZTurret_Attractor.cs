using UnityEngine;
using System.Collections;

public class PVZTurret_Attractor : PVZTurret {

	// Use this for initialization
	override protected void Start () {
		Health = 0.1f;
		DEFAULT_APPEAR_DELAY = 0.0f;
		base.Start ();
		hasMovableProjectile = false;
		CANNON_LENGTH = 1.8f;
		CANNON_HEIGHT = 0.0f;
		//GetComponentInChildren<SpriteRenderer> ().enabled = false;
		sunComsumption = 7;
		CD_GROWUP = 1.0f;
		timer_Growup = CD_GROWUP;
		GetComponentInChildren<TextMesh> ().text = sunComsumption.ToString ();

	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update ();
		if (planted) {
			AttractJobs ();
			timer_Appear-=Time.deltaTime;
			if (timer_Appear<=0){
				GetComponent<SpriteRenderer> ().enabled = true;
				gameObject.transform.GetChild(0).GetComponent<SpriteRenderer> ().enabled = true;
			}
		}
	}

	override protected void OnMouseUp(){
		base.OnMouseUp ();
		if (planted) {
			SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer spriteRenderer in spriteRenderers)
				spriteRenderer.enabled = false;
		}
	}

	/*void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PVZJob> () && planted) {
			Destroy (gameObject);
		}
	}*/

	void AttractJobs(){
		PVZJob[] jobScripts = GameObject.FindObjectsOfType<PVZJob> ();
		foreach (PVZJob jobScript in jobScripts) {
			if (jobScript.transform.position.x < transform.position.x-0.5f)
				continue;
			GameObject job = jobScript.gameObject;
			float jobSpeed = jobScript.getSpeed();
			job.transform.position = Vector3.MoveTowards(job.transform.position, transform.position, Mathf.Abs(jobSpeed) * Time.deltaTime);
		}
	}
}
