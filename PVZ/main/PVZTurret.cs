using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PVZTurret : PVZMonoBehaviour {
	//public Sprite sprite;
	public GameObject Projectile;
	public GameObject Effect;

	protected int sunComsumption = 10;
	protected float Health = 1.0f;
	protected float CANNON_LENGTH = 1.15f;
	protected float CANNON_HEIGHT= 0.34f;
	protected float CD_GROWUP = 2.0f;
	protected float CD_FIRE= 2.0f;
	protected bool hasMovableProjectile = true;
	protected bool grownup = false;
	protected bool planted = false;
	protected float timer_Growup;
	protected float timer_Fire;
	protected Vector3 currGridPosition;
	protected GameObject GlowGrid;
	protected bool onTheWayBackToDeck = false;
	protected  PVZSceneManager SceneManagerScript;
	protected float DEFAULT_APPEAR_DELAY = 0.0f;
	protected float timer_Appear;

	private Vector3 LOC_DECK;
	private Vector3 INIT_SCALE = new Vector3 (0.5f, 0.5f, 1.0f);
	private float speed_GoingToward = 10.0f;
	private bool onTheWayToPlant = false;
	private Vector3 LOC_PLANT;
	private bool onmouse = false;


	// Use this for initialization
	protected virtual void Start () {
		SceneManagerScript = Camera.main.GetComponent<PVZSceneManager>();
		GlowGrid = GameObject.Find ("GlowGrid");
		timer_Growup = CD_GROWUP;
		timer_Fire = CD_FIRE;
		transform.localScale = INIT_SCALE;
		LOC_DECK = transform.position;
		GetComponentInChildren<TextMesh> ().text = sunComsumption.ToString ();
		timer_Appear = DEFAULT_APPEAR_DELAY;
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update ();

		/*Health*/
		if (Health <= 0)
			Die ();

		/*Timer for growing up*/
		if (!grownup){
			timer_Growup -= Time.deltaTime;
			transform.localScale = INIT_SCALE + (CD_GROWUP-timer_Growup)/CD_GROWUP*(Vector3.one-INIT_SCALE);
			if (timer_Growup <= 0) {
				transform.localScale = Vector3.one;
				grownup = true;
				timer_Growup = CD_GROWUP;
			}
		}

		/*On the way back to deck*/
		if (onTheWayBackToDeck) {
			transform.position = Vector3.MoveTowards (transform.position, LOC_DECK, speed_GoingToward * Time.deltaTime);
			if (transform.position == LOC_DECK)
				onTheWayBackToDeck = false;
		}

		/*On The WayTo Plant*/
		if (onTheWayToPlant) {
			transform.position = Vector3.MoveTowards (transform.position, LOC_PLANT, speed_GoingToward * Time.deltaTime);
			if (transform.position == LOC_DECK)
				onTheWayToPlant = false;
		}

		/*Timer for firing*/
		if (planted && hasMovableProjectile) {
			timer_Fire -= Time.deltaTime;
			if (timer_Fire <= 0) {
				//Debug.Log(JobInFront());
				if (JobInFront ()) {
					Fire ();
					timer_Fire = CD_FIRE;
				}
			}
		}
	}

	public bool isPlanted(){
		return planted;
	}

	void OnMouseDown(){
		if (SceneManagerScript.GetSunCount () - sunComsumption >= 0
			&& !planted && grownup && Time.timeScale == 1)
			onmouse = true;
	}

	virtual protected void OnMouseDrag(){
		if (!onmouse)
			return;
		if (SceneManagerScript.GetSunCount() - sunComsumption >= 0) {
			if (!planted && grownup && Time.timeScale == 1){
				Vector2 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector2 leftdownCorner = new Vector2(LAWN_CENTER_X-LAWNBLOCK_WIDTH*(NUM_COLS/2.0f), LAWN_CENTER_Y-LAWNBLOCK_HEIGHT*(NUM_ROWS/2.0f));
				int rowi = (int)((cursorPosition.x-leftdownCorner.x)/LAWNBLOCK_WIDTH)-(int)(NUM_COLS/2);
				int coli = (int)((cursorPosition.y-leftdownCorner.y)/LAWNBLOCK_HEIGHT)-(int)(NUM_ROWS/2);

				transform.position = cursorPosition;
				currGridPosition = new Vector3(LAWN_CENTER_X+rowi*LAWNBLOCK_WIDTH,
				                                     LAWN_CENTER_Y+coli*LAWNBLOCK_HEIGHT, 
				                                     0.0f);
				if (OnTheLawn(cursorPosition))
					MoveGlowGrid(1,currGridPosition);
				else
					MoveGlowGrid(0,currGridPosition);

				GetComponentInChildren<TextMesh> ().text = "";
			}
		}
	}

	virtual protected void OnMouseUp(){
		if (!onmouse)
			return;

		//Shovel
		if (SceneManagerScript.OnShovel) {
			if (planted) {
				SceneManagerScript.ToggleShovelMode (false);
				Destroy (gameObject);
				return;
			}
		}
			
		//Plant
		MoveGlowGrid(0, GlowGrid.transform.position);
		if (SceneManagerScript.GetSunCount () - sunComsumption >= 0) {
			Vector2 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (grownup && !planted && OnTheLawn (GlowGrid) && Time.timeScale == 1 && OnTheLawn(cursorPosition)
			    && GlowGrid.GetComponent<PVZGlowGrid>().ApplicableGrid()) {
				//transform.position = currGridPosition;
				if (Effect!=null)
					Instantiate (Effect, currGridPosition, Quaternion.identity);
				onTheWayToPlant = true;
				planted = true;
				SceneManagerScript.AddNumGeneratedPlants(1);
				GenerateNewSmallTurret ();
				SceneManagerScript.SetSunCount (SceneManagerScript.GetSunCount () - sunComsumption);
				GameObject.Find ("Text_Sun").GetComponent<Text> ().text = SceneManagerScript.GetSunCount ().ToString ();
				LOC_PLANT = currGridPosition;
				GetComponent<Animator>().enabled = true;
			} else {
				onTheWayBackToDeck = true;
				onmouse = false;
				if (!planted)
					GetComponentInChildren<TextMesh> ().text = sunComsumption.ToString ();
			}
		}
		GlowGrid.GetComponent<PVZGlowGrid> ().SetApplicableGrid (true);
	}

	public void SellBack(){
		/*onTheWayBackToDeck = true;
		onmouse = false;
		GetComponentInChildren<TextMesh> ().text = sunComsumption.ToString ();

*/
		SceneManagerScript.SetSunCount (SceneManagerScript.GetSunCount () + sunComsumption);
		GameObject.Find ("Text_Sun").GetComponent<Text> ().text = SceneManagerScript.GetSunCount ().ToString ();
		Destroy (gameObject);
	}

	void GenerateNewSmallTurret(){
		GameObject newGO = (GameObject)Instantiate (gameObject, LOC_DECK, Quaternion.identity);
		newGO.name = name;
		newGO.GetComponent<PVZTurret> ().CD_GROWUP = CD_GROWUP;
		newGO.GetComponent<PVZTurret> ().grownup = false;
		newGO.GetComponent<PVZTurret> ().planted = false;
	}

	protected void Fire(){
		if (Projectile != null) {
			Vector3 projectilePosition = new Vector3 (transform.position.x + CANNON_LENGTH, 
		                                         transform.position.y + CANNON_HEIGHT, 
		                                         transform.position.z - 1.0f);
			Instantiate (Projectile, projectilePosition, Projectile.transform.rotation); 
		}
	}

	public void HurtBy(float damage){
		Health -= damage;
	}

	protected void MoveGlowGrid(float visiability, Vector3 loc){
		Color ggColor = GlowGrid.GetComponent<SpriteRenderer> ().color;
		GlowGrid.GetComponent<SpriteRenderer>().color = new Color(ggColor.r,ggColor.g,ggColor.b,visiability);
		GlowGrid.transform.position = loc;
	}
}