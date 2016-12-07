using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour {

	public GameObject mainCamera;

	public Button validatePlacementButton;

	public Button removeObjectButton;

	public GameObject silhouettePrefab;


	private PlacementModeBehaviour placementModeBehaviour;
	private NormalModeBehaviour normalModeBehaviour;

	enum UserMode {normal, placingObject, movingObject, rotatingObject, inMenu}; 
	private UserMode userMode = UserMode.normal;

	void Start(){
		placementModeBehaviour = new PlacementModeBehaviour (mainCamera, validatePlacementButton);
		normalModeBehaviour = new NormalModeBehaviour (mainCamera, silhouettePrefab, removeObjectButton);
	}

	void FixedUpdate(){
//		Debug.Log (userMode);
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
