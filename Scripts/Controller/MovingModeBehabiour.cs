using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class MovingModeBehabiour : EditingObjectBehaviour {

	private Button validateMovingButton;

	private GameObject editingObject;

	public MovingModeBehabiour(Button validateMovingButton){
		this.validateMovingButton = validateMovingButton;
	}


	public void MovingObjectFrame(){

		if (localMainCamera == null) {
			return;
		}

		RaycastHit hit;
		Ray ray = new Ray (localMainCamera.transform.position, localMainCamera.transform.forward);

		// we want to intersect anything but the gost layer (the 9th layer)
		int layerMask = 1 << 9; // layerMask = 0000000100000000 in base 2
		layerMask = ~layerMask; // layerMask = 1111111011111111 in base 2

		// if the main camera if oriented in front of something
		if (Physics.Raycast (ray, out hit, maxRange, layerMask)) {

			if (!targetingSomething) {
				targetingSomething = true;
				ghost.SetActive (true);
			}
			//TODO mettre (hit.point.x, hit.point.y, hit.point.z) à la place, le centre de tous les objets doit se trouver au sol
			ghost.transform.position = new Vector3 (hit.point.x, hit.point.y + editingObject.transform.position.y + epsilon, hit.point.z);

			if (ghost.GetComponent<CollidingUpdater> ().isColliding ()) {
				ghostMaterial.color = ghostCollisionColor;
				validateMovingButton.interactable = false;
			} else {
				ghostMaterial.color = ghostColor;
				validateMovingButton.interactable = true;
			}

		} else {
			if (targetingSomething) {
				targetingSomething = false;
				ghost.SetActive (false);
				validateMovingButton.interactable = false;
			}
		}
	}

	public void setEditingObject(GameObject obj){
		this.editingObject = obj;
		//the editing object mush be in the ghost layer to avoid collision whith the other ghost
		obj.layer = 9;

	}

	public void instantiateGhost(){
		base.instantiateGhost (this.editingObject);

		targetingSomething = false;
		ghost.SetActive (false);
	}

	public void moveObjectToGhost(){
		this.editingObject.transform.position = ghost.transform.position;
	}	

	public void destroyGhost(){
		this.editingObject.layer = 0;
		this.editingObject = null;

		base.destroyGhost ();
	}

}
