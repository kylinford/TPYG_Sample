using UnityEngine;
using System.Collections;

public class PVZProjectile_SimpleBall : PVZProjectile {
	// Update is called once per frame

	void Start(){
		speed = 3.0f;
		power = 0.2f;
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		base.OnTriggerEnter2D (col);
		Die ();
	}
}
