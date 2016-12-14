using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(NetworkManager))]
public class NetworkManagerBehaviour : MonoBehaviour {

	public Controller controller;

	private NetworkManager networkManager;

//	private bool host; // the networkManager is host or client
	private string localIP;
//	private string distantServerIp;

	// Use this for initialization
	void Start () {
		networkManager = GetComponent<NetworkManager> ();
		localIP = Network.player.ipAddress;

		Debug.Log ("StartHost");
		networkManager.StartHost ();
	
	}

	public void connectAsClient(){
		networkManager.StopHost ();

		NetworkClient networkClient = networkManager.StartClient ();
	}

	public void startHosting(){
		networkManager.StartHost ();
	}

	public void setDistantServerIP(string ip){
		networkManager.networkAddress = ip;
	}
}
