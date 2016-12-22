using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportModeBehaviour{

	private GameObject localMainCamera;
	private GameObject teleportMarkerPrefab;
	private GameObject marker;

	private Button validateTeleportButton;

	private bool targetingGround;
	private float maxRange = 10f;

	public TeleportModeBehaviour( GameObject teleportMarkerPrefab, Button validateTeleportButton){
		this.teleportMarkerPrefab = teleportMarkerPrefab;
		this.validateTeleportButton = validateTeleportButton;
	}

	public void setPlayerMainCamera(GameObject mainCamera){
		this.localMainCamera = mainCamera;
	}

	public void teleportingFrame(){
		if (localMainCamera == null) {
			return;
		}

		RaycastHit hit;
		Ray ray = new Ray (localMainCamera.transform.position, localMainCamera.transform.forward);

		// we want to intersect only the ground layer (the 10th layer)
		int layerMask = 1 << 10; // layerMask = 0000001000000000 in base 2

		// if the main camera if oriented in front of something
		if (Physics.Raycast (ray, out hit, maxRange, layerMask)) {
			if (!targetingGround) {
				targetingGround = true;
				marker.SetActive (true);
				validateTeleportButton.interactable = true;
			}

			marker.transform.position = new Vector3 (hit.point.x, 0, hit.point.z);

		} else {
			if (targetingGround) {
				targetingGround = false;
				marker.SetActive (false);
				validateTeleportButton.interactable = false;
			}
		}
	}

	public void instanciateMarker(){
		marker = Object.Instantiate (teleportMarkerPrefab);

		targetingGround = false;
		validateTeleportButton.interactable = false;
		marker.SetActive (false);
	}

	public void destroyMarker(){
		Object.Destroy (marker);
		marker = null;
	}

	public void teleportPlayer(){
		if (marker != null) {
			Vector3 markerPos = marker.transform.position;


			localMainCamera.transform.position = new Vector3 (markerPos.x, localMainCamera.transform.position.y, markerPos.z);
		}
	}
}
