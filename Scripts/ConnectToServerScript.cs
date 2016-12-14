using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Toggle))]
public class ConnectToServerScript : MonoBehaviour {

	public NetworkManagerBehaviour networkManagerBehaviour;

	private Toggle toggle;
	private InputField inputField;

	// Use this for initialization
	void Start () {
		toggle = GetComponent<Toggle> ();
		inputField = GetComponentInChildren<InputField> ();
	}

	public void clientButtonClicked(){
		if(toggle.isOn){
			networkManagerBehaviour.connectAsClient();
		}
	}

	public void ipAddressUpdated(){
		string ip = inputField.text;
		if (ip != "") {
			toggle.interactable = true;
			networkManagerBehaviour.setDistantServerIP (inputField.text);
		} else {
			toggle.interactable = false;
		}
	}
}
