using UnityEngine;
using System.Collections;

public class PVZTurret_Change : PVZTurret {

	private bool onCollisioWithJob = false;

	override protected void OnMouseDrag(){
		if (SceneManagerScript.GetSunCount() - sunComsumption >= 0) {
			if (!planted && grownup && Time.timeScale == 1){
				Vector2 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				transform.position = cursorPosition;
				currGridPosition = cursorPosition;
				if (OnTheLawn(cursorPosition))
					MoveGlowGrid(1,currGridPosition);
				else
					MoveGlowGrid(0,currGridPosition);
				
				GetComponentInChildren<TextMesh> ().text = "";
			}
		}
	}

	override protected void OnMouseUp(){		
		MoveGlowGrid(0, GlowGrid.transform.position);
		if (onCollisioWithJob) {
			base.OnMouseUp();
		}
		else {
			onTheWayBackToDeck = true;
			GetComponentInChildren<TextMesh> ().text = sunComsumption.ToString ();
			
		}
	}
	
	void OnTriggerStay2D(Collider2D other){
		if (other.GetComponent<PVZJob> ())
			onCollisioWithJob = true;
	}
	
	void OnTriggerExit2D(Collider2D other){
		if (other.GetComponent<PVZJob> ())
			onCollisioWithJob = false;
	}

}
