using UnityEngine;
using System.Collections;

public class CollidingUpdater : MonoBehaviour {

	private int nbCollindingObjects;

	void OnTriggerEnter(Collider other){
		nbCollindingObjects++;
	}
	void OnTriggerExit(Collider other){
		nbCollindingObjects--;
	}

	public bool isColliding(){
		return nbCollindingObjects>0;
	}
}
