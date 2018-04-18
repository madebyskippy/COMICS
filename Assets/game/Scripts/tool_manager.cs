using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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


		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (hit.collider != null) {
			currentTool = hit.collider.tag;
		} else {
			currentTool = "null";
		}

		if (currentTool == "gutter") {
			img.color = Color.red;
		} else if (currentTool == "sub") {
			img.color = Color.green;
		} else if (currentTool == "pacer") {
			img.color = Color.blue;
		} else if (currentTool == "null") {
			img.color = Color.gray;
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	public string getTool(){
		return currentTool;
	}
}
