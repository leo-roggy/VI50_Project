using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// this script allow to place new object in the scene. It also allow to move and rotate existing objects.
public class ObjectModifyer {



	private GameObject mainCamera;

	// ----- highlight targeted object -----



	private GameObject lastTargetedObject;
	private Material lastTargetedMaterial;
	private Material highlightedMaterial;

	private Button removeButton;


	// ----- placement -----

	private Button validatePlacementButton;

	private GameObject selectedModel;

	private Material ghostMaterial;
	private Color ghostColor = new Color (0f, 1f, 0f, 0.3f);
	private Color ghostCollisionColor = new Color (1f, 0f, 0f, 0.3f);
	private GameObject ghost;

	private bool targetingSomething;
	private float maxRange = 10f; 

	private float epsilon = 0.0001f;




	public ObjectModifyer(GameObject mainCamera, Button validatePlacementButton, Material highlightedMaterial, Button removeButton){
		this.mainCamera = mainCamera;
		this.validatePlacementButton = validatePlacementButton;
		this.highlightedMaterial = highlightedMaterial;
		this.removeButton = removeButton;
	}

	public void HighlightTargetedObject(){
		RaycastHit hit;
		Ray ray = new Ray (mainCamera.transform.position, mainCamera.transform.forward);

		// we want to intersect anything but the Ground layer (the 10th layer)
		int layerMask = 1 << 10; // layerMask = 0000001000000000 in base 2
		layerMask = ~layerMask; // layerMask = 1111110111111111 in base 2

		// if the main camera if oriented in front of something
		if (Physics.Raycast (ray, out hit, maxRange, layerMask)) {

			if (lastTargetedObject == null) {
				lastTargetedObject = hit.transform.gameObject;
				lastTargetedMaterial = lastTargetedObject.GetComponent<Renderer> ().material;
				lastTargetedObject.GetComponent<Renderer> ().material = highlightedMaterial;

				removeButton.interactable = true;
			}

		}else{
			if (lastTargetedObject != null) {
				lastTargetedObject.GetComponent<Renderer> ().material = lastTargetedMaterial;
				lastTargetedObject = null;

				removeButton.interactable = false;
			}
		}
	}

	public void removeTargetedObject(){
		if (lastTargetedObject != null) {
			Object.Destroy (lastTargetedObject);
			lastTargetedObject = null;
			removeButton.interactable = false;
		}
	}

	public void PlacingObjectFrame(){

		RaycastHit hit;
		Ray ray = new Ray (mainCamera.transform.position, mainCamera.transform.forward);

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
			ghost.transform.RotateAround (ghost.transform.position, Vector3.up, mainCamera.transform.rotation.eulerAngles.y);

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
		ghost = Object.Instantiate (selectedModel);

		ghostMaterial = ghost.GetComponent<Renderer> ().material;
		ghostMaterial.color = ghostColor;

		ghost.layer = 9;
		ghost.AddComponent<CollidingUpdater> ();

		targetingSomething = false;
		ghost.SetActive (false);
	}

	public void instanciatePrefab(){
		Object.Instantiate (selectedModel, ghost.transform.position, ghost.transform.rotation);
	}


	public void destroyGhost(){
		Object.Destroy (ghost);
		ghost = null;
	}

	public void setSelectedModel(GameObject model){
				this.selectedModel = model;
	}

}
