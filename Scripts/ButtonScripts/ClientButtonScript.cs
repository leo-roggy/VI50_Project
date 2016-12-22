using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Toggle))]
public class ClientButtonScript : MonoBehaviour {

	public NetworkManager networkManager;
	public Button cancelButton;

	private Toggle toggle;
	private InputField inputField;


	// Use this for initialization
	void Start () {
		toggle = GetComponent<Toggle> ();
		inputField = GetComponentInChildren<InputField> ();
	}

	void Update(){
		cancelButton.interactable = networkManager.IsClientConnected () || networkManager.isNetworkActive;


//		if (networkManager.isNetworkActive) {
//			cancelButton.interactable = true;
//		} else {
//			if (networkManager.client != null && networkManager.client.isConnected) {
//				cancelButton.interactable = true;
//			} else {
//				cancelButton.interactable = false;
//			}
//		}
//		cancelButton.interactable =  || networkManager.isNetworkActive;

	}

	public void clientButtonClicked(){
		if(toggle.isOn){
			if (networkManager.isNetworkActive) {
				networkManager.StopHost ();
			}
			if (!networkManager.IsClientConnected ()) {
				networkManager.StartClient ();
			}
		}
	}

	public void ipAddressValueChanged(){
		string ip = inputField.text;
		if (ip != "") {
			toggle.interactable = true;
		} else {
			toggle.interactable = false;
		}
	}
	public void ipAddressEndEdit(){
		string ip = inputField.text;
		if (ip != "") {
			networkManager.networkAddress = ip;
		}
	}
}
