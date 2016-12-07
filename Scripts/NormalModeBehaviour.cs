using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NormalModeBehaviour {


	private GameObject mainCamera;

	private GameObject silhouettePrefab;

	private GameObject silhouetteObject;

	private GameObject lastTargetedObject;
//	private Material lastTargetedMaterial;
//	private Material silhouetteMaterial;

	private Button removeButton;

	private float maxRange = 10f; 

	public NormalModeBehaviour (GameObject mainCamera, GameObject silhouettePrefab, Button removeButton) {
		this.mainCamera = mainCamera;
		this.silhouettePrefab = silhouettePrefab;
		this.removeButton = removeButton;
	}

	public void HighlightTargetedObjectFrame(){
		RaycastHit hit;
		Ray ray = new Ray (mainCamera.transform.position, mainCamera.transform.forward);

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

//				silhouetteObject = new GameObject ();
				silhouetteObject.transform.position = lastTargetedObject.transform.position;
				silhouetteObject.transform.rotation = lastTargetedObject.transform.rotation;
				silhouetteObject.transform.localScale = lastTargetedObject.transform.localScale;
//				MeshFilter mf = silhouetteObject.AddComponent <MeshFilter>() as MeshFilter;
				silhouetteObject.GetComponent<MeshFilter> ().mesh = lastTargetedObject.GetComponent<MeshFilter> ().mesh;
//				MeshRenderer mr = silhouetteObject.AddComponent <MeshRenderer>() as MeshRenderer;
//				mr.material = silhouetteMaterial;



//				silhouetteObject.GetComponent<Renderer> ().material = silhouetteMaterial;

				removeButton.interactable = true;
			}

		} else {
			if (silhouetteObject != null) {
				unhighlightTargetedObject ();
			}
		}
	}

	public void unhighlightTargetedObject(){
		if (silhouetteObject != null) {
//			Debug.Log("destroy");
			Object.Destroy(silhouetteObject);

			lastTargetedObject = null;
			silhouetteObject = null;

			removeButton.interactable = false;
		}
	}

	public void removeTargetedObject(){
		if (lastTargetedObject != null) {
			Object.Destroy (lastTargetedObject);
			lastTargetedObject = null;
			removeButton.interactable = false;
		}
	}
}
