using UnityEngine;
using System.Collections;
using System;

public class PVZText3D_Matching : PVZMonoBehaviour {
	private float speed = 0.5f;
	private Vector3 targetPosition;
	private float score = 0.0f;

	void Start(){
		targetPosition = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update ();
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed*Time.deltaTime);
		if (transform.position == targetPosition)
			Destroy(gameObject);
	}

	public void SetScore(float s){
		score = s;
		GetComponentInChildren<TextMesh>().text = String.Format ("{0:P2}", score);

	}
}
