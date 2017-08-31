using UnityEngine;
using System.Collections;

public class PVZButton_Shovel : PVZButton {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartShovelMode(){//Called in Button_Shovel Click Event
		PVZSceneManager sceneManager = FindObjectOfType<PVZSceneManager>();
		if (!sceneManager.OnShovel)
			sceneManager.ToggleShovelMode(true);
		else
			sceneManager.ToggleShovelMode(false);
		
	}
}
