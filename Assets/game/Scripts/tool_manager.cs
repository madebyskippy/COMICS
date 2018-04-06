using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tool_manager : MonoBehaviour {

	string currentTool;
	Image img;

	// Use this for initialization
	void Start () {
		currentTool = "null";
		img = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = Input.mousePosition;
		pos.z = 10f;
		transform.position = Camera.main.ScreenToWorldPoint(pos);

		if (currentTool == "gutter") {
			img.color = Color.red;
		} else if (currentTool == "sub") {
			img.color = Color.green;
		} else if (currentTool == "expanding") {
			img.color = Color.blue;
		} else if (currentTool == "null") {
			img.color = Color.white;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log (col.tag);
		currentTool = col.tag;
	}

	void OnTriggerExit2D(Collider2D col){
		Debug.Log (col.tag);
		currentTool = "null";
	}
}
