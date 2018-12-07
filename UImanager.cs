using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UImanager : MonoBehaviour {
	//Used to toggle body parts
	public GameObject ObjectToToggle;
	//used for body rotate
	public GameObject AutoRotateObject;
	//used for player movement
	public GameObject PlayerTransform;
	//Control toggle
	public GameObject Controls;
	//positions for moving camera
	public Transform[] CameraPositions;
	//How fast object rotates
	public float RotationsPerMinute = 0;
	//colors for UI
	public Color32 UIcolor;
	public Color32 PressedColor;
	public Color32 LookedColor;
	 
	//When UI clicked it checks objects active state and switches it, as well as its color.
	public void Click(){
		if (ObjectToToggle != null) {
			if (ObjectToToggle.active == true) {
				ObjectToToggle.gameObject.SetActive (false);
				GetComponent<Image> ().color = PressedColor;

				//Sets object to true
			} else {
				ObjectToToggle.gameObject.SetActive (true);
				GetComponent<Image> ().color = UIcolor;
			}
		}
	}
	//sets UI color when not being looked at
	public void OnCursorNull(){
		GetComponent<Image> ().material.color = UIcolor;
	}
	//sets UI color when being looked at
	public void OnCursorOver(){
		GetComponent<Image> ().material.color = LookedColor;
	}
	//sets rotation speed and starts rotation coroutine
	public void AutoRotateON(){
		if (AutoRotateObject != null) {
			RotationsPerMinute = 1;
			StartCoroutine ("RotateCoroutine");
			}
		}
	//turns off auto rotate by setting value to 0
	public void AutoRotateOFF(){
		RotationsPerMinute = 0;
		GetComponent<Image>().color = UIcolor;
	}

	//this method is called when rightrotate button is clicked.  rotates object right by setting rotation to negative value
	public void RotateRight(){
		GetComponent<Image>().color = PressedColor;
		RotationsPerMinute = -7f;
		StartCoroutine ("RotateCoroutine");
	}
	//same as rotate right but sets rotation value to positive value.
	public void RotateLeft(){
		GetComponent<Image>().color = PressedColor;
		RotationsPerMinute = 7f;
		StartCoroutine ("RotateCoroutine");
	}
	//Starts Y axis pan coroutine and gives it the low camera target
	public void PanDown(){
		GetComponent<Image>().color = PressedColor;
		if (PlayerTransform != null) {
			StartCoroutine ("PanUpDownCoroutine", CameraPositions[0]);
		}
	}
	//starts Y axis pan coroutine and gives it the high camera target
	public void PanUp(){
		GetComponent<Image>().color = PressedColor;
		if (PlayerTransform != null) {
			StartCoroutine ("PanUpDownCoroutine", CameraPositions[1]);
		}
	}
	//Starts X axis coroutine and gives far right target
	public void PanRight(){
		GetComponent<Image>().color = PressedColor;
		if (PlayerTransform != null) {
			StartCoroutine ("PanLeftRightCoroutine", CameraPositions[2]);
		}
	}
	//Starts X axis coroutine and gives far left target
	public void PanLeft(){
		GetComponent<Image>().color = PressedColor;
		if (PlayerTransform != null) {
			StartCoroutine ("PanLeftRightCoroutine", CameraPositions[3]);
		}
	}
	//Starts center coroutine and gives center target
	public void Center(){
		GetComponent<Image>().color = PressedColor;
		if (PlayerTransform != null) {
			StartCoroutine ("CenterCoroutine", CameraPositions[4]);
		}
	}
	//Starts Z axis pan coroutine and gives zoom in target
	public void ZoomIn(){
		GetComponent<Image>().color = PressedColor;
		if (PlayerTransform != null) {
			StartCoroutine ("ZoomCoroutine", CameraPositions[5]);
		}
	}
	//Starts Z axis pan coroutine and gives zoom out target
	public void ZoomOut(){
		GetComponent<Image>().color = PressedColor;
		if (PlayerTransform != null) {
			StartCoroutine ("ZoomCoroutine", CameraPositions[6]);
		}
	}
	//this method is used to toggle/hide the UI elements
	public void HideAll(){
		bool toggle = true;
		toggle = !toggle;
		if (Controls.activeSelf== true) {
			Controls.SetActive (false);
			GetComponent<Image> ().color = PressedColor;
		} else {
			Controls.SetActive (true);
			GetComponent<Image> ().color = UIcolor;
		}
	}
	//While RPM is not 0 rotate at new value until it is 0 then return
	IEnumerator RotateCoroutine (){
		while (RotationsPerMinute != 0) {
			GetComponent<Image>().color = PressedColor;
			AutoRotateObject.transform.Rotate (0, RotationsPerMinute * 0.2f, 0);
			yield return null;
			GetComponent<Image> ().color = UIcolor;
		}
	}
	//moves player to the center until centered fully
	IEnumerator CenterCoroutine (Transform CameraPositions){
		while (Vector3.Distance (PlayerTransform.transform.position, CameraPositions.position) > .1f) {
			PlayerTransform.transform.position = Vector3.MoveTowards (PlayerTransform.transform.position, CameraPositions.position, 1f * Time.deltaTime);
			yield return null;
		}
		GetComponent<Image>().color = UIcolor;
	}
	//moves player along the Y axis until target is reached
	IEnumerator PanUpDownCoroutine (Transform CameraPositions){
		while (PlayerTransform.transform.position.y != CameraPositions.transform.position.y) {
			PlayerTransform.transform.position = new Vector3 (PlayerTransform.transform.position.x, Mathf.MoveTowards (PlayerTransform.transform.position.y, CameraPositions.position.y, Time.deltaTime *1), PlayerTransform.transform.position.z);
			yield return null;
		}
		GetComponent<Image>().color = UIcolor;
	}
	//Moves player along the X axis until target is reached
	IEnumerator PanLeftRightCoroutine (Transform CameraPositions){
		while (PlayerTransform.transform.position.x != CameraPositions.transform.position.x) {
			PlayerTransform.transform.position = new Vector3 (Mathf.MoveTowards (PlayerTransform.transform.position.x, CameraPositions.position.x, Time.deltaTime *1), PlayerTransform.transform.position.y, PlayerTransform.transform.position.z);
			yield return null;
		}
		GetComponent<Image>().color = UIcolor;
	}
	//Moves player along the Z axis until target is reached
	IEnumerator ZoomCoroutine (Transform CameraPositions){
		while (PlayerTransform.transform.position.z != CameraPositions.transform.position.z) {
			PlayerTransform.transform.position = new Vector3 (PlayerTransform.transform.position.x, PlayerTransform.transform.position.y,Mathf.MoveTowards (PlayerTransform.transform.position.z, CameraPositions.position.z, Time.deltaTime *1));
			yield return null;
		}
		GetComponent<Image>().color = UIcolor;
	}
	//called when user lets go of click or touch
	public void UpClick(){
		//stops all coroutines because button is not being held down
		if (PlayerTransform != null) {
			GetComponent<Image>().color = UIcolor;
			StopAllCoroutines ();
		}
		// sets rotations to 0 so they dont keep rotating
		if (AutoRotateObject != null) {
			if (RotationsPerMinute == -7f) {
				GetComponent<Image> ().color = UIcolor;
				RotationsPerMinute = 0;
			} 
			if (RotationsPerMinute == 7f) {
				GetComponent<Image> ().color = UIcolor;
				RotationsPerMinute = 0;
			} 

		}

	}
}
	