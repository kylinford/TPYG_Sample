using UnityEngine;
using System.Collections;

public class PVZTurret_Tornado : PVZTurret {

	// Use this for initialization
	override protected void Start () {
		base.Start ();
		CD_FIRE= 0.0f;
		CANNON_HEIGHT = 0.0f;
		CANNON_LENGTH = 0.0f;
		sunComsumption = 10;
		CD_GROWUP = 1.0f;
		timer_Growup = CD_GROWUP;
		GetComponentInChildren<TextMesh> ().text = sunComsumption.ToString ();
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update ();
		if (planted) {
			StartCoroutine (WaitAndFire (0.5f));
		}
	}

	IEnumerator WaitAndFire(float time){
		yield return new WaitForSeconds (time);
		Fire ();
		Destroy(gameObject);
	}
}
