using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class RotationModeBehaviour : EditingObjectBehaviour {

	private Button validateRotationButton;

	private GameObject editingObject;

	private float rotationSpeed = 10f;

	public RotationModeBehaviour(Button validateRotationButton){
		this.validateRotationButton = validateRotationButton;
	}


	public void RotatingObjectFrame(){

		if (localMainCamera == null) {
			return;
		}

		if (Input.GetMouseButton (0)) {
			ghost.transform.RotateAround (ghost.transform.position, Vector3.up, Input.GetAxis ("Mouse X") * rotationSpeed * -1f);
		}

		if (ghost.GetComponent<CollidingUpdater> ().isColliding ()) {
			ghostMaterial.color = ghostCollisionColor;
			validateRotationButton.interactable = false;
		} else {
			ghostMaterial.color = ghostColor;
			validateRotationButton.interactable = true;
		}
	}

	public void setEditingObject(GameObject obj){
		this.editingObject = obj;
		//the editing object mush be in the ghost layer to avoid collision whith the other ghost
		obj.layer = 9;
	}

	public void instantiateGhost(){
		base.instantiateGhost (this.editingObject);

		ghost.SetActive (true);
	}

	public void applyGhostRotationToObject(){
		this.editingObject.transform.rotation = ghost.transform.rotation;
	}	

	public void destroyGhost(){
		this.editingObject.layer = 0;
		this.editingObject = null;

		base.destroyGhost ();
	}

}
