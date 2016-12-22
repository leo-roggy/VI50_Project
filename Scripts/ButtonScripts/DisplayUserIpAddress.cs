using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

using System.Collections;

[RequireComponent (typeof (InputField))]
public class DisplayUserIpAddress : MonoBehaviour {

	void Start () {
		GetComponent<InputField> ().text = Network.player.ipAddress;
	}
}
