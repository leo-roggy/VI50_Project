using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent (typeof (NetworkIdentity))]
public class LocalPlayerScript : NetworkBehaviour {

	private GameObject[] instanciablePrefabs;

	void OnStartLocalPlayer(){

	}


	[Command]
	public void CmdInstaciateOnServer(int id, Vector3 position, Quaternion rotation){ // We can only pass primitive parameters to command functions
		Debug.Log("LocalPlayerScript, CmdInstaciateOnServer, ClientScene.prefabs.Count = "+ClientScene.prefabs.Count);
		this.instanciablePrefabs = new GameObject[ClientScene.prefabs.Count];
		ClientScene.prefabs.Values.CopyTo (this.instanciablePrefabs, 0);

		GameObject prefab = null;
		prefab = this.instanciablePrefabs [id];// this.instanciablePrefabs[0] is the mainCameraPrefab

		if (prefab == null) {
			Debug.LogError ("prefab null");
		}

		GameObject obj =(GameObject)Object.Instantiate (prefab, position, rotation);
		NetworkServer.Spawn (obj);
	}

	[Command]
	public void CmdDestroyOnServer(NetworkInstanceId objectId){
		GameObject obj = NetworkServer.FindLocalObject (objectId);

		if (obj == null) {
			Debug.LogError ("Object not found on server");
		} else {
			NetworkServer.Destroy (obj);
		}
	}
}
