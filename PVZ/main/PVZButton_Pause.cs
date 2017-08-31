using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PVZButton_Pause : PVZButton {
	public GameObject Panel_PauseMenu;

	public void PauseGame(){//Called in Pause Button
		GameObject pausemenu = Instantiate (Panel_PauseMenu);
		pausemenu.transform.SetParent (GameObject.Find ("Canvas").transform);
		pausemenu.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		pausemenu.transform.position = new Vector3 (0.0f, 0.0f, 0.0f);

		Time.timeScale = 0;
		Camera.main.GetComponent<AudioSource> ().Pause ();

		GetComponent<Button> ().interactable = false;
	}

}
