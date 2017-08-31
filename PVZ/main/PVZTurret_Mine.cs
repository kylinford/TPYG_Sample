using UnityEngine;
using System.Collections;

public class PVZTurret_Mine : PVZTurret {

	// Use this for initialization
	override protected void Start () {
		base.Start ();
		CANNON_HEIGHT = 0.0f;
		CANNON_LENGTH = 0.0f;
		hasMovableProjectile = false;
		sunComsumption = 10;
		CD_GROWUP = 5.0f;
		timer_Growup = CD_GROWUP;
		GetComponentInChildren<TextMesh> ().text = sunComsumption.ToString ();

	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update ();

	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.GetComponent<PVZJob> () && planted) {
			Fire ();
			Destroy (gameObject);
		}
	}
}
