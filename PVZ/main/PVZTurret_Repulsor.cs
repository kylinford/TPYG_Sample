using UnityEngine;
using System.Collections;

public class PVZTurret_Repulsor : PVZTurret {

	// Use this for initialization
	override protected void Start () {
		base.Start ();
		CD_FIRE = 15.0f;
		CANNON_LENGTH = 1.5f;
		sunComsumption = 9;
		CD_GROWUP = 2.0f;
		timer_Growup = CD_GROWUP;
		GetComponentInChildren<TextMesh> ().text = sunComsumption.ToString ();

	}
}