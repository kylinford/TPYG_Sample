using UnityEngine;
using System.Collections;

public class PVZSun : PVZMonoBehaviour {
	public Sprite[] images;
	private int SunCount = 10;
	private Vector3 TargetPosition_Fall;
	private Vector3 TargetPosition_Collected = new Vector3(-9.09f, 6.94f, -5.0f);
	private const float DEFAULT_SPEED_FALL = 1.0f;
	private const float DEFAULT_SPEED_COLLECTED = 7.0f;
	private float Speed_Fall = DEFAULT_SPEED_FALL;
	private float Speed_Collected = DEFAULT_SPEED_COLLECTED;
	private bool collected = false;
	private bool arrivedGround = false;
	private float fadeDelay = 3.0f;

	// Use this for initialization
	void Start () {
		float Y_TargetPosition_Fall = LAWN_CENTER_Y + Random.Range (-NUM_ROWS / 2, NUM_ROWS / 2) * LAWNBLOCK_HEIGHT;
		TargetPosition_Fall = new Vector3(transform.position.x, Y_TargetPosition_Fall, transform.position.z);
		METADataStorage ds_meta = FindObjectOfType<TPYGDataStorageRef> ().ds_meta;

		int imageIndex = 1;//For tutorial level
		if (!FindObjectOfType<PVZSceneManager_Tutorial2> ())
			imageIndex = ds_meta.minigameID;
		GetComponent<SpriteRenderer> ().sprite = images [imageIndex];
	}
	
	// Update is called once per frame
	override protected void Update () {
		if (FindObjectOfType<PVZSceneManager> ().levelCompleted)
			return;
		
		base.Update ();
		if (!collected){
			transform.position = Vector3.MoveTowards(transform.position, TargetPosition_Fall, Speed_Fall*Time.deltaTime);
			if (transform.position == TargetPosition_Fall)
				arrivedGround = true;
			
			/*Timer*/
			if (arrivedGround){
				fadeDelay -= Time.deltaTime;
				if (fadeDelay <= 0){
					Destroy(gameObject);
				}
			}
		}
		
		else if (collected){
			transform.position = Vector3.MoveTowards(transform.position, TargetPosition_Collected, Speed_Collected*Time.deltaTime);
			if (transform.position == TargetPosition_Collected){
				//Camera.main.GetComponent<ActionManager_Sun>().UpdateSunTextDisplay();
				//AudioSource.PlayClipAtPoint(Clip_SunFinish,transform.position);
				Destroy(gameObject);
			}
		}
	}

	public bool GetCollected(){
		return collected;
	}

	public int getSunCount(){
		return SunCount;
	}

	public void Collect(){
		collected = true;
	}

	public void SetSpeed_Fall(float speed){
		Speed_Fall = speed;
	}

	public float GetDefaultSpeed_Fall(){
		return DEFAULT_SPEED_FALL;
	}
}
