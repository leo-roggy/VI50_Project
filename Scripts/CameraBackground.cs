using UnityEngine;
using System.Collections;

public class CameraBackground : MonoBehaviour {

	void Start () {
		InitializeBackground ();
	}


	public void InitializeBackground(){

		// we get the GuiTexture component
		GUITexture gt = GetComponent<GUITexture> ();

		// we place the texture
		// we don't need to set x max and y max, the will be modified later
		gt.pixelInset = new Rect (Screen.width/2, Screen.height/2, 0, 0); 

		// the texture is liked to the first camera
		WebCamTexture wct = new WebCamTexture (WebCamTexture.devices[0].name, Screen.width, Screen.height, 30);
		// the texture will by updated each frame
		wct.Play ();
		gt.texture = wct;
	}
}
