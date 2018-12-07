using UnityEngine;
using System.Collections;

//controls object movement

public class MoveBack : MonoBehaviour {
	public Vector3 StartPos;
	public float speed;
	// Use this for initialization
	void Awake () {
		//saves objects starting point
	
			StartPos = gameObject.transform.position;
		
		speed = 5f;

	}
		
	void Update () {
		float step = speed * Time.deltaTime;

		//moves object back to starting position when script is enabled
		transform.position = Vector3.MoveTowards(transform.position,StartPos,step);

		//if the target is back to its start position, this script is disabled.
		if (transform.position == StartPos) {
			gameObject.GetComponent<MoveBack> ().enabled = false;
		}

	}
}
