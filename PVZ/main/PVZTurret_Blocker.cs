using UnityEngine;
using System.Collections;

public class PVZTurret_Blocker : PVZTurret {

	// Use this for initialization
	override protected void Start () {
		base.Start ();
		Health = 3.0f;
		hasMovableProjectile = false;
		sunComsumption = 10;
		CD_GROWUP = 3.0f;
		timer_Growup = CD_GROWUP;
		GetComponentInChildren<TextMesh> ().text = sunComsumption.ToString ();

	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update ();

	}
}
