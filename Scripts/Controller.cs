using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour {

	private GameObject playerMainCamera;

	public Button validatePlacementButton;

	public Button removeObjectButton;

	public GameObject silhouettePrefab;


	private PlacementModeBehaviour placementModeBehaviour;
	private NormalModeBehaviour normalModeBehaviour;

	enum UserMode {normal, placingObject, movingObject, rotatingObject, inMenu}; 
	private UserMode userMode = UserMode.normal;

	void Start(){
		playerMainCamera = null;
		
		placementModeBehaviour = new PlacementModeBehaviour (validatePlacementButton);
		normalModeBehaviour = new NormalModeBehaviour (silhouettePrefab, removeObjectButton);
	}

	private void researchPlayerMainCamera(){
		GameObject[] cameras = GameObject.FindGameObjectsWithTag ("PlayerCamera");

		GameObject localCamera = null;
		foreach (GameObject camera in cameras) {
			if(camera.GetComponent<NetworkIdentity>().isLocalPlayer){
				localCamera = camera;
			}
		}

		if (localCamera == null) {
			Debug.LogWarning ("Local camera not found");
//			Invoke ("updatePlayerMainCamera", 1);
		} else {
			Debug.Log ("Local camera found");
			this.playerMainCamera = localCamera;
			placementModeBehaviour.setPlayerMainCamera (localCamera);
			normalModeBehaviour.setPlayerMainCamera (localCamera);
		}
	}

	void FixedUpdate(){

		if (playerMainCamera == null) {
			researchPlayerMainCamera ();
			if (playerMainCamera == null) {
				return;
			}
		}

		switch (userMode) {
		case UserMode.normal:
			normalModeBehaviour.HighlightTargetedObjectFrame ();
			break;
		case UserMode.placingObject:
			placementModeBehaviour.PlacingObjectFrame ();
			break;
		case UserMode.movingObject:
			normalModeBehaviour.unhighlightTargetedObject ();

			break;
		case UserMode.rotatingObject:
			normalModeBehaviour.unhighlightTargetedObject ();

			break;
		case UserMode.inMenu:
			normalModeBehaviour.unhighlightTargetedObject ();
			break;
		}

	}

	public void enterInMenu(){
		userMode = UserMode.inMenu;
	}
	public void exitMenu(){
		userMode = UserMode.normal;
	}

	public void setSelectedModel(GameObject model){
		this.placementModeBehaviour.setSelectedModel (model);
	}

	public void placeObject(){
		userMode = UserMode.placingObject;

		placementModeBehaviour.instanciateGhost ();
	}

	public void validatePlacementObject(){
		userMode = UserMode.normal;

		placementModeBehaviour.instanciatePrefab ();
		placementModeBehaviour.destroyGhost ();

	}

	public void cancelPlacingObject(){
		userMode = UserMode.inMenu;

		placementModeBehaviour.destroyGhost ();
	}

	public void removeTargetedObject(){
		normalModeBehaviour.removeTargetedObject ();
	}

}
