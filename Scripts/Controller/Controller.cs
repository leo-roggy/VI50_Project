using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Controller : NetworkBehaviour {

	// Buttons
	public Button removeObjectButton;
	public Button moveObjectButton;
	public Button rotateObjectButton;
	public Button validatePlacementButton;
	public Button validateMovingButton;
	public Button validateRotationButton;
	public Button validateTeleportButton;

	public GameObject silhouettePrefab;

	public GameObject teleportMarkerPrefab;


	private GameObject playerMainCamera;


	enum UserMode {normal, placingObject, movingObject, rotatingObject, teleporting, inMenu}; 
	private UserMode userMode = UserMode.normal;

	private NormalModeBehaviour normalModeBehaviour;
	private PlacementModeBehaviour placementModeBehaviour;
	private MovingModeBehabiour movingModeBehaviour;
	private RotationModeBehaviour rotationModeBehaviour;
	private TeleportModeBehaviour teleportModeBehaviour;


	void Start(){
		playerMainCamera = null;
		
		placementModeBehaviour = new PlacementModeBehaviour (validatePlacementButton);
		normalModeBehaviour = new NormalModeBehaviour (silhouettePrefab, removeObjectButton, moveObjectButton, rotateObjectButton);
		movingModeBehaviour = new MovingModeBehabiour (validateMovingButton);
		rotationModeBehaviour = new RotationModeBehaviour (validateRotationButton);
		teleportModeBehaviour = new TeleportModeBehaviour (teleportMarkerPrefab, validateTeleportButton);
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
//			Debug.LogWarning ("Local camera not found");
		} else {
//			Debug.Log ("Local camera found");
			this.playerMainCamera = localCamera;
			normalModeBehaviour.setPlayerMainCamera (localCamera);
			placementModeBehaviour.setPlayerMainCamera (localCamera);
			movingModeBehaviour.setPlayerMainCamera (localCamera);
			rotationModeBehaviour.setPlayerMainCamera (localCamera);
			teleportModeBehaviour.setPlayerMainCamera (localCamera);

			GameObject[] instanciablePrefabs = new GameObject[ClientScene.prefabs.Count];
			ClientScene.prefabs.Values.CopyTo (instanciablePrefabs, 0);
			placementModeBehaviour.setInstanciablePrefabs (instanciablePrefabs);
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
			movingModeBehaviour.MovingObjectFrame ();
			break;
		case UserMode.rotatingObject:
			rotationModeBehaviour.RotatingObjectFrame ();
			break;
		case UserMode.teleporting:
			teleportModeBehaviour.teleportingFrame ();
			break;

		case UserMode.inMenu:
			break;
		}

	}

	public void enterInMenu(){
		userMode = UserMode.inMenu;
		normalModeBehaviour.unhighlightTargetedObject ();
	}
	public void exitMenu(){
		userMode = UserMode.normal;
	}

	// ---- placement ----

	public void setSelectedModel(int id){
		this.placementModeBehaviour.setSelectedModel (id);
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


	// ----- removing object ----

	public void removeTargetedObject(){
		normalModeBehaviour.removeTargetedObject ();
	}

	// ---- moving objetc ----

	public void movingObjectMode(){
		GameObject editingObject = normalModeBehaviour.getLastTargetedObject ();

		userMode = UserMode.movingObject;
		normalModeBehaviour.unhighlightTargetedObject ();

		movingModeBehaviour.setEditingObject (editingObject);
		movingModeBehaviour.instantiateGhost ();
	}

	public void validateMove(){
		movingModeBehaviour.moveObjectToGhost ();
		movingModeBehaviour.destroyGhost ();

		userMode = UserMode.normal;
	}

	public void cancelMove(){
		movingModeBehaviour.destroyGhost ();

		userMode = UserMode.normal;
	}

	// ---- rotating objetc ----

	public void rotatingObjectMode(){
		GameObject editingObject = normalModeBehaviour.getLastTargetedObject ();

		userMode = UserMode.rotatingObject;
		normalModeBehaviour.unhighlightTargetedObject ();

		rotationModeBehaviour.setEditingObject (editingObject);
		rotationModeBehaviour.instantiateGhost ();
	}

	public void validateRotation(){
		rotationModeBehaviour.applyGhostRotationToObject ();
		rotationModeBehaviour.destroyGhost ();

		userMode = UserMode.normal;
	}

	public void cancelRotation(){
		rotationModeBehaviour.destroyGhost ();

		userMode = UserMode.normal;
	}

	// ---- teleport ----

	public void teleportMode(){
		userMode = UserMode.teleporting;
		normalModeBehaviour.unhighlightTargetedObject ();

		teleportModeBehaviour.instanciateMarker ();
	}
	public void validateTeleport(){
		userMode = UserMode.normal;
		teleportModeBehaviour.teleportPlayer ();
		teleportModeBehaviour.destroyMarker ();
	}
	public void cancelTeleport(){
		userMode = UserMode.normal;
		teleportModeBehaviour.destroyMarker ();
	}

}
