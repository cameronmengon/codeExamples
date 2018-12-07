using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tour_Quiz_cam : MonoBehaviour {
	// here is the class for variables controlling the overall quiz
	[Header("Variables for managing tour/quiz")]
//	public TourGuide tourGuide;
	//object name
	public Text	text;
	public GameObject hoveredGO;
	public GameObject[] UIGameobject;
	//enum for hovering
	public enum HoverState{Hover, None};
	public HoverState hover_state = HoverState.None;
	//booleans for toggling
	public bool AutoRotateToggle = false;
	public bool HideToggle = false;


	// here we have variables for camera movement
	[Header("Camera and user input")]
	public UserCameraMovement cameraMovement;
	// variables for the selector
	public UISelector uiSelector;

	void Start(){
		//creates a list of gameobjects that are used to control the camera
		UIGameobject = GameObject.FindGameObjectsWithTag("ControlsUI");
	
	}
		
	void Update () {
		// move camera every frame user input;
		ControlCamera ();
		// check if the user is looking at a selectable object;
		CheckForLook();
		//check to see if the user has entered a click magnet pull ect..
		CheckForClick();
	
	}
		
	#region UserInput/CameraInput

	void ControlCamera ()
	{  
		//Used for pc development
		#region PC_Web
		Screen.lockCursor = cameraMovement.lockCursor;
		var targetOrientation = Quaternion.Euler (cameraMovement.targetDirection);
		var mouseDelta = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));
		mouseDelta = Vector2.Scale (mouseDelta, new Vector2 (cameraMovement.sensitivity.x * cameraMovement.smoothing.x, cameraMovement.sensitivity.y * cameraMovement.smoothing.y));
		cameraMovement._smoothMouse.x = Mathf.Lerp (cameraMovement._smoothMouse.x, mouseDelta.x, 1f / cameraMovement.smoothing.x);
		cameraMovement._smoothMouse.y = Mathf.Lerp (cameraMovement._smoothMouse.y, mouseDelta.y, 1f / cameraMovement.smoothing.y);
		cameraMovement._mouseAbsolute += cameraMovement._smoothMouse;
		if (cameraMovement.clampInDegrees.x < 180)
			cameraMovement._mouseAbsolute.x = Mathf.Clamp (cameraMovement._mouseAbsolute.x, -cameraMovement.clampInDegrees.x * 0.5f, cameraMovement.clampInDegrees.x * 0.5f);
		var xRotation = Quaternion.AngleAxis (-cameraMovement._mouseAbsolute.y, targetOrientation * Vector3.right);
		cameraMovement.cameraTransform.localRotation = xRotation;
		if (cameraMovement.clampInDegrees.y < 180)
			cameraMovement._mouseAbsolute.y = Mathf.Clamp (cameraMovement._mouseAbsolute.y, -cameraMovement.clampInDegrees.y * 0.5f, cameraMovement.clampInDegrees.y * 0.5f);
		cameraMovement.cameraTransform.localRotation *= targetOrientation;
		var yRotation = Quaternion.AngleAxis (cameraMovement._mouseAbsolute.x, cameraMovement.cameraTransform.InverseTransformDirection (Vector3.up));
		cameraMovement.cameraTransform.localRotation *= yRotation;
		#endregion
	}

	void CheckForLook ()
	{
		RaycastHit hit;
		if (Physics.Raycast (uiSelector.lookObject.position, uiSelector.lookObject.forward, out hit, 100f, uiSelector.selectableObjectsLayer)) {
			//changes crosshair color
			uiSelector.crossHair.material.color = Color.green;
			//sets looked object
			uiSelector.lookedAtObject = hit.collider.gameObject;
			//Tells object if cursor has entered hitbox
			if (hover_state == HoverState.None) {
				hit.collider.SendMessage ("OnCursorEnter", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
				hoveredGO = hit.collider.gameObject;
			}
			//sets hoverstate to hovering
			hover_state = HoverState.Hover;
		//tells object if cursor has left hitbox
		} else {
			if (hover_state == HoverState.Hover) {
				hoveredGO.SendMessage ("OnCursorExit", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
			}
			//sets hover state to null
			hover_state = HoverState.None;
			//sets crosshair to red
			uiSelector.crossHair.material.color = Color.red;
			//sets looked object to nul
			uiSelector.lookedAtObject = null;
		}
		//Tells object if it is currently being hovered over
		if (hover_state == HoverState.Hover && uiSelector.lookedAtObject == hoveredGO) {
			hoveredGO.SendMessage ("OnCursorOver", SendMessageOptions.DontRequireReceiver);
		
		} else {
			//Tells objects that the cursor is looking at nothing
			if (hoveredGO != null) {
				hoveredGO.SendMessage ("OnCursorNull", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
				hover_state = HoverState.None;
			}
		}
	}

	void CheckForClick ()
	{
		// code for Standalone builds
		#if UNITY_STANDALONE
		if (Input.GetMouseButtonDown (0)) {
			UserClick ();
		}
		//to know how long a user holds down a button
		if(Input.GetMouseButtonUp(0)){
			UserHold();
		}
		#endif

		#if UNITY_ANDROID
		if (Input.touchCount>0){
			if (Input.GetTouch(0).phase == TouchPhase.Began){
				UserClick();
			}
			if (Input.GetTouch(0).phase == TouchPhase.Ended){
				UserHold();
			}
		}
		#endif
	}

	void UserHold(){
		//creates a list of UI elements and tells all objects that touch/click has been released
		for(int UI = 0; UI< UIGameobject.Length; UI++){
			UIGameobject [UI].SendMessage ("UpClick", SendMessageOptions.DontRequireReceiver);
		}
	}
		
	void UserClick(){
		// if user is looking at a selectable object when user clicks call the method for selecting objects
		if (uiSelector.lookedAtObject != null) {
			//toggle lookable layer
			LookableLayerSwitch ();
			//tells object it has been clicked on
			uiSelector.lookedAtObject.gameObject.SendMessage ("Click", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
			//does button action based on case
			DoSelectedButton (uiSelector.lookedAtObject.name);
		}
	}
		
	#endregion

	void DoSelectedButton(string buttonName){
		// a switch that calls the appropriate method based on the clicked buttons name
		switch (buttonName){
		case "Skeletal":
			LookableLayerSwitch ();
			break;
		case "Respiratory":
			LookableLayerSwitch ();
			break;
		case "Nervous":
			LookableLayerSwitch ();
			break;
		case "Cardiovascular":
			LookableLayerSwitch ();
			break;
		case "Digestive":
			LookableLayerSwitch ();
			break;
		case "Urinary":
			LookableLayerSwitch ();
			break;
		case "Auto Rotate":
			AutoRotateToggle = !AutoRotateToggle;
			//turns auto rotate on and off
			if (AutoRotateToggle) {
				AutoRotateToggle = true;
				uiSelector.lookedAtObject.gameObject.SendMessage ("AutoRotateON", SendMessageOptions.DontRequireReceiver);
			}else{
				uiSelector.lookedAtObject.gameObject.SendMessage ("AutoRotateOFF",  SendMessageOptions.DontRequireReceiver);
			}
			LookableLayerSwitch ();
			break;
		case "Hide":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("HideAll",  SendMessageOptions.DontRequireReceiver);
			break;
		case "Right Rotate":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("RotateRight",  SendMessageOptions.DontRequireReceiver);
			break;
		case "Left Rotate":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("RotateLeft",  SendMessageOptions.DontRequireReceiver);
			break;
		case "Pan Down":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("PanDown", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
			break;
		case "Pan Up":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("PanUp", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
			break;
		case "Pan Right":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("PanRight", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
			break;
		case "Pan Left":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("PanLeft", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
			break;
		case "Center":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("Center", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
			break;
		case "Zoom In":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("ZoomIn", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
			break;
		case "Zoom Out":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("ZoomOut", uiSelector.lookedAtObject, SendMessageOptions.DontRequireReceiver);
			break;
		case "HideOS":
			LookableLayerSwitch ();
			uiSelector.lookedAtObject.gameObject.SendMessage ("HideAll",  SendMessageOptions.DontRequireReceiver);
			break;
		}
	}

	// switches lookable layer
	public void LookableLayerSwitch(){
		if (uiSelector.selectableObjectsLayer == 0) {
			uiSelector.selectableObjectsLayer = 1 << 8;
		}else{
			uiSelector.selectableObjectsLayer = 0;
		}	
	}
	// gets called when the speaker is done talking
	void DoneSpeaking(){
		MoveBack ();
	}

	void MoveBack(){
		LookableLayerSwitch ();
	}
}