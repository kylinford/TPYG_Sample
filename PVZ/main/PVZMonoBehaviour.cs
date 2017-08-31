using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PVZMonoBehaviour : TPYGMonoBehaviour {
	protected const float LAWNBLOCK_WIDTH = 2.377f;
	protected const float LAWNBLOCK_HEIGHT = 2.359f;
	protected const float LAWN_CENTER_X = 0.37f;
	protected const float LAWN_CENTER_Y = -0.61f;
	protected const int NUM_ROWS = 5;
	protected const int NUM_COLS = 7;
	protected const float RIGHT_THRESHOLD = LAWN_CENTER_X + (NUM_COLS/2+2)*LAWNBLOCK_WIDTH;
	protected const float LEFT_THRESHOLD = -RIGHT_THRESHOLD;
	protected const int NUM_JOB = 116;
	protected const int NUM_CHARACTERISTICS = 8;
	protected const int NUM_TURRETS = 6;

	protected float FRICTION = 0.08f;


	protected virtual void Update () {
		DestroySelfIfOffBounding ();
	}


	protected bool OnTheLawn(Vector3 position){
		return	position.y < LAWN_CENTER_Y + LAWNBLOCK_HEIGHT * (NUM_ROWS / 2.0f)
			&& position.y > LAWN_CENTER_Y - LAWNBLOCK_HEIGHT * (NUM_ROWS / 2.0f)
				&& position.x < LAWN_CENTER_X + LAWNBLOCK_WIDTH * (NUM_COLS / 2.0f)
				&& position.x > LAWN_CENTER_X - LAWNBLOCK_WIDTH * (NUM_COLS / 2.0f);
	}

	protected bool OnTheLawn(GameObject go){
		return	OnTheLawn (go.transform.position);
	}

	protected bool OnTheLawn(){
		return	OnTheLawn (transform.position);
	}

	protected bool JobInFront(){
		RaycastHit2D hit = Physics2D.Raycast(transform.position 
		                                     + new Vector3(GetComponent<SpriteRenderer>().bounds.size.x/2.0f + 0.1f,0.0f,0.0f), 
		                                     Vector2.right);
		if (hit.collider != null) {
			if (hit.collider.gameObject.GetComponent<PVZJob> ())
				return true;
			else if (hit.collider.gameObject.GetComponent<PVZTurret> () ||
			         hit.collider.gameObject.GetComponent<PVZProjectile>())
				return hit.collider.gameObject.GetComponent<PVZMonoBehaviour> ().JobInFront ();
		}
		return false;
	}

	void DestroySelfIfOffBounding(){
		if (transform.position.y > LAWN_CENTER_Y + LAWNBLOCK_HEIGHT * ((float)(NUM_ROWS + 4) / 2.0f)
		    || transform.position.y < LAWN_CENTER_Y - LAWNBLOCK_HEIGHT * ((float)(NUM_ROWS + 4) / 2.0f)
			|| transform.position.x > LAWN_CENTER_X + LAWNBLOCK_WIDTH * ((float)(NUM_COLS + 4) / 2.0f)
			|| transform.position.x < LAWN_CENTER_X - LAWNBLOCK_WIDTH * ((float)(NUM_COLS + 4) / 2.0f))
			Destroy(gameObject);
	}

	public void Die(){
		Destroy(gameObject);
		if (transform.parent)
			Destroy (transform.parent.gameObject);
	}
	/*
	protected void InstantiateGameObject(GameObject ObjectType, GameObject parent, Vector3 localScale, Vector3 localPosition){
		GameObject obj = Instantiate(ObjectType);
		obj.transform.SetParent(parent.transform);
		obj.transform.localScale = localScale;
		obj.transform.localPosition = localPosition;
	}*/
}



