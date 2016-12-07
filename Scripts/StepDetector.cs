using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StepDetector : MonoBehaviour {


	private Text t;

	private float firstFlag = 0f;
	private float secondFlag = 0f;

	private float counter = 0f;
	private int nbStep = 0;

	// Use this for initialization
	void Start () {
		t = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {


		float magnitude = Input.acceleration.magnitude;


		if (counter < 0f) { // reset
			counter = 0f;
			firstFlag = 0f;
			secondFlag = 0f;
		}

		if (firstFlag == 0f) {
			if (magnitude > 1.2f) { // detect first important magnitude
				firstFlag = magnitude;
				counter = 0.5f; // start counter
			}
		} else if(counter >= 0f) {// first flag != 0
			if (magnitude > firstFlag) { // if magnitude is more important than the previous frame
				firstFlag = magnitude;
				counter = 0.5f; // restart counter
			} else if (magnitude < 0.8f) {
				nbStep++;
				counter = 0f;
				firstFlag = 0f;
				secondFlag = 0f;
			} else{
				counter -= Time.deltaTime;
			}
		}

		string s = "nbStep : "+nbStep;
//		s += magnitude + "\n";
//		if (magnitude >= gPlus) {
//			s += "+";
//		} else if (magnitude <= gMinus) {
//			s += "-";
//		}

		t.text = s;

	}
}
