using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PVZJob : PVZMonoBehaviour {
	public bool allowSpeedResume = true;
	public GameObject PARTICLE_DEATH;
	public Sprite[] SpriteLib;
	public GameObject PanelInfo;

	private GameObject newPanelInfo;

	protected float health = 1.0f;
	protected float attack = 0.25f;
	protected float DEFAULT_SPEED = 0.5f;
	
	private float speed;
	private float timer_ResumeSpeed;
	private const float RESUME_SPEED_INTERVAL = 1.5f;

	//Job Info
	private int ID;
	private string Title;
	private string Category = "Engineering";
	private string[] Characteristics;
	/* Requirement index
	 * 0 Task Flexibility
	 * 1 Female Friendly
	 * 2 Midwest	
	 * 3 Family Friendly	
	 * 4 Verbal Skill
	 * 5 Quantitative Skill	
	 * 6 Reasoning Skill	
	 * 7 Salary
	 */
	private float[] CharacteristicsValues;


	// Use this for initialization
	void Start () {
		speed = DEFAULT_SPEED;
		//ID = UnityEngine.Random.Range(0, NUM_JOB);
		//Set (ID);
	}
	
	// Update is called once per frame
	override protected void Update () {
		if (FindObjectOfType<PVZSceneManager> ().levelCompleted)
			return;
		
		base.Update ();

		/*Health*/
		if (health <= 0.5) {
			PlayDangerEffect ();
			if (health<=0){
				PlayDeathEffect();
				Destroy(gameObject);
			}
		}

		/*Moving*/
		transform.Translate(Vector2.left * speed * Time.deltaTime);

		/*Friction*/
		if (Time.timeScale == 1) {
			float vY = gameObject.GetComponent<Rigidbody2D> ().velocity.y;
			float fricY = (vY < 0)?FRICTION:(-FRICTION);
			gameObject.GetComponent<Rigidbody2D> ().AddForce(new Vector2 (0.0f, fricY));
		}

		/*Speed Resume*/
		if (allowSpeedResume && speed <= 0) {
			timer_ResumeSpeed-=Time.deltaTime;
			if (timer_ResumeSpeed<=0){
				speed = DEFAULT_SPEED;
				timer_ResumeSpeed = RESUME_SPEED_INTERVAL;
			}
		}
	}

	void AssignSprite(){
		foreach (Sprite sprite in SpriteLib) {
			if (sprite.name.Contains(Category)){
				GetComponent<SpriteRenderer>().sprite = sprite;
				return;
			}
		} 
		GetComponent<SpriteRenderer>().sprite = SpriteLib[0];

	}

	void PlayDangerEffect(){
	}
	
	void PlayDeathEffect(){
		Instantiate(PARTICLE_DEATH, transform.position, Quaternion.identity);
	}

	public float GetDefaultSpeed(){
		return DEFAULT_SPEED;
	}

	/*public void InverseDefaultSpeed(){
		DEFAULT_SPEED = -DEFAULT_SPEED;
	}*/

	public float getSpeed(){  
		return speed;
	}
	
	public void setSpeed(float s){
		speed = s;
	}

	public float getHealth(){  
		return health;
	}
	
	public void setHealth(float h){
		health = h;
	}

	public void Set(int id){
		ID = id;
		Characteristics = new string[]{
			"Task Flexibility: ", 
			"Female Friendly: ", 
			"Midwest: ", 
			"Family Friendly: ", 
			"Verbal Skill: ", 
			"Quantitative Skill: ", 
			"Reasoning Skill: ", 
			"Salary: "};
		CharacteristicsValues = new float[Characteristics.Length];
		PVZDataStorage ds_pvz = FindObjectOfType<TPYGDataStorageRef>().ds_pvz;
		Title = ds_pvz.getJobData ("c", id, 0);
		Category = ds_pvz.getJobData ("g", id, 0);
		for (int i = 0; i<NUM_CHARACTERISTICS; i++) {
			CharacteristicsValues[i] = Convert.ToSingle(ds_pvz.GetComponent<PVZDataStorage> ().getJobData ("c", id, i + 1));
			//Characteristics [i] += String.Format ("{0:P0}", CharacteristicsValues[i]);
		}
		AssignSprite ();

	}
	
	public int GetID(){
		return ID;
	}
	
	public string GetTitle(){
		return Title;
	}
	
	public string GetCategory(){
		return Category;
	}
	
	public string[] GetCharacteristics(){
		return Characteristics;
	}

	void OnTriggerStay2D(Collider2D other) {

		if (other.gameObject.GetComponent<PVZClient> ())
			Destroy (gameObject);
		if (other.gameObject.GetComponent<PVZTurret> ()) {
			if (other.gameObject.GetComponent<PVZTurret> ().isPlanted ()){
				speed = 0.0f;
				PVZTurret tScript = other.gameObject.GetComponent<PVZTurret> ();
				tScript.HurtBy(attack * Time.deltaTime);
			}
		}
	}

	void OnMouseDown(){
		//int i = UnityEngine.Random.Range (0, Characteristics.Length);
		//UpdateTextMeshInfo (i);
		//UpdateInfoPanel (i);

		Time.timeScale = 0;
		InstantiatePanelInfo ();
	}
	void OnMouseUp(){
		TextMesh[] tms = GetComponentsInChildren<TextMesh>();
		foreach (TextMesh tm in tms) 
			tm.text = "";
		
		Time.timeScale = 1;
		Destroy (newPanelInfo);
		if (FindObjectOfType<PVZSceneManager_Tutorial2>()) {
			FindObjectOfType<PVZSceneManager_Tutorial2> ().ReadJob ();
		}
	}

	void InstantiatePanelInfo(){
		newPanelInfo = InstantiateUnderParent (PanelInfo, GameObject.Find ("Panel_Main"), Vector3.zero);
		Text[] texts = newPanelInfo.GetComponentsInChildren<Text> ();
		texts [0].text = Title;
		texts [1].text = "";
		//foreach (string characteristic in Characteristics) 
			//texts[1].text += characteristic + "\n";

		GameObject PVZAttributeBar = newPanelInfo.transform.FindChild ("PVZAttributeBar").gameObject;
		float heightPVZAttributeBar = PVZAttributeBar.GetComponent<RectTransform> ().rect.height;
		for (int i = 0; i<Characteristics.Length; i++) {
			if (i==0){
				PVZAttributeBar.GetComponent<Text>().text = Characteristics[i];
				PVZAttributeBar.GetComponentInChildren<Slider>().value = CharacteristicsValues[i];
				continue;
			}
			GameObject newPVZAttributeBar = InstantiateUnderParent(PVZAttributeBar, newPanelInfo, 
			                                                       PVZAttributeBar.transform.localPosition - new Vector3(0, 1.4f*(float)i*heightPVZAttributeBar,0));
			newPVZAttributeBar.GetComponent<Text>().text = Characteristics[i];
			newPVZAttributeBar.GetComponentInChildren<Slider>().value = CharacteristicsValues[i];
		}
	}

	void UpdateTextMeshInfo(int numCharacteristics){
		TextMesh[] tms = GetComponentsInChildren<TextMesh>();
		tms [0].text = Title;
		if (numCharacteristics>=0)
			tms [1].text = Characteristics [numCharacteristics];
	}

	void UpdateInfoPanel(int numCharacteristics){
		GameObject.Find ("Text_Talks").GetComponent<Text> ().text = Title + "\n" + Characteristics [numCharacteristics];
		GameObject.Find ("Image_Talks").GetComponent<Image> ().sprite = GetComponent<SpriteRenderer>().sprite;
	}

}
