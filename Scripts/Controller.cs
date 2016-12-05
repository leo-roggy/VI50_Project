using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour {

	public GameObject mainCamera;

	public Button validatePlacementButton;

	private ObjectModifyer objectModifyer;

	private bool placingObject = false;

	void Start(){
		objectModifyer = new ObjectModifyer (mainCamera, validatePlacementButton);
	}

	void FixedUpdate(){
		
		if (placingObject) {
			objectModifyer.PlacingObjectFrame ();
		} else {

		}
	}


	public void setSelectedModel(GameObject model){
		this.objectModifyer.setSelectedModel (model);
	}

	public void placeObject(){
		placingObject = true;
		objectModifyer.instanciateGhost ();
	}

	public void validatePlacementObject(){
		placingObject = false;
		objectModifyer.instanciatePrefab ();
		objectModifyer.destroyGhost ();

	}

	public void cancelPlacingObject(){
		placingObject = false;
		objectModifyer.destroyGhost ();
	}

}
