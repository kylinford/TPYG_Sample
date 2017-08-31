using UnityEngine;
using System.Collections;

public class PVZGlowGrid : PVZMonoBehaviour {

	private bool applicableGrid = true;

	override protected void Update(){
		base.Update ();
	}
	
	public bool ApplicableGrid(){
		return applicableGrid;
	}

	public void SetApplicableGrid(bool b){
		applicableGrid = b;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.GetComponent<PVZTurret> ()) {
			if (other.gameObject.GetComponent<PVZTurret> ().isPlanted ()){
				applicableGrid = false;
			}
		} 
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.GetComponent<PVZTurret> ()) {
			if (other.gameObject.GetComponent<PVZTurret> ().isPlanted ()){
				applicableGrid = true;

			}
		} 
	}

}
