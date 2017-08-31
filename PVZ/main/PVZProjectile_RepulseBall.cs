using UnityEngine;
using System.Collections;

public class PVZProjectile_RepulseBall : PVZProjectile {
	private float lifeTimer;
	private bool collided = false;
	private const float DEFAULT_FADE_DELAY = 5.0f;
	//public AudioClip audioHurt;

	void Start(){
		speed = 1.0f;
		power = 0.0f;
		lifeTimer = DEFAULT_FADE_DELAY;
	}

	override protected void Update () {
		base.Update ();

		if (collided) {
			if(lifeTimer>0)
				lifeTimer -= Time.deltaTime;
			if (lifeTimer <= 0)
				Destroy(this.gameObject);
		}

	}
	
	 void OnTriggerStay2D(Collider2D col){
		PVZJob jobScript = (PVZJob) col.gameObject.GetComponent<PVZJob>();
		if (col.gameObject.GetComponent<PVZJob>()){
			collided = true;
			jobScript.setSpeed(-speed);
			//lifeTimer = DEFAULT_FADE_DELAY;
		}
	}
}
