using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

// this script allow to place new object in the scene. It also allow to move and rotate existing objects.
public class PlacementModeBehaviour : EditingObjectBehaviour {

	private GameObject[] instanciablePrefabs;

	private Button validatePlacementButton;

	private GameObject selectedModel;
	private int selectedId;



	public PlacementModeBehaviour(Button validatePlacementButton){
		this.validatePlacementButton = validatePlacementButton;
	}



	public void PlacingObjectFrame(){

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
			ghost.transform.position = new Vector3 (hit.point.x, hit.point.y + selectedModel.transform.position.y + epsilon, hit.point.z);
			// the ghost is always oriented in front of the camera
			ghost.transform.rotation = selectedModel.transform.rotation;
			ghost.transform.RotateAround (ghost.transform.position, Vector3.up, localMainCamera.transform.rotation.eulerAngles.y);

			if (ghost.GetComponent<CollidingUpdater> ().isColliding ()) {
				ghostMaterial.color = ghostCollisionColor;
				validatePlacementButton.interactable = false;
			} else {
				ghostMaterial.color = ghostColor;
				validatePlacementButton.interactable = true;
			}

		} else {
			if (targetingSomething) {
				targetingSomething = false;
				ghost.SetActive (false);
				validatePlacementButton.interactable = false;
			}
		}
	}

	public void instanciateGhost(){
		base.instantiateGhost (selectedModel);

		targetingSomething = false;
		ghost.SetActive (false);
	}

	public void instanciatePrefab(){
		Debug.Log("PlacementModeBehaviour : instanciatePrefab");
		localMainCamera.GetComponent<LocalPlayerScript> ().CmdInstaciateOnServer(this.selectedId, ghost.transform.position, ghost.transform.rotation);
	}

	public void setSelectedModel(int id){
		this.selectedId = id;

		this.selectedModel = instanciablePrefabs[id];
	}

	public void setInstanciablePrefabs(GameObject[] instanciablePrefabs){
		this.instanciablePrefabs = instanciablePrefabs;
	}

}
