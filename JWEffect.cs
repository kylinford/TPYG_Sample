using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JWEffect : MonoBehaviour {
	public bool blink = false;
	public float speed_Blink = 1;
	public float minA = 0.5f;
	public float maxA = 1;

	public bool blinkGrey = false;
	public float speed_Grey = 1;
	public float timerGreyDown = 0.5f;
	public float timerGreyUp = 1;

	public float life = -1;
	public bool fadein = false;
	public float timer_fadein = 1.0f;
	public bool isFading = false;

	public bool TapToDestroy = false;
	public bool TransitionOn = false;
	public bool TransitionLoop = false;
	public bool onDestOffset = true;
	public Vector3 destinationOffset;
	public Vector3 destinationScale = Vector3.one;
	public Vector3 destinationEuler = Vector3.zero;
	public float defaultSpeed = 100.0f;
	public bool FlyinMidslowFlyout;
	public Vector3 flyInit;
	public Vector3 flyMid;
	public float speedFly_init = 700.0f;
	public float speedFly_mid = 1.0f;
				
	private Graphic[] graphics;
	private SpriteRenderer[] spriterenderers;
	private float[] destAplhasGaphics;
	private float[] destAplhasRenderers;
	public Vector3 initScale;
	public Vector3 initEuler;
	public Vector3 initPosition;
	private float currSpeed;

	public Vector3 InitPosition {get {return initPosition;}}

	private Vector3 destPosition;
	private float flyDistance;

	public Sprite ImageToSwap;

	// Use this for initialization
	void Start () {
		InitAttributes ();
		if (fadein) {
			isFading = true;
			StartCoroutine (DoFadeIn ());
		}
		initScale = transform.localScale;
		initEuler = transform.eulerAngles;
		initPosition = transform.localPosition;


		if (life >= 0) {
			StartCoroutine(Die(life));
		}

		if (FlyinMidslowFlyout) {
			destPosition = flyMid + (flyMid - flyInit);
			flyDistance = (destPosition - flyInit).magnitude;
			transform.localPosition = flyInit;
		}
		currSpeed = defaultSpeed;

		StartCoroutine (DoBlink ());
		StartCoroutine (DoBlink_Grey ());

	}
	
	// Update is called once per frame
	void Update () {
		if (TapToDestroy && !isFading) {
			if (Input.GetButtonDown("Fire1"))
				Destroy(gameObject);
		}

		if (FlyinMidslowFlyout) {
			if (transform.localPosition != destPosition) {
				currSpeed = speedFly_init + (1.0f - ((transform.localPosition - flyMid).magnitude / (flyDistance / 2.0f))) * (speedFly_mid - speedFly_init);
				//transform.localPosition = Vector3.MoveTowards(transform.localPosition, destPosition, Time.deltaTime*currSpeed);
			} else {
				Destroy (gameObject);
			}
		} else {
			currSpeed = defaultSpeed;
			destPosition = initPosition + destinationOffset;
		}

		if (onDestOffset) {
			if (transform.localPosition != destPosition) {
				//print (name);
				transform.localPosition = Vector3.MoveTowards (transform.localPosition, destPosition, Time.deltaTime * currSpeed);
				if (TransitionOn) {
					float progress = (transform.localPosition - initPosition).magnitude / (destPosition - initPosition).magnitude;
					transform.localScale = initScale + (destinationScale - initScale) * progress;
					transform.eulerAngles = initEuler + (destinationEuler - initEuler) * progress;
				}
				if (transform.localPosition == destPosition) {
					if (TransitionLoop) {
						transform.localPosition = initPosition;
						transform.eulerAngles = initEuler;
						transform.localScale = initScale;
					} else {
						initScale = transform.localScale;
						initEuler = transform.eulerAngles;
						initPosition = transform.localPosition;
						destinationOffset = Vector3.zero;
					}
				}
			} 
		}
	}

	void InitAttributes(){
		graphics = GetComponentsInChildren<Graphic> ();
		spriterenderers = GetComponentsInChildren<SpriteRenderer> ();
		destAplhasGaphics = new float[graphics.Length];
		destAplhasRenderers = new float[spriterenderers.Length];
		if (fadein) {
			for (int i=0; i<graphics.Length; i++) {
				destAplhasGaphics [i] = graphics [i].color.a;
				Color currColor = graphics [i].color;
				graphics [i].color = new Color (currColor.r, currColor.g, currColor.b, 0);
			}
			for (int i=0; i<spriterenderers.Length; i++) {
				destAplhasRenderers [i] = spriterenderers [i].color.a;
				Color currColor = spriterenderers [i].color;
				spriterenderers [i].color = new Color (currColor.r, currColor.g, currColor.b, 0);
			}
		}
	}

	IEnumerator Die(float waitTime){
		yield return new WaitForSeconds (waitTime);
		if (TapToDestroy == true)
			Destroy(gameObject);
	}

	float AddColorReturnMaxAlpha(Color offsetColor){
		float maxAlpha = 0;
		for (int i = 0; i < graphics.Length; i++) {
			graphics [i].color = graphics [i].color + offsetColor;
			if (graphics [i].color.a > maxAlpha)
				maxAlpha = graphics [i].color.a;
		}
		for (int i = 0; i < spriterenderers.Length; i++) {
			spriterenderers [i].color = spriterenderers [i].color + offsetColor;
			if (spriterenderers [i].color.a > maxAlpha)
				maxAlpha = graphics [i].color.a;
		}
		return maxAlpha;
	}

	IEnumerator DoBlink(){
		bool alphaUp = true;
		while (true) {
			yield return new WaitForSeconds(Time.deltaTime);
			if (blink){
				if (alphaUp){
					float currMaxAlpha = AddColorReturnMaxAlpha(new Color(0,0,0, speed_Blink*Time.deltaTime));
					if (currMaxAlpha >= maxA){
						yield return new WaitForSeconds(1);
						alphaUp = false;
					}
				}else{
					float currMaxAlpha = AddColorReturnMaxAlpha(new Color(0,0,0, -speed_Blink*Time.deltaTime));
					if (currMaxAlpha <= minA)
						alphaUp = true;
				}
			}
		}
	}

	IEnumerator DoBlink_Grey(){
		bool greyUp = true;
		float cumulativeGrey = 0;
		while (true) {
			yield return new WaitForSeconds(Time.deltaTime);
			if (blinkGrey){
				float offset = speed_Blink*Time.deltaTime;
				if (greyUp){
					AddColorReturnMaxAlpha(new Color(offset, offset, offset, 0));
					cumulativeGrey += offset;
					if (cumulativeGrey >= timerGreyUp){
						//yield return new WaitForSeconds(1);
						cumulativeGrey = 0;
						greyUp = false;
					}
				}else{
					AddColorReturnMaxAlpha(new Color(-offset, -offset, -offset, 0));
					cumulativeGrey += offset;
					if (cumulativeGrey >= timerGreyDown){
						//yield return new WaitForSeconds(0.5f);
						cumulativeGrey = 0;
						greyUp = true;
					}
				}
			}
		}
	}

	IEnumerator DoFadeIn(){
		int stepsN = (int)(timer_fadein / Time.deltaTime);
		bool finished = false;

		while (!finished) {
			for (int i = 0; i < graphics.Length; i++) {
				Color currColor = graphics [i].color;
				if (currColor.a < destAplhasGaphics[i]) 
					graphics [i].color = new Color 
						(currColor.r, currColor.g, currColor.b, currColor.a + (destAplhasGaphics[i]/(float)stepsN));
				else
					finished = true;
			}
			for (int i = 0; i < spriterenderers.Length; i++) {
				Color currColor = spriterenderers [i].color;
				if (currColor.a < destAplhasRenderers[i]) 
					spriterenderers [i].color = new Color 
						(currColor.r, currColor.g, currColor.b, currColor.a + (destAplhasRenderers[i]/(float)stepsN));
				else
					finished = true;
			}
			yield return new WaitForSeconds(Time.deltaTime);
		}
		isFading = false;
	}

	public void StartSwapImage(){
		StartCoroutine (SwapImage_SpriteRenderer ());
	}

	IEnumerator SwapImage_SpriteRenderer(){
		int stage = 0;
		Color colorInit;
		colorInit = spriterenderers [0].color;
		Sprite ImageInit;
		if (spriterenderers.Length > 0)
			ImageInit = spriterenderers [0].sprite;
		else
			ImageInit = graphics [0].GetComponent<Image>().sprite;

		while (stage != 2) {
			if (stage == 0) {//fadeout
				if (spriterenderers [0].color.a > 0) {
					spriterenderers [0].color = new Color (colorInit.r, colorInit.g, colorInit.b, spriterenderers [0].color.a - 2*Time.deltaTime);
					yield return new WaitForSeconds(Time.deltaTime);
				}else{
					stage = 1;
					spriterenderers [0].sprite = ImageToSwap;
				}
			} else if (stage == 1) {//fadein
				if (spriterenderers [0].color.a < 1) {
					spriterenderers [0].color = new Color (colorInit.r, colorInit.g, colorInit.b, spriterenderers [0].color.a + 2*Time.deltaTime);
					yield return new WaitForSeconds(Time.deltaTime);
				}else{
					stage = 2;
					ImageToSwap = ImageInit;
					ImageInit = spriterenderers [0].sprite;
				}
			}
		}
	}

}
