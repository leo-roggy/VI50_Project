using UnityEngine;
using System.Collections;

public abstract class EditingObjectBehaviour {

	protected GameObject localMainCamera;

	protected Material ghostMaterial;
	protected Color ghostColor = new Color (0f, 1f, 0f, 0.3f);
	protected Color ghostCollisionColor = new Color (1f, 0f, 0f, 0.3f);
	protected GameObject ghost;



	protected bool targetingSomething;
	protected float maxRange = 10f;

	protected float epsilon = 0.0001f;

	public void setPlayerMainCamera(GameObject mainCamera){
		this.localMainCamera = mainCamera;
	}

	protected void instantiateGhost(GameObject model){
		ghost = Object.Instantiate (model);

		ghost.AddComponent <CollidingUpdater>();

		ghostMaterial = ghost.GetComponent<Renderer> ().material;
		ghostMaterial.color = ghostColor;

		ghost.layer = 9;
	}

	public void destroyGhost(){
		Object.Destroy (ghost);
		ghost = null;
	}
}
