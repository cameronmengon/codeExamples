using UnityEngine;
using Leap.Unity.PinchUtility.Examples;
using System.Collections;
using UnityEngine.UI;



public class Colorslider : MonoBehaviour {
	public Image image;
	public Material Redgradient;
	public Material Greengradient;
	public Material Bluegradient;
	public Slider RedSlider;
	public Slider GreenSlider;
	public Slider BlueSlider;
	public Color linecolor;
	public PinchDraw pinchdraw;
	private float red ;
	private float blue ;
	private float green ;

	//Custom color picker using Leap motion hand detection see portfolio VR Draw demo 

	//sets colors to starting black position----------------------------------------------------
	void Awake(){
		pinchdraw = GameObject.Find ("Pinch Drawing").GetComponent<PinchDraw>();
		linecolor = Color.black;
		RedLerp ();
		GreenGradient ();
		BlueGradient ();
		ChangeColor ();
	
	}
	//changes color swatch and sets linecolor----------------------------------------------------
	public void ChangeColor(){
		linecolor = new Color (red, green, blue, 1);
		image.color = linecolor;
		SetLineColor ();
	}

	public void SetLineColor(){
		pinchdraw.DrawColor = linecolor;
	}

	//changes image gradient shaders-------------------------------------------------------------
	public void RedLerp(){
		Redgradient.SetColor ("_Color", linecolor - Color.red);
		Redgradient.SetColor ("_Color2", Color.red+linecolor );
		SetLineColor ();
	}

	public void GreenGradient(){
		Greengradient.SetColor ("_Color", linecolor - Color.green);
		Greengradient.SetColor ("_Color2", Color.green +linecolor);
		SetLineColor ();
	}

	public void BlueGradient(){
		Bluegradient.SetColor ("_Color", linecolor - Color.blue);
		Bluegradient.SetColor ("_Color2", Color.blue+linecolor );
		SetLineColor ();
	}
	//set numerical value for slider 0-1---------------------------------------------------------
	public float RedValue {
		get {
			return red;
		}
		set {
			red = value;
		}
	}

	public float GreenValue{
		get {
			return green;
		}
		set {
			green = value;
		}
	}

	public float BlueValue{
		get {
			return blue;
		}
		set {
			blue = value;
		}
	}

	//Color Preset functions--------------------------------------------------------------------
	public void White(){
		linecolor = Color.white;
		image.color = linecolor;
		RedLerp ();
		GreenGradient ();
		BlueGradient ();
		RedSlider.value = 1;
		GreenSlider.value = 1;
		BlueSlider.value = 1;
		SetLineColor ();
	}
	public void Black(){
		linecolor = Color.black;
		image.color = linecolor;
		RedSlider.value = 0;
		GreenSlider.value = 0;
		BlueSlider.value = 0;
	}
	public void Cyan(){
		linecolor = Color.cyan;
		image.color = linecolor;
		RedSlider.value = 0;
		GreenSlider.value = 1;
		BlueSlider.value = 1;
		SetLineColor ();
	}
	public void Red(){
		linecolor = Color.red;
		image.color = linecolor;
		RedSlider.value = 1;
		GreenSlider.value = 0;
		BlueSlider.value = 0;

	}
	public void Blue(){
		linecolor = Color.blue;
		image.color = linecolor;
		RedSlider.value = 0;
		GreenSlider.value = 0;
		BlueSlider.value = 1;
	}
	public void Green(){
		linecolor = Color.green;
		image.color = linecolor;
		RedSlider.value = 0;
		GreenSlider.value = 1;
		BlueSlider.value = 0;
	}
	public void Yellow(){
		linecolor = Color.yellow;
		image.color = linecolor;
		RedSlider.value = 1;
		GreenSlider.value = 1;
		BlueSlider.value = 0f;

	}
	public void Magenta(){
		linecolor = Color.magenta;
		image.color = linecolor;
		RedSlider.value = 1;
		GreenSlider.value = 0;
		BlueSlider.value = 1;
	}
}
