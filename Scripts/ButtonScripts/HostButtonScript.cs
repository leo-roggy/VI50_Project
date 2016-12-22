using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Toggle))]
public class HostButtonScript : MonoBehaviour {

	public NetworkManager networkManager;

	private Toggle toggle;

	void Start () {
		toggle = GetComponent<Toggle> ();
	}

	public void hostButtonClicked(){
		if(toggle.isOn){
			if (networkManager.IsClientConnected ()) {
				networkManager.StopClient ();
			}

			if (!networkManager.isNetworkActive) {
				networkManager.StartHost ();
			}
		}
	}
}
