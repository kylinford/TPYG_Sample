using UnityEngine;
using System.Collections;

public class PVZProjectile : PVZMonoBehaviour {
	protected float speed = 1.0f;
	protected float power = 0.3f;

	override protected void Update () {
		base.Update ();
		transform.Translate(Vector3.right * speed * Time.deltaTime);
	}

	protected virtual void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.GetComponent<PVZJob> ()) {
			PVZJob jobScript = (PVZJob)col.gameObject.GetComponent<PVZJob> ();
			jobScript.setHealth (jobScript.getHealth () - power);
		}
	}

}
