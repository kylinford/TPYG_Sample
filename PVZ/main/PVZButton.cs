using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PVZButton : TPYGButton {
	METADataStorage ds_meta;

	// Use this for initialization
	void Start () {
		ds_meta = FindObjectOfType<TPYGDataStorageRef> ().ds_meta;
	}

	public void LoadNextScene(string sceneName){
		if (sceneName == "") {
			FindObjectOfType<PVZClient>().LevelCompleted();
		}
		else {
			ds_meta.numPVZTurretUnlocked++;
			SceneManager.LoadScene (sceneName);
			Time.timeScale = 1;
		}
	}

	//Called in Resume Button
	public void ResumeGame(){
		Destroy(transform.parent.gameObject);
		Time.timeScale = 1;
		Camera.main.GetComponent<AudioSource> ().Play ();
		FindObjectOfType<PVZButton_Pause> ().GetComponent<Button> ().interactable = true;
	}
	//Called in Restart Button
	public void RestartGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1;
		FindObjectOfType<PVZButton_Pause> ().GetComponent<Button> ().interactable = true;

	}
	//Called in Exit Button
	public void ExitGame(){
		Application.Quit();
	}

	public override void LoadScene(string scenename){
		//Destroy Clients
		if (!SceneManager.GetActiveScene ().name.Contains ("PVZ")) {
			PVZClient[] pvzclients = FindObjectsOfType<PVZClient> ();
			foreach (PVZClient pvzclient in pvzclients) {
				Destroy (pvzclient.gameObject);
			}
		}
		//Load scene
		Time.timeScale = 1;
		SceneManager.LoadScene (scenename);
	}
}
