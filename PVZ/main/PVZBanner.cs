using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PVZBanner : MonoBehaviour {
	public float waitTime = 3;
	public float speed = 3;
	Vector3 positionLower;
	Vector3 positionHigher;

	// Use this for initialization
	void Start () {
		positionHigher = transform.position;
		positionLower = positionHigher + new Vector3 (0, -5, 0);

		transform.position = positionLower;
		StartCoroutine (WaitAndMoveToDest (waitTime, positionHigher));
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator WaitAndMoveToDest(float time, Vector3 positionDest){
		yield return new WaitForSeconds (time);
		StartCoroutine (MoveToDest (positionDest));
	}

	IEnumerator MoveToDest(Vector3 positionDest){
		bool arrived = false;
		while (!arrived) {
			yield return new WaitForSeconds (Time.deltaTime);
			if (transform.position != positionDest)
				transform.position = Vector3.MoveTowards (transform.position, positionDest, speed * Time.deltaTime);
			else
				arrived = true;
		}
	}

	public IEnumerator ShowWinningMessage(string message){
		GetComponentInChildren<Text> ().text = message;
		yield return MoveToDest (positionLower);

		FindObjectOfType<PVZSceneManager> ().levelCompleted = true;

	}
}
