using UnityEngine;
using System.Collections;

public class Glowscript : MonoBehaviour{
	//creates an array of public gameobjects
public GameObject[] objectstoglow;

	void Awake()
		{
		//sets script to inactive
		this.GetComponent<Glowscript> ().enabled = false;
		}

	void Update()
		{
		//renders glow every frame
		glowPower ();
		}
		
	void glowPower()
		{
		//pingpong variable between 0 - 1
		float intensityA = 0f;
		float intensityB = 1f;
		float GlowIntensity = Mathf.PingPong(Time.time, intensityB - intensityA);
		//for each gameobject in the array set glow power equal to pingpong variable
		foreach (GameObject glowobj in objectstoglow) 
			{
				glowobj.GetComponent<Renderer> ().material.SetFloat ("_MKGlowPower", GlowIntensity);
			}
		}

	void OnDisable()
		{
		// disables glow when script is set to false
		foreach (GameObject glowobj in objectstoglow) 
			{
				glowobj.GetComponent<Renderer> ().material.SetFloat ("_MKGlowPower", 0);
			}
		}
		
}