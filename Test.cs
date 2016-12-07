using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Test : MonoBehaviour {

	private Text t;

	// Use this for initialization
	void Start () {
		t = GetComponent<Text> ();
		Input.location.Start();
	}
	
	// Update is called once per frame
	void Update () {
		string s = "";


		s += "Gyro attitude : " + Input.gyro.attitude.eulerAngles;
		s += "\nGyro gravity : " + Input.gyro.gravity;
		s += "\nGyro user acc : " + Input.gyro.userAcceleration;

		s += "\nAcceleration : " + Input.acceleration;
//		s += "\n\nacc events count : " + Input.accelerationEventCount;
//		foreach (AccelerationEvent ae in Input.accelerationEvents) {
//			s += "\nevent : " + ae.acceleration;
//		}

		s += "\n\nMagnetic heading : " + Input.compass.magneticHeading;

		if (Input.location.status == LocationServiceStatus.Running) {
			s+="\n\nlocation longitude : "+Input.location.lastData.longitude;
			s+="\nlocation latitude : "+Input.location.lastData.latitude;
			s+="\nlocation altitude : "+Input.location.lastData.altitude;
			s+="\nlocation accuracy : "+Input.location.lastData.horizontalAccuracy;
		}

		t.text = s;
	}
}
