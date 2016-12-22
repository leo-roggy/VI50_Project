using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class NormalModeBehaviour {

	private GameObject localMainCamera;

	private GameObject silhouettePrefab;

	private GameObject silhouetteObject;

	private GameObject lastTargetedObject;

	private Button removeButton;
	private Button moveObjectButton;
	private Button rotateObjectButton;

	private float maxRange = 10f; 

	public NormalModeBehaviour (GameObject silhouettePrefab, Button removeButton, Button moveObjectButton, Button rotateObjectButton) {
		this.silhouettePrefab = silhouettePrefab;
		this.removeButton = removeButton;
		this.moveObjectButton = moveObjectButton;
		this.rotateObjectButton = rotateObjectButton;
	}

	public void setPlayerMainCamera(GameObject mainCamera){
		this.localMainCamera = mainCamera;
	}

	public void HighlightTargetedObjectFrame(){

		if (localMainCamera == null) {
			return;
		}

		RaycastHit hit;
		Ray ray = new Ray (localMainCamera.transform.position, localMainCamera.transform.forward);

		// we want to intersect anything but the Ground layer (the 10th layer)
		int layerMask = 1 << 10; // layerMask = 0000001000000000 in base 2
		layerMask = ~layerMask; // layerMask = 1111110111111111 in base 2

		// if the main camera if oriented in front of something
		if (Physics.Raycast (ray, out hit, maxRange, layerMask)) {
			if (lastTargetedObject != null && lastTargetedObject != hit.transform.gameObject) {
				unhighlightTargetedObject ();
			}

			if (lastTargetedObject == null) {
				lastTargetedObject = hit.transform.gameObject;

				silhouetteObject = Object.Instantiate (silhouettePrefab) as GameObject;

				silhouetteObject.transform.position = lastTargetedObject.transform.position;
				silhouetteObject.transform.rotation = lastTargetedObject.transform.rotation;
				silhouetteObject.transform.localScale = lastTargetedObject.transform.localScale;
				silhouetteObject.GetComponent<MeshFilter> ().mesh = lastTargetedObject.GetComponent<MeshFilter> ().mesh;


				removeButton.interactable = true;
				moveObjectButton.interactable = true;
				rotateObjectButton.interactable = true;
			}

		} else {
			if (silhouetteObject != null) {
				unhighlightTargetedObject ();
			}
		}
	}

	public void unhighlightTargetedObject(){
		if (silhouetteObject != null) {
			Object.Destroy(silhouetteObject);

			lastTargetedObject = null;
			silhouetteObject = null;

			removeButton.interactable = false;
			moveObjectButton.interactable = false;
			rotateObjectButton.interactable = false;
		}
	}

	public void removeTargetedObject(){
		if (lastTargetedObject != null) {
			Debug.Log ("NormalModeBehaviour, removeTargetedObject");

			Object.Destroy (lastTargetedObject);
			NetworkIdentity ni = lastTargetedObject.GetComponent<NetworkIdentity> ();

			localMainCamera.GetComponent<LocalPlayerScript> ().CmdDestroyOnServer (ni.netId);

			lastTargetedObject = null;

			removeButton.interactable = false;
			moveObjectButton.interactable = false;
			rotateObjectButton.interactable = false;
		}
	}

	public GameObject getLastTargetedObject(){
		return lastTargetedObject;
	}
		
}
