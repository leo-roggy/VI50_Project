using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent (typeof(NetworkIdentity))]
[RequireComponent (typeof(Camera))]
public class DisableNonLocalCameraDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NetworkIdentity ni = GetComponent<NetworkIdentity> ();
		Camera cam = GetComponent<Camera> ();
		if (!ni.isLocalPlayer){
			cam.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
