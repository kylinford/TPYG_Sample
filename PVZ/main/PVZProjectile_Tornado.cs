using UnityEngine;
using System.Collections;

public class PVZProjectile_Tornado : PVZProjectile {
	private Vector3 initProjectilePosition;
	private bool goingBack = false;
	private float leftSpeedFast = 10.0f;
	private float force;
	//public AudioClip audioHurt;
	
	void Start(){
		speed = 2.5f;
		power = 0.0f;
		initProjectilePosition = transform.position;
	}
	
	override protected void Update () {
		base.Update ();
		//transform.Translate(Vector3.right * speed * Time.deltaTime);
		if(transform.position.x > RIGHT_THRESHOLD){
			Destroy(this.gameObject);
		}
		if (goingBack) {
			transform.position = Vector3.MoveTowards(transform.position, initProjectilePosition, (leftSpeedFast)*Time.deltaTime);
			if (transform.position == initProjectilePosition){
				Destroy(gameObject);
			}
		}
	}

	/*Split*/
	override protected void OnTriggerEnter2D(Collider2D col){
		float power = 0.6f;
		if (col.gameObject.GetComponent<PVZJob> ()) {
			if (col.gameObject.transform.position.y > LAWN_CENTER_Y + LAWNBLOCK_HEIGHT*(NUM_ROWS/2.0f-1))
				force = -power;
			else if (col.gameObject.transform.position.y < LAWN_CENTER_Y - LAWNBLOCK_HEIGHT*(NUM_ROWS/2.0f-1))
				force = power;
			else{
				int frac = Random.Range (0, 2);
				force = -power + frac*2;
			}
		}
	}
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.GetComponent<PVZJob> ()) {
			col.attachedRigidbody.AddForce(new Vector2 (0.0f,force));
		}
	}

	/*Pull back*/
	/*
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.GetComponent<Job>()){
			if (transform.position.x - col.gameObject.transform.position.x < 0.001f)
				goingBack = true;
			if (goingBack){
				col.gameObject.transform.position = new Vector3(transform.position.x, col.gameObject.transform.position.y, transform.position.z);
				col.gameObject.GetComponent<Job>().setSpeed(speed);
			}
		}
	}*/
}
