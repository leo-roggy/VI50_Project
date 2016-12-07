using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour {

	public GameObject mainCamera;

	public Button validatePlacementButton;

	public Button removeObjectButton;

	public Material highlightedMaterial;

	private ObjectModifyer objectModifyer;

	enum UserMode {normal, placingObject, inMenu}; 
	private UserMode userMode = UserMode.normal;

	void Start(){
		objectModifyer = new ObjectModifyer (mainCamera, validatePlacementButton, highlightedMaterial, removeObjectButton);
	}

	void FixedUpdate(){
		Debug.Log (userMode);
		switch (userMode) {
		case UserMode.normal:
			objectModifyer.HighlightTargetedObject ();
			break;
		case UserMode.placingObject:
			objectModifyer.PlacingObjectFrame ();
			break;
		case UserMode.inMenu:
			// DO NOTHING
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
		this.objectModifyer.setSelectedModel (model);
	}

	public void placeObject(){
		userMode = UserMode.placingObject;

		objectModifyer.instanciateGhost ();
	}

	public void validatePlacementObject(){
		userMode = UserMode.normal;

		objectModifyer.instanciatePrefab ();
		objectModifyer.destroyGhost ();

	}

	public void cancelPlacingObject(){
		userMode = UserMode.inMenu;

		objectModifyer.destroyGhost ();
	}

	public void removeTargetedObject(){
		objectModifyer.removeTargetedObject ();
	}

}
